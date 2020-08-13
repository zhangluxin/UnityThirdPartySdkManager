using System;
using System.Collections.Generic;

namespace UnityThirdPartySdkManager.Editor.Configs
{
    /// <summary>
    ///     微信配置
    /// </summary>
    [Serializable]
    public class WeChatConfig
    {
        /// <summary>
        ///     sdk相关文件（只放可编译文件）
        /// </summary>
        public readonly List<string> sdkFileList = new List<string>(new[] {"Plugins/iOS/WeChat/WeChatUtil.mm"});

        /// <summary>
        ///     appid
        /// </summary>
        public string appId;

        /// <summary>
        ///     跳转链接
        /// </summary>
        public List<string> associatedDomains = new List<string>(new[] {"ulink.ilemen.net"});

        /// <summary>
        ///     开启
        /// </summary>
        public bool enable;

        /// <summary>
        ///     pod名
        /// </summary>
        public string pod = "WechatOpenSDK";

        /// <summary>
        ///     scheme
        /// </summary>
        public List<string> schemes = new List<string>(new[] {"weixin", "weixinULAPI"});
    }
}