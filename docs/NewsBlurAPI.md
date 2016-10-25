
<!--链接集合-->
[000]: http://www.shisujie.com/
[001]: http://www.newsblur.com/api
[002]: http://github.com/samuelclay/NewsBlur/tree/master/apps/reader/views.py
[003]: http://github.com/samuelclay/NewsBlur/tree/master/apps/social/views.py
[004]: http://github.com/samuelclay/NewsBlur/tree/master/apps/rss_feeds/views.py
[005]: http://github.com/samuelclay/NewsBlur/tree/master/templates/static/api.yml
[006]: http://github.com/samuelclay/NewsBlur
[007]: mailto:samuel@newsblur.com
[008]: http://www.instapaper.com/api/full

<!--NewsBlur API参考-->

> 原文链接：[The NewsBlur API][001]

# API介绍

NewsBlur是一个个人新闻阅读器，以便于人们进行全球性的新闻获取交流。它重新定义了新闻阅读的方式。

NewsBlur的API提供了以下功能：获取已订阅的订阅源、订阅数量、订阅源的图标、订阅情况统计，以及个人订阅的具体新闻故事。不需要任何API key，但是在使用API端点之前，都需要进行身份验证。另请不要攻击我们的服务器。

我们的整个API都是开源的，包括端点的实现。你可以从以下几个来源获取具体的信息：

* [/reader/views source][002],
* [/social/ views source][003],
* [/rss_feeds/ views source][004], 
* [the API definitions in YAML][005].

我们希望收到pull request。如果你希望添加一个端点、修改输出，或者改进一些地方，你都可以 [NewsBlur's repo on Github][006] 在上面进行处理。

# 关于OAuth认证的说明

NewsBlur支持OAuth，但是要使用OAuth，你必须发邮件给 [**Samuel**][007] 来请求一个客户端ID和Secret。你需要提供NewsBlur的用户名，重定向链接（一般是你软件的网站），以及软件名称。

在使用OAuth认证请求时，类似所有其他OAuth API，你都需要确保HTTP头里都包含认证信息。使用`/oauth/authorize`接口进行OAuth认证。使用 `/oauth/token` 接口接收认证的token（应用于每个请求中）。

当然，你也可以不使用OAuth，不过每次请求都需要传递`newsblur_sessionid`的cookie值——cookie值来源于登录。 

# API 目录

## 认证

* [POST `/api/login`](#login)


# 认证

<h2 id="login">POST /api/login</h2>

* 已有账号登录

参数|描述|默认值|示例
----|---|------|----
username|**必需** 用户名||`samuelclay`
password|**可选** 密码||`new$blur`

### 小贴士
* 如果用户没有设置密码，你不能发送任何密码。这不是[Instapaper][008]——*这个网站api在没有密码的情况下，可以随意发送一个密码字符串*

## POST /api/logout

* 当前登录用户注销登录

## POST /api/signup

* 创建新用户

参数|描述|默认值|示例
----|---|------|----
username|**必要** 用户名||`samuelclay`
password|**可选** 密码||`new$blur`
email|**必要** 邮件地址||`samuel@newsblur.com`

# 订阅

## GET /rss_feeds/search_feed

* 依据网站网址或RSS地址搜索订阅源信息

参数|描述|默认值|示例
---|----|------|----
address|**必要** 搜索RSS和网站网址，并返回订阅源||`techcrunch.com`
offset|**可选** 利用偏移定位到订阅源列表中的某一条||`1`

## GET /reader/feeds

* 获取用户已订阅的订阅源列表
* 包含三个未读数据（positive, neutral, negative——*未明白*），以及网站图表（可选）

参数|描述|默认值|示例
---|----|------|----
include_favicons|**可选** 同时获取网站图标。由于此操作比较耗时，故可以选择关闭。然后再用`/api/v1/feeds/favicons/`来单独请求网站图标。|`false`|'true/false'
flat|**可选** 返回非目录结构的文件夹——即只返回叶子节点的文件夹 。需要在一个层次上显示所有文件夹时会比较有用。|`false`|`true/false`
update_counts|**可选** 强制重新计算所有订阅的未读数量。建议的方法是：使用此接口不进行更新未读数量，然后利用`refresh_feeds`来更新数量。这样可以保证尽快的显示用户订阅数据，然后更新数量。打开此选择将会导致加载缓慢。|`false`|`true/false`

### 提示

* 使用`/reader/refresh_feeds`接口来更新未读数量
* 如果你可以缓存网站图标或者稍后获取它们，那么关闭`include_favicons`

## GET /reader/favicons

* 获取订阅源列表的网站图标列表。配合使用`/reader/feeds` 与`include_favicons=false` ，这样订阅源请求客户以包含较少的数据。在移动设备上会很有用，不过这需要二次请求。

参数|描述|默认值|示例
----|---|------|----
feed_ids|**可选** 订阅源id列表。留空时，返回所有用户已订阅的订阅源的网站图标||`feed_ids=12&feed_ids=24`

### 提示

* 使用内联的图像可以使用如下方法

    <img src="data:image/png;base64,[IMAGE_DATA_STRING]" />

## GET /reader/page/:id

* 获取某个订阅源的原始页面

## GET /reader/refresh_feeds

* 刷新每一个订阅源的未读计数
* 一分钟内至多刷新一次 ———— *不能连续刷新*

## GET /reader/feeds_trainer

* 获取所有常用分类
* 同时包括用户自己的分类

参数|描述|默认值|示例
----|----|-----|----
feed_id|**可选** 针对单一订阅源。留空时，一次性获取所有订阅源的分类||`42`

### 提示

* 如果仅仅需要获取用户的分类，使用`/classifiers/:id`
* 忽略`feed_id`来获取所有订阅的分类

## GET /rss_feeds/statistics/:id

* 某一订阅源的统计与历史数据
* 包括：订阅源的更新频率、网站分类的常见分布、订阅源平均每月有多少新闻/故事，以及最后五个订阅信息及网页的抓取信息（*抓取时间、是否成功等*）

## GET /rss_feeds/feed_autocomplete

* 获取含有搜索词的订阅源列表
* 顺序搜索订阅源地址、链接和标题
* 仅仅显示含有两个以上订阅者的订阅源

参数|描述|默认值|示例
----|---|------|----
term|**可选** 用于搜索订阅源地址、链接和标题的短语/词语||`tech`


# 新闻/故事