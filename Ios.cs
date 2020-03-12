using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor.UnityThirdPartySdkManager
{
    /// <summary>
    /// IOS打包
    /// </summary>
    public static class Ios
    {
        /// <summary>
        /// 执行打包
        /// </summary>
        /// <param name="pathToBuiltProject">打包工程路径</param>
        /// <param name="config">配置</param>
        public static void Build(string pathToBuiltProject, Config config)
        {
            if (!Directory.Exists(pathToBuiltProject))
            {
                Debug.LogError("项目路径不存在");
                return;
            }

            if (config.podList.Length > 0)
                GeneratePodfile(pathToBuiltProject, config);
            ModifyPbxproj(pathToBuiltProject, config);
            ModifyPlist(pathToBuiltProject, config);
        }

        /// <summary>
        /// 生成cocoapod
        /// </summary>
        /// <param name="pathToBuiltProject">打包工程路径</param>
        /// <param name="config">配置</param>
        private static void GeneratePodfile(string pathToBuiltProject, Config config)
        {
            var podfilePath = pathToBuiltProject + "/Podfile";
            if (File.Exists(podfilePath))
            {
                File.Delete(podfilePath);
            }

            var streamWriter = new StreamWriter(podfilePath);
            var allstr = new StringBuilder();
            allstr.Append($"source '{config.podUrl}'\n");
            allstr.Append($"platform :ios, '{config.iosVersion}'\n");
            allstr.Append("\n");
            allstr.Append("target 'UnityFramework' do\n");
            foreach (var pod in config.podList)
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
                    Arguments = "install --project-directory=" + pathToBuiltProject,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                }
            };
            Debug.Log($"执行: {proc.StartInfo.FileName} {proc.StartInfo.Arguments}");
            proc.Start();
            var content = proc.StandardOutput.ReadToEnd();
            Debug.Log(content);
            proc.WaitForExit();
            Debug.Log($"执行: {proc.StartInfo.FileName} {proc.StartInfo.Arguments} 结束");
        }

        /// <summary>
        /// 修改pbxproj文件
        /// </summary>
        /// <param name="pathToBuiltProject">打包工程路径</param>
        /// <param name="config">配置</param>
        private static void ModifyPbxproj(string pathToBuiltProject, Config config)
        {
            var pbxprojPath = Path.Combine(pathToBuiltProject, "Unity-iPhone.xcodeproj", "project.pbxproj");
            var pbxProject = new PBXProject();
            pbxProject.ReadFromFile(pbxprojPath);
            var target = pbxProject.TargetGuidByName("UnityFramework");
            if (!string.IsNullOrEmpty(config.iosSdkPath))
                AddSdkFiles(pbxProject, target, config.iosSdkPath);
            if (!config.bitCode)
                CloseBitCode(pbxProject, target);
            pbxProject.WriteToFile(pbxprojPath);
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
        /// <param name="pathToBuiltProject">打包工程路径</param>
        /// <param name="config">配置</param>
        private static void ModifyPlist(string pathToBuiltProject, Config config)
        {
            var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
            var plistDocument = new PlistDocument();
            plistDocument.ReadFromFile(plistPath);
            if (plistDocument.root["LSApplicationQueriesSchemes"].AsArray() == null)
            {
                plistDocument.root.CreateArray("LSApplicationQueriesSchemes");
            }

            plistDocument.root["LSApplicationQueriesSchemes"].AsArray().AddString("");
            plistDocument.WriteToFile(plistPath);
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
    }
}