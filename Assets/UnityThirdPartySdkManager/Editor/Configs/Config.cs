using System;

namespace UnityThirdPartySdkManager.Editor.Configs
{
    /// <summary>
    ///     config结构体
    /// </summary>
    [Serializable]
    public class Config
    {
        #region 通用配置

        /// <summary>
        ///     百度地图
        /// </summary>
        public BaiduMap baiduMap;

        /// <summary>
        ///     支持百度地图
        /// </summary>
        public bool enableBaiduMap;

        /// <summary>
        ///     支持极光推送
        /// </summary>
        public bool enableJpush;

        /// <summary>
        ///     支持聊呗
        /// </summary>
        public bool enableLiaoBe;

        /// <summary>
        ///     支持talkingdata
        /// </summary>
        public bool enableTalkingData;

        /// <summary>
        ///     支持微信
        /// </summary>
        public bool enableWeChat;

        /// <summary>
        ///     极光推送
        /// </summary>
        public JPush jPush;

        /// <summary>
        ///     聊呗
        /// </summary>
        public LiaoBe liaoBe;

        /// <summary>
        ///     talkingdata
        /// </summary>
        public TalkingData talkingData;

        /// <summary>
        ///     微信
        /// </summary>
        public WeChat weChat;

        #endregion

        #region Ios配置

        /// <summary>
        ///     cocoapods
        /// </summary>
        public string podUrl;

        /// <summary>
        ///     cocoapods最低版本
        /// </summary>
        public string iosVersion;

        /// <summary>
        ///     是否支持bitcode
        /// </summary>
        public bool bitCode;

        #endregion
    }
}