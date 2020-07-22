# UnityThirdPartySdkManager
Unity第三方sdk管理

# 作用
用于Unity项目接入第三方sdk，例如微信，百度地图等

## 支持版本
`>=Unity 2019.4`

## 使用方法
项目根目录下的`Packages/manifest.json`中添加PackageManager依赖
```json
{ 
  "dependencies": {
    "com.lucy.thirdparty-sdk": "https://github.com/zhangluxin/UnityThirdPartySdkManager.git#1.0.2"
  }
}
```
启动项目，选择`自定义工具->SDK配置->SDK管理`进行配置

根目录（*和Assets平级*）创建`Sdk`目录，目录结构为
```
|——Ios
|——Android
```
`Ios`目录存放ios sdk相关文件和库

`Android`目录存放ios sdk相关文件和库

## 开发者
![名字](https://wx3.sinaimg.cn/mw690/8a323e5cly1gcr72ahmikj203h01qgli.jpg)

![邮件](https://wx4.sinaimg.cn/mw690/8a323e5cly1gcr72ahqsej209l01jq31.jpg)