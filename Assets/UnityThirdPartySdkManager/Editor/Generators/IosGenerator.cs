using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor.iOS.Xcode;
using UnityThirdPartySdkManager.Editor.Configs;
using Debug = UnityEngine.Debug;

namespace UnityThirdPartySdkManager.Editor.Generators
{
    /// <summary>
    ///     ios生成器
    /// </summary>
    public class IosGenerator : Generator
    {
        public IosGenerator(string pathToBuiltProject, Config config) : base(pathToBuiltProject, config)
        {
        }

        /// <summary>
        ///     生成
        /// </summary>
        public override void Run()
        {
            if (!Directory.Exists(pathToBuiltProject))
            {
                Debug.LogError("项目路径不存在");
                return;
            }

            GeneratePretreatment();
            if (config.ios.pod.enable) GeneratePodfile();

            ModifyPbxproj();
            ModifyPlist();
        }

        /// <summary>
        ///     生成pod文件
        /// </summary>
        private void GeneratePodfile()
        {
            var podfilePath = Path.Combine(pathToBuiltProject, "Podfile");
            if (File.Exists(podfilePath)) File.Delete(podfilePath);

            var streamWriter = new StreamWriter(podfilePath);
            var allstr = new StringBuilder();
            allstr.Append($"source '{config.ios.pod.podUrl}'\n");
            allstr.Append($"platform :ios, '{config.ios.pod.podIosVersion}'\n");
            allstr.Append("\n");
            allstr.Append("target 'UnityFramework' do\n");
            foreach (var pod in GeneratePodList()) allstr.Append($"pod '{pod}'\n");

            allstr.Append("end\n");
            streamWriter.Write(allstr);
            streamWriter.Close();
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = "/usr/local/bin/pod",
                    Arguments = $"install --project-directory={pathToBuiltProject}",
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                }
            };
            proc.Start();
            var content = proc.StandardOutput.ReadToEnd();
            Debug.Log(content);
            proc.WaitForExit();
            Debug.Log("pod执行完成!");
        }

        /// <summary>
        ///     生成pod列表
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GeneratePodList()
        {
            var list = new List<string>();
            if (config.weChat.enable) list.Add(config.weChat.pod);
            if (config.baiduMap.enable) list.Add(config.baiduMap.pod);

            return list;
        }

        /// <summary>
        ///     修改pbxproj文件
        /// </summary>
        private void ModifyPbxproj()
        {
            var pbxprojPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            var pbxProject = new PBXProject();
            pbxProject.ReadFromFile(pbxprojPath);
            var frameworkTarget = pbxProject.GetUnityFrameworkTargetGuid();
            if (!config.ios.bitCode)
                CloseBitCode(pbxProject, frameworkTarget);
            var prejectTarget = pbxProject.GetUnityMainTargetGuid();
            pbxProject.AddCapability(prejectTarget, PBXCapabilityType.InAppPurchase, "ios.entitlements");
            var capManager = new ProjectCapabilityManager(pbxprojPath, "ios.entitlements", null, prejectTarget);
            AddCapability(capManager);
            pbxProject.WriteToFile(pbxprojPath);
        }

        /// <summary>
        ///     关掉bitcode
        /// </summary>
        private static void CloseBitCode(PBXProject pbxProject, string target)
        {
            pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
        }

        /// <summary>
        ///     修改plist
        /// </summary>
        private void ModifyPlist()
        {
            var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
            var plistDocument = new PlistDocument();
            plistDocument.ReadFromFile(plistPath);
            var schemes = GenerateSchemeList();
            if (schemes.Count > 0)
                AddSchemes(plistDocument, schemes);
            var urlTypes = GenerateUrlTypes();
            if (urlTypes.Count > 0)
                AddUrlTypes(plistDocument, urlTypes);
            plistDocument.WriteToFile(plistPath);
        }

        /// <summary>
        ///     生成scheme列表
        /// </summary>
        /// <returns></returns>
        private List<string> GenerateSchemeList()
        {
            var list = new List<string>();
            if (config.weChat.enable) list.AddRange(config.weChat.schemes);

            return list;
        }

        /// <summary>
        ///     生成urlType列表
        /// </summary>
        /// <returns></returns>
        private List<UrlType> GenerateUrlTypes()
        {
            var list = new List<UrlType>();
            list.AddRange(config.ios.urlTypes);
            if (config.weChat.enable)
            {
                var idUrltype = new UrlType {id = "wexin", urlScheme = config.weChat.appId};
                list.Add(idUrltype);
                var linkUrltype = new UrlType
                    {id = "ulink", urlScheme = string.Join(",", config.weChat.associatedDomains.ToArray())};
                list.Add(linkUrltype);
            }

            if (config.baiduMap.enable)
            {
                var idUrltype = new UrlType {id = "baidu", urlScheme = $"baidu{config.baiduMap.iOSAppId}"};
                list.Add(idUrltype);
            }

            return list;
        }

        /// <summary>
        ///     添加Capability
        /// </summary>
        private void AddCapability(ProjectCapabilityManager capManager)
        {
            var arr = GenerateAssociatedDomainList().ToArray();
            for (var i = 0; i < arr.Length; i++)
                if (!arr[i].StartsWith("applinks:"))
                    arr[i] = $"applinks:{arr[i]}";

            capManager.AddAssociatedDomains(arr);
            capManager.WriteToFile();
        }

        /// <summary>
        ///     生成跳转链接列表
        /// </summary>
        /// <returns></returns>
        private List<string> GenerateAssociatedDomainList()
        {
            var list = new List<string>();
            if (config.weChat.enable) list.AddRange(config.weChat.associatedDomains);

            return list;
        }

        /// <summary>
        ///     修改预处理
        /// </summary>
        private void GeneratePretreatment()
        {
            var pretreatmentFile = Path.Combine(pathToBuiltProject, "Libraries", "UnityThirdPartySdkManager", "Plugins",
                "iOS", "Common", "Pretreatment.h");
            var streamWriter = new StreamWriter(pretreatmentFile);
            var allstr = new StringBuilder();
            allstr.Append("#pragma once\n");
            allstr.Append("\n");
            allstr.Append("#ifndef SDK_PRETREATMENT_DEF\n");
            allstr.Append("\n");
            allstr.Append("#define SDK_PRETREATMENT_DEF true\n");
            allstr.Append($"#define SDK_WECHAT_ENABLE {config.weChat.enable.ToString().ToLower()}\n");
            allstr.Append($"#define SDK_BAIDUMAP_ENABLE {config.baiduMap.enable.ToString().ToLower()}\n");
            allstr.Append("\n");
            allstr.Append("#endif\n");
            allstr.Append("\n");
            streamWriter.Write(allstr);
            streamWriter.Close();
        }

        /// <summary>
        ///     添加Application Queries Schemes
        /// </summary>
        /// <param name="plistDocument">plist</param>
        /// <param name="schemes">schemes</param>
        private static void AddSchemes(PlistDocument plistDocument, IEnumerable<string> schemes)
        {
            if (!plistDocument.root.values.ContainsKey("LSApplicationQueriesSchemes"))
                plistDocument.root.CreateArray("LSApplicationQueriesSchemes");

            var queriesSchemes = plistDocument.root["LSApplicationQueriesSchemes"].AsArray();
            foreach (var scheme in schemes)
                if (queriesSchemes.values.All(element => element.AsString() != scheme))
                    queriesSchemes.AddString(scheme);
        }

        /// <summary>
        ///     添加Url Type
        /// </summary>
        /// <param name="plistDocument">plist</param>
        /// <param name="urlTypes">urlTypes</param>
        private static void AddUrlTypes(PlistDocument plistDocument, List<UrlType> urlTypes)
        {
            if (!plistDocument.root.values.ContainsKey("CFBundleURLTypes"))
                plistDocument.root.CreateArray("CFBundleURLTypes");

            foreach (var urlType in urlTypes)
            {
                if (char.IsDigit(urlType.urlScheme[0]))
                {
                    Debug.LogError("urlType不能以数字开头");
                    continue;
                }

                var typeDic = plistDocument.root["CFBundleURLTypes"].AsArray().AddDict();
                typeDic.SetString("CFBundleTypeRole", "Editor");
                typeDic.SetString("CFBundleURLName", urlType.id);
                typeDic.CreateArray("CFBundleURLSchemes");
                typeDic["CFBundleURLSchemes"].AsArray().AddString(urlType.urlScheme);
            }
        }
    }
}