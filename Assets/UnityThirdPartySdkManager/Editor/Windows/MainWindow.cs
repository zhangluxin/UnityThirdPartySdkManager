using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityThirdPartySdkManager.Editor.Configs;

namespace UnityThirdPartySdkManager.Editor.Windows
{
    public class MainWindow : EditorWindow
    {
        #region 变量定义

        /// <summary>
        /// 配置文件目录
        /// </summary>
        private static readonly string ConfigFilePath = Path.Combine(Application.persistentDataPath, "Sdks");

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string ConfigFile = Path.Combine(ConfigFilePath, "config.json");

        /// <summary>
        /// 配置信息
        /// </summary>
        private Config _config;

        #endregion

        #region 功能入口

        /// <summary>
        ///     sdk参数配置
        /// </summary>
        [MenuItem("Tools/SDK配置/SDK管理")]
        public static void OpenMainConfigWindow()
        {
            var window = GetWindow<MainWindow>();
            window.titleContent = new GUIContent("SDK配置主窗口");
            window.Show();


            Debug.Log(JsonUtility.ToJson(ReadConfig()));
        }

        /// <summary>
        ///     sdk参数配置
        /// </summary>
        [MenuItem("Tools/SDK配置/还原配置文件")]
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
            // var config = ReadConfig();
            switch (target)
            {
                case BuildTarget.iOS:
                    // Ios.Build(pathToBuiltProject, config);
                    break;
                case BuildTarget.Android:
                    break;
            }
        }

        #endregion

        #region 生命周期

        private void OnEnable()
        {
            _config = ReadConfig();
        }

        private void OnGUI()
        {
            InitUi();
        }


        private void OnDestroy()
        {
            SaveConfig();
        }

        #endregion

        #region 界面方法

        /// <summary>
        /// 初始化ui
        /// </summary>
        private void InitUi()
        {
            InitSdkList();
        }

        /// <summary>
        /// 初始化sdk列表
        /// </summary>
        private void InitSdkList()
        {
            GUILayout.Label("开启Sdk列表", EditorStyles.boldLabel);
            _config.enableWeChat = EditorGUILayout.Toggle("微信", _config.enableWeChat);
            if (_config.enableWeChat)
            {
                _config.weChat.appId = EditorGUILayout.TextField("AppId", _config.weChat.appId);
                _config.weChat.ulink = EditorGUILayout.TextField("Ulink", _config.weChat.ulink);
            }

            _config.enableBaiduMap = EditorGUILayout.Toggle("百度地图", _config.enableBaiduMap);
            _config.enableJpush = EditorGUILayout.Toggle("极光推送", _config.enableJpush);
            _config.enableTalkingData = EditorGUILayout.Toggle("TalkingData", _config.enableTalkingData);
            _config.enableLiaoBe = EditorGUILayout.Toggle("聊呗", _config.enableLiaoBe);
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
        /// 创建配置文件
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
        /// 保存配置
        /// </summary>
        private void SaveConfig()
        {
            var jsonStr = JsonUtility.ToJson(_config);
            File.WriteAllText(ConfigFile, jsonStr);
        }

        #endregion
    }
}