using System;
using System.Collections.Generic;

namespace UnityThirdPartySdkManager.Editor.Configs
{
    /// <summary>
    /// cocospad配置
    /// </summary>
    [Serializable]
    public class PodConfig
    {
        /// <summary>
        ///     是否启用
        /// </summary>
        public bool enable = true;

        /// <summary>
        ///     pod库支持的ios版本
        /// </summary>
        public string podIosVersion = "10.0";

        /// <summary>
        ///     pod库地址
        /// </summary>
        public string podUrl = "https://github.com/CocoaPods/Specs.git";
    }
}