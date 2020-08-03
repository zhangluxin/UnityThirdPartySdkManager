using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace UnityThirdPartySdkManager.Editor.Configs
{
    /// <summary>
    /// ios配置
    /// </summary>
    [Serializable]
    public class IosConfig
    {
        /// <summary>
        /// Cocoapods 配置
        /// </summary>
        public Cocoapods cocoapods;

        /// <summary>
        /// scheme列表
        /// </summary>
        public List<string> schemes;

        /// <summary>
        /// urlType列表
        /// </summary>
        public List<UrlType> urlTypes;

        /// <summary>
        /// 是否开启bitcode
        /// </summary>
        public bool bitCode;

        /// <summary>
        /// sdk路径
        /// </summary>
        public string sdkPath = "Sdk/Ios";
    }
}