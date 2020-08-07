#import <Foundation/Foundation.h>
#import "CommonUtil.h"
#import "WXApi.h"

NS_ASSUME_NONNULL_BEGIN

// 微信工具
@interface WeChatUtil : NSObject <WXApiDelegate>

// appid
@property(strong, nonatomic) NSString *appId;

// ulink
@property(strong, nonatomic) NSString *ulink;

// 单例
+ (instancetype)getInstance;

@end

NS_ASSUME_NONNULL_END
