using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;
using UnityThirdPartySdkManager.Editor.Configs;
using UnityThirdPartySdkManager.Editor.Generator;

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

        /// <summary>
        ///  pod列表
        /// </summary>
        private ReorderableList _podList;

        /// <summary>
        ///  scheme列表
        /// </summary>
        private ReorderableList _schemeList;

        /// <summary>
        ///  pod列表
        /// </summary>
        private ReorderableList _urlTypeList;

        /// <summary>
        /// 跳转地址列表
        /// </summary>
        private ReorderableList _associatedDomainsList;

        /// <summary>
        /// 标签页位置（0为ios，1为安卓）
        /// </summary>
        private int _selectedToolBarId;

        #endregion

        #region 功能入口

        /// <summary>
        ///     sdk参数配置
        /// </summary>
        [MenuItem("自定义工具/SDK配置/SDK管理")]
        public static void OpenMainConfigWindow()
        {
            var window = GetWindow<MainWindow>();
            window.titleContent = new GUIContent("SDK配置");
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
            var config = ReadConfig();
            switch (target)
            {
                case BuildTarget.iOS:
                    new IosGenerator(pathToBuiltProject, config).Run();
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
            InitAllList();
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
            GUILayout.Label("Sdk列表", EditorStyles.boldLabel);
            _selectedToolBarId = GUILayout.Toolbar(_selectedToolBarId, new[] {"Ios", "Android"});
            GUILayout.Label("______________________________________________________________________");
            switch (_selectedToolBarId)
            {
                case 0:
                    MakeupIosUi();
                    break;
                case 1:
                    MakeupAndroidUi();
                    break;
            }
        }

        /// <summary>
        /// 初始化所有List
        /// </summary>
        private void InitAllList()
        {
            InitPodList();
            InitSchemeList();
            InitUrlTypeList();
            InitAssociatedDomainsList();
        }

        #region Ios界面

        /// <summary>
        /// 初始化pod列表
        /// </summary>
        private void InitPodList()
        {
            _podList = new ReorderableList(_config.ios.cocoapods.podList, _config.ios.cocoapods.podList.GetType())
            {
                drawHeaderCallback = rect => { GUI.Label(rect, "pod列表"); },
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    _config.ios.cocoapods.podList[index] =
                        EditorGUI.TextField(rect, _config.ios.cocoapods.podList[index]);
                },
                onAddCallback = list => { _config.ios.cocoapods.podList.Add(""); }
            };
        }

        /// <summary>
        /// 初始化scheme列表
        /// </summary>
        private void InitSchemeList()
        {
            _schemeList = new ReorderableList(_config.ios.schemes, _config.ios.schemes.GetType())
            {
                drawHeaderCallback = rect => { GUI.Label(rect, "scheme列表"); },
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    _config.ios.schemes[index] = EditorGUI.TextField(rect, _config.ios.schemes[index]);
                },
                onAddCallback = list => { _config.ios.schemes.Add(""); }
            };
        }

        /// <summary>
        /// 初始化urlType列表
        /// </summary>
        private void InitUrlTypeList()
        {
            _urlTypeList = new ReorderableList(_config.ios.urlTypes, _config.ios.urlTypes.GetType())
            {
                drawHeaderCallback = rect => { GUI.Label(rect, "urlType列表"); },
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    var rect1 = new Rect(rect.x, rect.y, rect.width / 2, rect.height);
                    var rect2 = new Rect(rect.x + rect.width / 2, rect.y, rect.width / 2, rect.height);
                    _config.ios.urlTypes[index].id = EditorGUI.TextField(rect1, _config.ios.urlTypes[index].id);
                    _config.ios.urlTypes[index].urlScheme =
                        EditorGUI.TextField(rect2, _config.ios.urlTypes[index].urlScheme);
                },
                onAddCallback = list => { _config.ios.urlTypes.Add(new UrlType()); }
            };
        }

        /// <summary>
        /// 初始化跳转连接列表
        /// </summary>
        private void InitAssociatedDomainsList()
        {
            _associatedDomainsList =
                new ReorderableList(_config.ios.associatedDomains, _config.ios.associatedDomains.GetType())
                {
                    drawHeaderCallback = rect => { GUI.Label(rect, "跳转链接列表"); },
                    drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        _config.ios.associatedDomains[index] =
                            EditorGUI.TextField(rect, _config.ios.associatedDomains[index]);
                    },
                    onAddCallback = list => { _config.ios.associatedDomains.Add(""); }
                };
        }

        /// <summary>
        /// ios 配置
        /// </summary>
        private void MakeupIosUi()
        {
            MakeupPodUi();
            _schemeList.DoLayoutList();
            _urlTypeList.DoLayoutList();
            _associatedDomainsList.DoLayoutList();
            _config.ios.bitCode = EditorGUILayout.Toggle("支持BitCode", _config.ios.bitCode);
        }

        /// <summary>
        /// android 配置
        /// </summary>
        private void MakeupAndroidUi()
        {
        }

        /// <summary>
        /// pod列表
        /// </summary>
        private void MakeupPodUi()
        {
            _config.ios.cocoapods.enable =
                EditorGUILayout.BeginToggleGroup("开启CocoaPods", _config.ios.cocoapods.enable);
            EditorGUILayout.Foldout(_config.ios.cocoapods.enable, "CocoaPods配置");
            EditorGUI.indentLevel++;
            if (_config.ios.cocoapods.enable)
            {
                _config.ios.cocoapods.podUrl = EditorGUILayout.TextField("pod地址", _config.ios.cocoapods.podUrl);
                _config.ios.cocoapods.podIosVersion =
                    EditorGUILayout.TextField("ios版本", _config.ios.cocoapods.podIosVersion);
                _podList.DoLayoutList();
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
        }

        #endregion

        #region Android界面

        #endregion

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