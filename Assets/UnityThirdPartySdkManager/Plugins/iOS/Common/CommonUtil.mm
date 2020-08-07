#import "CommonUtil.h"

// 通用方法
@implementation CommonUtil

// 从urlType里面取id
+ (NSString *)getIdFromUrlTypes:(NSString *)key {
    NSDictionary *infoPlistDict = [[NSBundle mainBundle] infoDictionary];
    NSArray *urlTypesArr = infoPlistDict[@"CFBundleURLTypes"];
    if (urlTypesArr != nil) {
        for (NSUInteger i = 0; i < [urlTypesArr count]; i++) {
            NSDictionary *urlTypeDict = urlTypesArr[i];
            NSString *name = urlTypeDict[@"CFBundleURLName"];
            NSString *val = [urlTypeDict[@"CFBundleURLSchemes"] objectAtIndex:0];
            if ([name compare:key] == 0) {
                return val;
            }
        }
    }
    return nil;
}

@end
