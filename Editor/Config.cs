using System;
using System.Collections.Generic;

namespace UnityThirdPartySdkManager.Editor
{
    /// <summary>
    ///     config结构体
    /// </summary>
    [Serializable]
    public struct Config
    {
        #region IOS

        /// <summary>
        ///     cocoapods
        /// </summary>
        public string podUrl;

        /// <summary>
        ///     cocoapods最低版本
        /// </summary>
        public string iosVersion;

        /// <summary>
        ///     cocoapods需要加载的库
        /// </summary>
        public string[] podList;

        /// <summary>
        ///     ios sdk路径
        /// </summary>
        public string iosSdkPath;

        /// <summary>
        ///     是否支持bitcode
        /// </summary>
        public bool bitCode;

        /// <summary>
        ///     Application Queries Schemes
        /// </summary>
        public string[] schemes;

        /// <summary>
        ///     URL Types
        /// </summary>
        public Dictionary<string, string> urlTypes;

        #endregion

        #region Android

        #endregion
    }
}