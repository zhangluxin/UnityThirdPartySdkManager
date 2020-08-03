using System;
using System.Collections.Generic;

namespace UnityThirdPartySdkManager.Editor.Configs
{
    /// <summary>
    /// cocoa
    /// </summary>
    [Serializable]
    public class Cocoapods
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool enable;

        /// <summary>
        /// pod库地址
        /// </summary>
        public string podUrl = "https://github.com/CocoaPods/Specs.git";

        /// <summary>
        /// pod库支持的ios版本
        /// </summary>
        public string podIosVersion = "10.0";

        /// <summary>
        /// pod列表
        /// </summary>
        public List<string> podList;
    }
}