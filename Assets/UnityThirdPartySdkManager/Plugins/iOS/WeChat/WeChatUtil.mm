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

}

- (void)onReq:(BaseReq *)req {

}


// 微信sdk注册
extern "C" void IosWeChatRegister() {
    [WXApi registerApp:[WeChatUtil getInstance].appId universalLink:[WeChatUtil getInstance].ulink];
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
