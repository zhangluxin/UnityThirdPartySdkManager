# UnityThirdPartySdkManager
Unity第三方sdk管理

# 作用
用于Unity项目接入第三方sdk，例如微信，百度地图等

## 使用方法
作为子模块添加到`Assets/Editor/UnityThirdPartySdkManager`

根目录（*和Assets平级*）创建`Sdk`目录，目录结构为
```
|——Ios
|——Android
|——config.json
```
`Ios`目录存放ios sdk相关文件和库

`Android`目录存放android sdk相关文件和库

`config.json`文件例子如下
```json
{
  "podUrl": "https://gitee.com/cg16888/yygame_pod.git",
  "iosVersion": "10.0",
  "podList": [
    "WeChat",
    "BMKLocationKit",
    "LiaoBe",
    "YunCeng"
  ],
  "iosSdkPath": "Sdk/Ios",
  "bitCode": false,
  "schemes": [
    "openapp",
    "weixin",
    "weixinUlink"
  ],
  "urlTypes": {
    "weixin": "wx111111111111111111",
    "baidu": "baidu1111111111111111"
  }
}
```
## config释义
 
值 | 释义 | 备注
---- | ----- | ------  
podUrl | cocoapods库url | 公开库为`https://github.com/CocoaPods/Specs.git` 
iosVersion | ios版本 | 不要低于Unity里的设置最小版本
podList | cocoapods引用库列表 | 不用的记得删除
iosSdkPath | ios sdk 保存路径 | 相对根目录的路径
bitCode | 是否支持bitcode | 有些傻*第三方一直不支持
schemes | 添加ApplicationQueriesSchemes | 不用的记得删除
urlTypes | 添加urlType | 不用的记得删除
    


## 开发者
![名字](https://wx3.sinaimg.cn/mw690/8a323e5cly1gcr72ahmikj203h01qgli.jpg)

![邮件](https://wx4.sinaimg.cn/mw690/8a323e5cly1gcr72ahqsej209l01jq31.jpg)