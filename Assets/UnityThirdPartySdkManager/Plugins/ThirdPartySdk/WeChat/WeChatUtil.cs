using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityThirdPartySdkManager.Plugins.ThirdPartySdk.WeChat
{
    /// <summary>
    /// 微信sdk方法
    /// </summary>
    public static class WeChatUtil
    {
        #region Ios方法

        /// <summary>
        /// 微信sdk注册
        /// </summary>
        [DllImport("__Internal")]
        private static extern void IosWeChatRegister();

        /// <summary>
        /// 取微信appId
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        private static extern string IosWeChatGetAppId();

        /// <summary>
        /// 微信是否安装
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        private static extern bool IosWeChatIsInstalled();

        /// <summary>
        /// 拉起微信登录
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        private static extern void IosWeChatRedirectLogin();

        /// <summary>
        /// 拉起微信分享网址
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        private static extern void IosWeChatShareUrl(string url, string title, string description, string path,
            int sceneType);

        /// <summary>
        /// 拉起微信分享文本
        /// </summary>
        [DllImport("__Internal")]
        private static extern void IosWeChatShareText(string text, int sceneType);

        /// <summary>
        /// 拉起微信分享图片
        /// </summary>
        [DllImport("__Internal")]
        private static extern void IosWeChatSharePic(string filePath, int sceneType);

        /// <summary>
        /// 订阅
        /// </summary>
        [DllImport("__Internal")]
        private static extern void IosWeChatSubscription(string templateId, string reserved, int sceneType);

        /// <summary>
        /// 分享到小程序
        /// </summary>
        [DllImport("__Internal")]
        private static extern void IosWeChatShareMiniProgram(string userName, string path, int miniProgramType);

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
                    // todo
                    break;
                case RuntimePlatform.IPhonePlayer:
                    IosWeChatRegister();
                    break;
            }
        }

        /// <summary>
        /// 取微信appId
        /// </summary>
        public static string GetAppId()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return ""; // todo
                case RuntimePlatform.IPhonePlayer:
                    return IosWeChatGetAppId();
                default:
                    return "";
            }
        }

        /// <summary>
        /// 微信是否安装
        /// </summary>
        /// <returns></returns>
        public static bool IsInstalled()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return false; // todo
                case RuntimePlatform.IPhonePlayer:
                    return IosWeChatIsInstalled();
                default:
                    return false;
            }
        }

        /// <summary>
        /// 拉起微信登录
        /// </summary>
        /// <returns></returns>
        public static void RedirectLogin()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    // todo
                    break;
                case RuntimePlatform.IPhonePlayer:
                    IosWeChatRedirectLogin();
                    break;
            }
        }

        /// <summary>
        /// 分享网址
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="title">标题</param>
        /// <param name="description">文本内容</param>
        /// <param name="sceneType">发送场景 0：对话，1：朋友圈，2：收藏，3：指定联系人</param>
        /// <param name="path">缩略图地址（默认是icon）</param>
        public static void ShareUrl(string url, string title, string description, int sceneType,
            string path = "Icon.png")
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    // todo
                    break;
                case RuntimePlatform.IPhonePlayer:
                    IosWeChatShareUrl(url, title, description, path, sceneType);
                    break;
            }
        }

        /// <summary>
        /// 分享文本
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="sceneType">发送场景 0：对话，1：朋友圈，2：收藏，3：指定联系人</param>
        public static void ShareText(string text, int sceneType)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    // todo
                    break;
                case RuntimePlatform.IPhonePlayer:
                    IosWeChatShareText(text, sceneType);
                    break;
            }
        }

        /// <summary>
        /// 分享图片
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <param name="sceneType">发送场景 0：对话，1：朋友圈，2：收藏，3：指定联系人</param>
        public static void SharePic(string filePath, int sceneType)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    // todo
                    break;
                case RuntimePlatform.IPhonePlayer:
                    IosWeChatSharePic(filePath, sceneType);
                    break;
            }
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="templateId">标识订阅场值</param>
        /// <param name="reserved">订阅消息模板 ID</param>
        /// <param name="sceneType">用于保持请求和回调的状态</param>
        public static void Subscription(string templateId, string reserved, int sceneType)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    // todo
                    break;
                case RuntimePlatform.IPhonePlayer:
                    IosWeChatSubscription(templateId, reserved, sceneType);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">拉起的小程序的username</param>
        /// <param name="path">拉起小程序页面的可带参路径</param>
        /// <param name="miniProgramType">拉起小程序的类型 0：正式版，1：开发版，2：体验版</param>
        public static void ShareMiniProgram(string userName, string path, int miniProgramType = 0)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    // todo
                    break;
                case RuntimePlatform.IPhonePlayer:
                    IosWeChatShareMiniProgram(userName, path, miniProgramType);
                    break;
            }
        }

        #endregion
    }
}