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
        ///     是否开启bitcode
        /// </summary>
        public bool bitCode = true;

        /// <summary>
        ///     Cocoapods 配置
        /// </summary>
        public PodConfig pod;

        /// <summary>
        ///     urlType列表
        /// </summary>
        public List<UrlType> urlTypes;
    }
}