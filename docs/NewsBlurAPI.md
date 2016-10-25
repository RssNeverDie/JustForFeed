
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