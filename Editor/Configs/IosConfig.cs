using System;
using System.Collections.Generic;

namespace UnityThirdPartySdkManager.Editor.Configs
{
    /// <summary>
    ///     ios配置
    /// </summary>
    [Serializable]
    public class IosConfig
    {
        /// <summary>
        ///     跳转链接
        /// </summary>
        public List<string> associatedDomains;

        /// <summary>
        ///     是否开启bitcode
        /// </summary>
        public bool bitCode;

        /// <summary>
        ///     Cocoapods 配置
        /// </summary>
        public Cocoapods cocoapods;

        /// <summary>
        ///     scheme列表
        /// </summary>
        public List<string> schemes;

        /// <summary>
        ///     sdk路径
        /// </summary>
        public string sdkPath = "Sdk/Ios";

        /// <summary>
        ///     urlType列表
        /// </summary>
        public List<UrlType> urlTypes;
    }
}