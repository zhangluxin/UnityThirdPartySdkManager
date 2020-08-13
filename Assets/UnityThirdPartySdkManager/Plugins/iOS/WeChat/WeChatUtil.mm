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
    _appId = [CommonUtil getIdFromUrlTypes:@"wexin"];
    NSString *ulink = [CommonUtil getIdFromUrlTypes:@"ulink"];
    _ulink = [NSString stringWithFormat:@"https://%@/", ulink];
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(handleOpenURL:) name:@"kUnityOnOpenURL" object:nil];
    return self;
}

- (bool)handleOpenURL:(NSURL *)url {
    return [WXApi handleOpenURL:url delegate:self];
}

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
        const char *code = [[NSString stringWithFormat:@"%d", launchMiniProgramResp.errCode] UTF8String];
        UnitySendMessage("SdkManager", "LaunchMiniResp", code);
    }
}

- (void)onReq:(BaseReq *)req {
    if ([req isKindOfClass:[GetMessageFromWXReq class]]) {
    } else if ([req isKindOfClass:[ShowMessageFromWXReq class]]) {
    } else if ([req isKindOfClass:[LaunchFromWXReq class]]) {
        LaunchFromWXReq *launchFromWXReq = (LaunchFromWXReq *) req;
        const char *code = [[[NSString alloc] initWithString:launchFromWXReq.message.messageExt] UTF8String];
        UnitySendMessage("SdkManager", "LaunchMiniReq", code);
    }
}


// 微信sdk注册
extern "C" void IosWeChatRegister() {
    [WXApi registerApp:[WeChatUtil getInstance].appId universalLink:[WeChatUtil getInstance].ulink];
    NSLog(@"---------------IosWeChatRegister!!");
}

// 取得微信appid
extern "C" char *IosWeChatGetAppId() {
    return strdup([[WeChatUtil getInstance].appId UTF8String]);
}

// 取微信是否安装
extern "C" bool IosWeChatIsInstalled() {
    return [WXApi isWXAppInstalled];
}

// 拉起微信登录
extern "C" void IosWeChatRedirectLogin() {
    SendAuthReq *req = [[SendAuthReq alloc] init];
    req.scope = @"snsapi_userinfo";
    [WXApi sendAuthReq:req viewController:UnityGetGLViewController() delegate:[WeChatUtil getInstance] completion:nil];
}

@end
