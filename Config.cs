using System;
using System.IO;
using UnityEngine;

namespace Editor.UnityThirdPartySdkManager
{
    /// <summary>
    /// config结构体
    /// </summary>
    [Serializable]
    public struct Config
    {
        #region IOS

        /// <summary>
        /// cocoapods
        /// </summary>
        public string podUrl;

        /// <summary>
        /// cocoapods最低版本
        /// </summary>
        public string iosVersion;

        /// <summary>
        /// cocoapods需要加载的库
        /// </summary>
        public string[] podList;

        /// <summary>
        /// ios sdk路径
        /// </summary>
        public string iosSdkPath;

        /// <summary>
        /// 是否支持bitcode
        /// </summary>
        public bool bitCode;

        /// <summary>
        /// Application Queries Schemes
        /// </summary>
        public string[] schemes;

        #endregion

        #region Android

        #endregion
    }
}