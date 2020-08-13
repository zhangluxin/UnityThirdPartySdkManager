#pragma once

#import <Foundation/Foundation.h>
#import "CommonUtil.h"
#import "Pretreatment.h"

#if SDK_WECHAT_ENABLE
#import "WXApi.h"
#endif

NS_ASSUME_NONNULL_BEGIN

#if SDK_WECHAT_ENABLE
// 微信工具
@interface WeChatUtil : NSObject <WXApiDelegate>
#else
// 微信工具
@interface WeChatUtil : NSObject
#endif

// appid
@property(strong, nonatomic) NSString *appId;

// ulink
@property(strong, nonatomic) NSString *ulink;

// 单例
+ (instancetype)getInstance;

@end

NS_ASSUME_NONNULL_END
