using System;

namespace UnityThirdPartySdkManager.Editor.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [Serializable]
    public class Config
    {
        /// <summary>
        /// ios配置
        /// </summary>
        public IosConfig ios;

        /// <summary>
        /// 安卓配置
        /// </summary>
        public AndroidConfig android;

        /// <summary>
        /// 微信配置
        /// </summary>
        public WeChatConfig weChat;
    }
}