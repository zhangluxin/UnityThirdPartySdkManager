using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using UnityThirdPartySdkManager.Editor.Configs;
using Debug = UnityEngine.Debug;

namespace UnityThirdPartySdkManager.Editor.Generator
{
    /// <summary>
    /// ios生成器
    /// </summary>
    public class IosGenerator
    {
        /// <summary>
        /// 配置
        /// </summary>
        private readonly Config _config;

        /// <summary>
        /// 项目路径
        /// </summary>
        private readonly string _pathToBuiltProject;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="pathToBuiltProject">项目路径</param>
        /// <param name="config">配置</param>
        public IosGenerator(string pathToBuiltProject, Config config)
        {
            _config = config;
            _pathToBuiltProject = pathToBuiltProject;
        }

        /// <summary>
        /// 生成
        /// </summary>
        public void Run()
        {
            if (!Directory.Exists(_pathToBuiltProject))
            {
                Debug.LogError("项目路径不存在");
                return;
            }

            if (_config.podList.Count > 0)
            {
                GeneratePodfile();
            }

            ModifyPbxproj();
            ModifyPlist();
        }

        /// <summary>
        /// 生成pod文件
        /// </summary>
        private void GeneratePodfile()
        {
            var podfilePath = Path.Combine(_pathToBuiltProject, "Podfile");
            if (File.Exists(podfilePath))
            {
                File.Delete(podfilePath);
            }

            var streamWriter = new StreamWriter(podfilePath);
            var allstr = new StringBuilder();
            allstr.Append($"source '{_config.podUrl}'\n");
            allstr.Append($"platform :ios, '{_config.iosVersion}'\n");
            allstr.Append("\n");
            allstr.Append("target 'UnityFramework' do\n");
            foreach (var pod in _config.podList)
            {
                allstr.Append($"pod '{pod}'\n");
            }

            allstr.Append("end\n");
            streamWriter.Write(allstr);
            streamWriter.Close();
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = "/usr/local/bin/pod",
                    Arguments = $"install --project-directory=${_pathToBuiltProject}",
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
        /// 修改pbxproj文件
        /// </summary>
        private void ModifyPbxproj()
        {
            var pbxprojPath = Path.Combine(_pathToBuiltProject, "Unity-iPhone.xcodeproj", "project.pbxproj");
            var pbxProject = new PBXProject();
            pbxProject.ReadFromFile(pbxprojPath);
            var target = pbxProject.TargetGuidByName("UnityFramework");
            if (!string.IsNullOrEmpty(_config.iosSdkPath))
                AddSdkFiles(pbxProject, target, _config.iosSdkPath);
            if (!_config.bitCode)
                CloseBitCode(pbxProject, target);
            pbxProject.WriteToFile(pbxprojPath);
        }

        /// <summary>
        /// 添加sdk文件
        /// </summary>
        /// <param name="pbxProject">打包工程路径</param>
        /// <param name="target">配置的target("默认为UnityFramework")</param>
        /// <param name="iosSdkPath">sdk路径</param>
        private static void AddSdkFiles(PBXProject pbxProject, string target, string iosSdkPath)
        {
            var sdkPath = Path.Combine(Application.dataPath, "..", iosSdkPath);
            if (!Directory.Exists(sdkPath)) return;
            var directory = new DirectoryInfo(sdkPath);
            AddFiles(directory, pbxProject, target, "Sdks");
        }

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="directory">目录</param>
        /// <param name="pbxProject">打包工程</param>
        /// <param name="target">配置的target("默认为UnityFramework")</param>
        /// <param name="path">路径</param>
        private static void AddFiles(DirectoryInfo directory, PBXProject pbxProject, string target, string path)
        {
            foreach (var file in directory.GetFiles())
            {
                var fileGuid = pbxProject.AddFile(file.FullName, $"{path}/{file.Name}", PBXSourceTree.Build);
                pbxProject.AddFileToBuild(target, fileGuid);
            }

            foreach (var directoryInfo in directory.GetDirectories())
            {
                AddFiles(directoryInfo, pbxProject, target, $"{path}/{directoryInfo.Name}");
            }
        }

        /// <summary>
        /// 关掉bitcode
        /// </summary>
        private static void CloseBitCode(PBXProject pbxProject, string target)
        {
            pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
        }

        /// <summary>
        /// 修改plist
        /// </summary>
        private void ModifyPlist()
        {
            var plistPath = Path.Combine(_pathToBuiltProject, "Info.plist");
            var plistDocument = new PlistDocument();
            plistDocument.ReadFromFile(plistPath);
            if (_config.schemes.Count > 0)
                AddSchemes(plistDocument, _config.schemes);
            if (_config.urlTypes.Count > 0)
                AddUrlTypes(plistDocument, _config.urlTypes);
            plistDocument.WriteToFile(plistPath);
        }
        
        /// <summary>
        /// 添加Application Queries Schemes
        /// </summary>
        /// <param name="plistDocument">plist</param>
        /// <param name="schemes">schemes</param>
        private static void AddSchemes(PlistDocument plistDocument, IEnumerable<string> schemes)
        {
            // todo
            if (!plistDocument.root.values.ContainsKey("LSApplicationQueriesSchemes"))
            {
                plistDocument.root.CreateArray("LSApplicationQueriesSchemes");
            }

            var queriesSchemes = plistDocument.root["LSApplicationQueriesSchemes"].AsArray();
            foreach (var scheme in schemes)
            {
                var isExist = queriesSchemes.values.Any(element => element.AsString() == scheme);
                if (!isExist)
                {
                    queriesSchemes.AddString(scheme);
                }
            }
        }

        /// <summary>
        /// 添加Url Type
        /// </summary>
        /// <param name="plistDocument">plist</param>
        /// <param name="urlTypes">urlTypes</param>
        private static void AddUrlTypes(PlistDocument plistDocument, List<string> urlTypes)
        {
            // todo
            // if (!plistDocument.root.values.ContainsKey("CFBundleURLTypes"))
            // {
            //     plistDocument.root.CreateArray("CFBundleURLTypes");
            // }
            //
            // foreach (var urlType in urlTypes)
            // {
            //     if (char.IsDigit(urlType.Value[0]))
            //     {
            //         Debug.LogError("urlType不能以数字开头");
            //         continue;
            //     }
            //
            //     var typeDic = plistDocument.root["CFBundleURLTypes"].AsArray().AddDict();
            //     typeDic.SetString("CFBundleTypeRole", "Editor");
            //     typeDic.SetString("CFBundleURLName", urlType.Key);
            //     typeDic.CreateArray("CFBundleURLSchemes");
            //     typeDic["CFBundleURLSchemes"].AsArray().AddString(urlType.Value);
            // }
        }
    }
}