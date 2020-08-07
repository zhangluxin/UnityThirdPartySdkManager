using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityThirdPartySdkManager.Editor.Configs;
using UnityThirdPartySdkManager.Editor.Generators;

namespace UnityThirdPartySdkManager.Editor.Windows
{
    public class MainWindow : EditorWindow
    {
        #region 变量定义

        /// <summary>
        ///     配置文件目录
        /// </summary>
        private static readonly string ConfigFilePath = Path.Combine(Application.persistentDataPath, "Sdks");

        /// <summary>
        ///     配置文件路径
        /// </summary>
        private static readonly string ConfigFile = Path.Combine(ConfigFilePath, "config.json");

        /// <summary>
        ///     配置信息
        /// </summary>
        private Config _config;

        #endregion

        #region 功能入口

        /// <summary>
        ///     sdk参数配置
        /// </summary>
        [MenuItem("自定义工具/SDK配置/SDK管理")]
        private static void ShowWindow()
        {
            var window = GetWindow<MainWindow>();
            window.titleContent = new GUIContent("SDK配置");
            window.Show();
        }

        /// <summary>
        ///     sdk参数配置
        /// </summary>
        [MenuItem("自定义工具/SDK配置/还原配置文件")]
        public static void ClearSdk()
        {
            CreateConfig();
        }

        /// <summary>
        ///     打包
        /// </summary>
        /// <param name="target">目标平台</param>
        /// <param name="pathToBuiltProject">打包路径</param>
        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            var config = ReadConfig();
            Generator generator;
            switch (target)
            {
                case BuildTarget.iOS:
                    generator = new IosGenerator(pathToBuiltProject, config);
                    break;
                case BuildTarget.Android:
                    generator = new AndroidGenerator(pathToBuiltProject, config);
                    break;
                default:
                    Debug.LogWarning("其他平台打包");
                    return;
            }

            generator.Run();
        }

        #endregion

        #region 生命周期

        private void OnEnable()
        {
            _config = ReadConfig();
            // InitAllList();
        }

        private void OnGUI()
        {
            GenerateUi();
        }


        private void OnDestroy()
        {
            SaveConfig();
        }

        #endregion

        #region 界面方法

        /// <summary>
        /// 生成ui
        /// </summary>
        private void GenerateUi()
        {
            GUILayout.Label("Sdk列表", EditorStyles.boldLabel);
            GenerateWechat();
        }

        private void GenerateWechat()
        {
            _config.weChat.enable = EditorGUILayout.BeginToggleGroup("微信", _config.weChat.enable);
            EditorGUI.indentLevel++;
            if (_config.weChat.enable)
            {
                _config.weChat.appId = EditorGUILayout.TextField("AppId", _config.weChat.appId);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
        }

        #endregion

        #region 工具方法

        /// <summary>
        ///     读取配置
        /// </summary>
        /// <returns></returns>
        private static Config ReadConfig()
        {
            try
            {
                var configJson = File.ReadAllText(ConfigFile);
                return JsonUtility.FromJson<Config>(configJson);
            }
            catch (Exception)
            {
                Debug.LogError("读取config配置文件出错");
                throw;
            }
        }

        /// <summary>
        ///     创建配置文件
        /// </summary>
        private static void CreateConfig()
        {
            if (!Directory.Exists(ConfigFilePath))
                Directory.CreateDirectory(ConfigFilePath);

            if (File.Exists(ConfigFile))
                File.Delete(ConfigFile);
            // Debug.Log(ConfigFile);
            var config = new Config();
            var jsonStr = JsonUtility.ToJson(config);
            File.WriteAllText(ConfigFile, jsonStr);
        }

        /// <summary>
        ///     保存配置
        /// </summary>
        private void SaveConfig()
        {
            var jsonStr = JsonUtility.ToJson(_config);
            File.WriteAllText(ConfigFile, jsonStr);
        }

        #endregion
    }
}