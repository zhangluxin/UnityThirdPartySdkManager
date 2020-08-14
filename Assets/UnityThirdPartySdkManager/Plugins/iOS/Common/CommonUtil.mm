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

+ (UIImage *)imageCompressWithSimple:(UIImage *)image {
    CGFloat width = image.size.width;
    CGFloat height = image.size.height;
    CGFloat newWidth;
    CGFloat newHeight;
    if (width > height) {
        newWidth = 300;
        newHeight = 300 * height / width;
    } else {
        newHeight = 300;
        newWidth = 300 * width / height;
    }
    CGSize size;
    size.width = newWidth;
    size.height = newHeight;
    UIGraphicsBeginImageContext(size);
    [image drawInRect:CGRectMake(0, 0, size.width, size.height)];
    UIImage *newImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    return newImage;
}

@end
