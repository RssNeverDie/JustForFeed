[000]: http://www.shisujie.com

> 本文托管地址：<http://git.oschina.net/huaxia283611/JustForFeed/tree/master/docs/ThirdPartyAPI.md>

# RSS阅读器开发信息整合

名称|开源及费用|API|官方支持平台|语言|描述
----|-----|-----|-------|-------|---
[Feedly](http://www.feedly.com/)|否，高级收费|<https://developer.feedly.com/>|Web/IOS/Android/Kindle|支持中文
[Reader](http://reader.aol.com/)|否|<http://reader.aol.com/api>|Web/IOS/Android|支持中文
[NewsBlur](http://www.newsblur.com/)|[开源](https://github.com/samuelclay/NewsBlur)，高级收费|<http://www.newsblur.com/api>|Web/IOS/Android|英文
[Inoreader](http://www.inoreader.com/)|否，高级收费|<http://www.inoreader.com/developers/>|Web/IOS/Android/WP|支持中文
[Reader](http://reeder.io)|[开源](https://github.com/sosedoff/reeder)|<http://reeder.io/api>|未知|英文|官网已无法访问，应已不在维护，纯粹因为开源整理进来
[Feedbin](https://feedbin.com/)|[开源](https://github.com/feedbin/feedbin)，收费|<https://github.com/feedbin/feedbin-api#readme>|Web|英文

> 关于其他大部分rss阅读器，很多都是依赖以上几个后台api服务  
> 曾经霸主：Google Reader —— 谷歌服务调整被砍


# .NET开发参考

## 针对win10/UMP开发

* RSS/Atom 订阅源——<https://msdn.microsoft.com/zh-cn/windows/uwp/networking/web-feeds>
* SyndicationFeed Class——<https://msdn.microsoft.com/zh-cn/library/windows/apps/windows.web.syndication.syndicationfeed.aspx>
* 官方示例 Windows-appsample-rssreader —— <https://github.com/Microsoft/Windows-appsample-rssreader>

## 其他

* 由于现在微软.net framework提供了`SyndicationFeed`订阅信息解析类，故我们自己无需再去专门写解析。

# 个人开发 - 暂未明确定位，故有时调整会比较大 - 偏向学习

* 只为订阅阅读器 —— <http://git.oschina.net/huaxia283611/JustForFeed>
    * 开发语言平台——wpf，.net 4.5，C#
    * 初步开发部分内容参考了微软ump的官方示例

# 其他内容

## 关于RSS/Atom

> 简单理解：RSS与Atom就是两中信息传输格式（XML格式），而且非常相似。虽说Atom像是RSS的进化版本，但我们提到订阅时，更多的会说到RSS（有时还隐含表述了Atom）。   
> 另注：此Atom非Github的Atom，未避免更多的误解，在提及订阅的时候，直接用RSS，而不提及Atom

* 概念：没有概念，就是特殊xml文档格式的定义，其他都是扯
* 格式：见：<http://git.oschina.net/huaxia283611/JustForFeed> 中docs文件夹下的[RSSAndAtomFormat.md](http://git.oschina.net/huaxia283611/JustForFeed/tree/master/docs/RSSAndAtomFormat.md)文档

## 关于新闻组

> 关于新闻组，开发相关内容较少，故在此额外添加，以作后续参考  
> 如果你能够去墙外，直接看 [Google Groups](https://groups.google.com)

* 知识点：Usenet、NNTP
* 参考项目：
    * [Nntp Client Library](https://nntpclientlib.codeplex.com/)
    * [Indy](http://www.indyproject.org/)——mono推荐[Indy.Sockets](https://github.com/mono/website/blob/gh-pages/docs/tools+libraries/libraries/index.md#indysockets)
    * <https://github.com/pmengal/MailSystem.NET> —— 含有NNTP内容 
* 开发经验参考：
    * <http://tatmingstudio.blogspot.com/2007/11/aspnet-nntp-newsgroup-reader-sharpnext.html>


***
掘客：[奇葩史][000]