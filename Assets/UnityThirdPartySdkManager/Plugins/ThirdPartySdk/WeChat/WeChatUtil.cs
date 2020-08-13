using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityThirdPartySdkManager.Plugins.ThirdPartySdk.WeChat
{
    /// <summary>
    /// 微信sdk方法
    /// </summary>
    public struct WeChatUtil
    {
        #region Ios方法

        /// <summary>
        /// 微信sdk注册
        /// </summary>
        [DllImport("__Internal")]
        private static extern void IosWeChatRegister();

        #endregion

        #region Android方法

        #endregion

        #region C#方法

        /// <summary>
        /// 微信sdk注册
        /// </summary>
        public static void Register()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    break;
                case RuntimePlatform.IPhonePlayer:
                    IosWeChatRegister();
                    break;
            }
        }

        #endregion
    }
}