using System;
using System.Collections.Generic;

namespace UnityThirdPartySdkManager.Editor.Configs
{
    /// <summary>
    ///     config结构体
    /// </summary>
    [Serializable]
    public class Config
    {
        /// <summary>
        /// 支持微信
        /// </summary>
        public bool enableWeChat;

        /// <summary>
        /// 微信
        /// </summary>
        public WeChat weChat;

        /// <summary>
        /// 支持百度地图
        /// </summary>
        public bool enableBaiduMap;

        /// <summary>
        /// 支持极光推送
        /// </summary>
        public bool enableJpush;

        /// <summary>
        /// 支持talkingdata
        /// </summary>
        public bool enableTalkingData;

        /// <summary>
        /// 支持聊呗
        /// </summary>
        public bool enableLiaoBe;
    }
}