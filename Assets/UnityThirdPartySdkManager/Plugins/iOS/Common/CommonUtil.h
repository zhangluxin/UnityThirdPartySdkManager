#pragma once

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface CommonUtil : NSObject

// 从urlType里面取id
+ (NSString *)getIdFromUrlTypes:(NSString *)key;

// 图片压缩
+ (UIImage *)imageCompressWithSimple:(UIImage *)image;

@end

NS_ASSUME_NONNULL_END
