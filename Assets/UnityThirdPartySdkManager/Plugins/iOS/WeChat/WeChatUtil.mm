#import "WeChatUtil.h"

// 微信sdk
@implementation WeChatUtil

// 单例
+ (instancetype)getInstance {
    static WeChatUtil *_wxInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        if (_wxInstance == nil) {
            _wxInstance = [[WeChatUtil alloc] init];
        }
    });
    return _wxInstance;
}

- (id)init {
#if SDK_WECHAT_ENABLE
    _appId = [CommonUtil getIdFromUrlTypes:@"wexin"];
    NSString *ulink = [CommonUtil getIdFromUrlTypes:@"ulink"];
    _ulink = [NSString stringWithFormat:@"https://%@/", ulink];
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(handleOpenURL:) name:@"kUnityOnOpenURL" object:nil];
#endif
    return self;
}

- (bool)handleOpenURL:(NSURL *)url {
#if SDK_WECHAT_ENABLE
    return [WXApi handleOpenURL:url delegate:self];
#else
    return true;
#endif
}

#if SDK_WECHAT_ENABLE

- (void)onResp:(BaseResp *)resp {
    if ([resp isKindOfClass:[SendMessageToWXResp class]]) {
        SendMessageToWXResp *sendMessageToWXResp = (SendMessageToWXResp *) resp;
        const char *code = [[NSString stringWithFormat:@"%d", sendMessageToWXResp.errCode] UTF8String];
        UnitySendMessage("SdkManager", "WechatShareResp", code);
    } else if ([resp isKindOfClass:[SendAuthResp class]]) {
        SendAuthResp *sendAuthResp = (SendAuthResp *) resp;
        const char *code;
        if (sendAuthResp.errCode != 0) {
            code = [[NSString stringWithFormat:@"%d", sendAuthResp.errCode] UTF8String];
            UnitySendMessage("SdkManager", "WechatLoginFailResp", code);
        } else {
            code = [[NSString stringWithFormat:@"%@", sendAuthResp.code] UTF8String];
            UnitySendMessage("SdkManager", "WechatLoginResp", code);
        }
    } else if ([resp isKindOfClass:[WXLaunchMiniProgramResp class]]) {
        WXLaunchMiniProgramResp *launchMiniProgramResp = (WXLaunchMiniProgramResp *) resp;
        const char *code = [[NSString stringWithFormat:@"%@", launchMiniProgramResp.extMsg] UTF8String];
        UnitySendMessage("SdkManager", "LaunchMiniResp", code);
    }
}

#endif

// 微信sdk注册
extern "C" void IosWeChatRegister() {
#if SDK_WECHAT_ENABLE
    [WXApi registerApp:[WeChatUtil getInstance].appId universalLink:[WeChatUtil getInstance].ulink];
#endif
}

// 取得微信appid
extern "C" char *IosWeChatGetAppId() {
#if SDK_WECHAT_ENABLE
    return strdup([[WeChatUtil getInstance].appId UTF8String]);
#else
    return strdup("");
#endif
}

// 取微信是否安装
extern "C" bool IosWeChatIsInstalled() {
#if SDK_WECHAT_ENABLE
    return [WXApi isWXAppInstalled];
#else
    return false;
#endif
}

// 拉起微信登录
extern "C" void IosWeChatRedirectLogin() {
#if SDK_WECHAT_ENABLE
    SendAuthReq *req = [[SendAuthReq alloc] init];
    req.scope = @"snsapi_userinfo";
    [WXApi sendAuthReq:req viewController:UnityGetGLViewController() delegate:[WeChatUtil getInstance] completion:nil];
#endif
}

// 拉起微信分享网址
extern "C" void IosWeChatShareUrl(const char *url, const char *title, const char *description, const char *path, int sceneType) {
#if SDK_WECHAT_ENABLE
    WXMediaMessage *message = [WXMediaMessage message];
    message.title = [NSString stringWithUTF8String:title];
    message.description = [NSString stringWithUTF8String:description];
    UIImage *appIcon = [UIImage imageNamed:[NSString stringWithUTF8String:path]];
    [message setThumbImage:appIcon];
    WXWebpageObject *ext = [WXWebpageObject object];
    ext.webpageUrl = [NSString stringWithUTF8String:url];
    message.mediaObject = ext;
    SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.message = message;
    req.scene = sceneType;
    [WXApi sendReq:req completion:nil];
#endif
}

// 拉起微信分享文本
extern "C" void IosWeChatShareText(const char *text, int sceneType) {
#if SDK_WECHAT_ENABLE
    SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
    req.text = [NSString stringWithUTF8String:text];
    req.bText = YES;
    req.scene = sceneType;
    [WXApi sendReq:req completion:nil];
#endif
}

// 拉起微信分享图片
extern "C" void IosWeChatSharePic(const char *filePath, int sceneType) {
#if SDK_WECHAT_ENABLE
    WXMediaMessage *message = [WXMediaMessage message];
    WXImageObject *ext = [WXImageObject object];
    ext.imageData = [NSData dataWithContentsOfFile:[NSString stringWithUTF8String:filePath]];
    UIImage *image = [UIImage imageWithData:ext.imageData];
    ext.imageData = UIImageJPEGRepresentation(image, 0.6f);
    message.mediaObject = ext;
    UIImage *thunbImage = [CommonUtil imageCompressWithSimple:image];
    [message setThumbImage:thunbImage];
    SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.message = message;
    req.scene = sceneType;
    [WXApi sendReq:req completion:nil];
#endif
}

// 微信订阅
extern "C" void IosWeChatSubscription(const char *templateId, const char *reserved, int sceneType) {
#if SDK_WECHAT_ENABLE
    WXSubscribeMsgReq *req = [[WXSubscribeMsgReq alloc] init];
    req.scene = static_cast<UInt32>(sceneType);
    req.templateId = [NSString stringWithUTF8String:templateId];
    req.reserved = [NSString stringWithUTF8String:reserved];
    [WXApi sendReq:req completion:nil];
#endif
}

// 分享到小程序
extern "C" void IosWeChatShareMiniProgram(const char *userName, const char *path, int miniProgramType) {
#if SDK_WECHAT_ENABLE
    WXLaunchMiniProgramReq *launchMiniProgramReq = [WXLaunchMiniProgramReq object];
    launchMiniProgramReq.userName = [NSString stringWithUTF8String:userName];
    launchMiniProgramReq.path = [NSString stringWithUTF8String:path];
    switch (miniProgramType) {
        case 0:
            launchMiniProgramReq.miniProgramType = WXMiniProgramTypeRelease;
            break;
        case 1:
            launchMiniProgramReq.miniProgramType = WXMiniProgramTypeTest;
            break;
        case 2:
            launchMiniProgramReq.miniProgramType = WXMiniProgramTypePreview;
            break;
        default:
            break;
    }
    [WXApi sendReq:launchMiniProgramReq completion:nil];
#endif
}

@end
