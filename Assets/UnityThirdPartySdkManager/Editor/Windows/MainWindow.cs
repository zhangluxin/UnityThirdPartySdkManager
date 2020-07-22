using System;
using System.IO;
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
            MakeupUi();
        }


        private void OnDestroy()
        {
            SaveConfig();
        }

        #endregion

        #region 界面方法

        /// <summary>
        ///     构建ui
        /// </summary>
        private void MakeupUi()
        {
            GUILayout.Label("开启Sdk列表", EditorStyles.boldLabel);
            MakeupWeChatUi();
            MakeupBaiduMapUi();
            MakeupTalkingDataUi();
            MakeupJPushUi();
            MakeupLiaoBeUi();
        }

        /// <summary>
        ///     微信
        /// </summary>
        private void MakeupWeChatUi()
        {
            _config.enableWeChat = EditorGUILayout.BeginToggleGroup("微信", _config.enableWeChat);
            EditorGUI.indentLevel++;
            _config.weChat.appId = EditorGUILayout.TextField("AppId", _config.weChat.appId);
            _config.weChat.ulink = EditorGUILayout.TextField("Ulink", _config.weChat.ulink);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
        }

        /// <summary>
        ///     百度地图
        /// </summary>
        private void MakeupBaiduMapUi()
        {
            _config.enableBaiduMap = EditorGUILayout.BeginToggleGroup("百度地图", _config.enableBaiduMap);
            EditorGUI.indentLevel++;
            _config.baiduMap.iosAppId = EditorGUILayout.TextField("Ios AppId", _config.baiduMap.iosAppId);
            _config.baiduMap.androidAppId = EditorGUILayout.TextField("Android AppId", _config.baiduMap.androidAppId);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
        }

        /// <summary>
        ///     talkingdata
        /// </summary>
        private void MakeupTalkingDataUi()
        {
            _config.enableTalkingData = EditorGUILayout.BeginToggleGroup("TalkingData", _config.enableTalkingData);
            EditorGUI.indentLevel++;
            _config.talkingData.appId = EditorGUILayout.TextField("AppId", _config.talkingData.appId);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
        }

        /// <summary>
        ///     极光推送
        /// </summary>
        private void MakeupJPushUi()
        {
            _config.enableJpush = EditorGUILayout.BeginToggleGroup("极光推送", _config.enableJpush);
            EditorGUI.indentLevel++;
            _config.jPush.appId = EditorGUILayout.TextField("AppId", _config.jPush.appId);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
        }

        /// <summary>
        ///     聊呗
        /// </summary>
        private void MakeupLiaoBeUi()
        {
            _config.enableLiaoBe = EditorGUILayout.BeginToggleGroup("聊呗", _config.enableLiaoBe);
            EditorGUI.indentLevel++;
            _config.liaoBe.appId = EditorGUILayout.TextField("AppId", _config.liaoBe.appId);
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