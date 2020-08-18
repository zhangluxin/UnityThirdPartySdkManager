using System;

namespace UnityThirdPartySdkManager.Editor.Configs
{
    /// <summary>
    ///     百度地图配置
    /// </summary>
    [Serializable]
    public class BaiduMapConfig
    {
        /// <summary>
        ///     开启
        /// </summary>
        public bool enable;

        /// <summary>
        ///     appid
        /// </summary>
        public string iOSAppId;

        /// <summary>
        ///     appid
        /// </summary>
        public string androidAppId;

        /// <summary>
        ///     pod名
        /// </summary>
        public string pod = "BMKLocationKit";
    }
}