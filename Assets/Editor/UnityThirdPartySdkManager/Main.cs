using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Editor.UnityThirdPartySdkManager
{
    /// <summary>
    /// 主入口
    /// </summary>
    public static class Main
    {
        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="target">目标平台</param>
        /// <param name="pathToBuiltProject">打包路径</param>
        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            var config = ReadConfig();
            switch (target)
            {
                case BuildTarget.iOS:
                    Ios.Build(pathToBuiltProject, config);
                    break;
                case BuildTarget.Android:
                    break;
            }
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <returns></returns>
        private static Config ReadConfig()
        {
            var configPath = Path.Combine(Application.dataPath, "..", "Sdk", "config.json");
            try
            {
                var configJson = File.ReadAllText(configPath);
                return JsonUtility.FromJson<Config>(configJson);
            }
            catch (Exception)
            {
                Debug.LogError("读取config配置文件出错");
                throw;
            }
        }
    }
}