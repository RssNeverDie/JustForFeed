
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


# 新闻/故事/文章


# 社交功能


# 订阅源管理

## POST /reader/add_url

* 通过url地址添加订阅。url可以是RSS订阅地址，也可以是网站地址。

参数|描述|默认值|示例
----|----|-----|----
url|**必要** RSS源或网站地址||`http://blog.newsblur.com`
folder|**可选** 需要添加到哪个目录，忽略则使用顶级目录|`[Top Level]`|`Blogs`

### 提示

* 如果你仅仅想在网站上显示添加网址对话框，可以使用以下URL：`http://www.newsblur.com/?url=%u` 。 ——*个人觉得用api开发的人一般不会使用到此提示内容*

## POST /reader/add_folder

* 添加新的文件夹

参数|描述|默认值|示例
---|----|---|-----
folder|**必要** 文件夹名称||`Photo-blogs Extraordinaire`
parent_folder|**可选** 需要将文件夹添加到哪个目录的下一级，忽略则使用顶级目录|`[Top Level]`|`All Blogs`

## POST /reader/move_feed_to_folder

* 将订阅源移动到另一个文件夹

参数|描述|默认值|示例
----|---|-----|----
feed_id|**必要** 订阅源标识id||`12`
in_folder|**必要** 当前订阅源所在的目录。如果一个订阅源在多个文件中存在，需要此来确定具体移动哪一个。||`Blogs`
to_folder|**必要** 将要把订阅源移动到哪一个文件夹||`Tumblrs`

### 提示

* 将文件夹留空，则表示指定的是顶级目录。

## POST /reader/move_folder_to_folder

* 将文件夹移动到另一个文件中。

参数|描述|默认值|示例
----|----|----|----
folder_name|**必要** 要移动的文件夹名称||`Tumblrs`
in_folder|**必要** 当前文件夹所在的文件夹目录。以此避免在多个文件中中有重名的文件夹（尽量避免此类状况）||`Blogs`
to_folder|**必要** 将要移动文件夹的目标文件夹||`Daily Blogs`

### 提示

* 文件夹名称留空时，表示使用顶级目录。

## POST /reader/rename_feed

* 重命名订阅源标题。仅用户自己可以看到新标题。

参数|描述|默认值|示例
----|-----|----|----
feed_title|**必要** 新的标题名称||`NYTimes`
feed_id|**必要** 要重命名的订阅源标识id||`42`

## POST /reader/delete_feed

* 取消一个订阅。从文件夹中删除订阅
* 设置`in_folder`参数，从正确的文件夹中移除订阅 —— 以防用户在多个文件夹中添加了订阅

参数|描述|默认值|示例
----|----|-----|----
feed_id|**必要** 要移除的订阅源id||`42`
in_folder|**可选** 订阅源所在的目录||`News`

## POST /reader/rename_folder

* 重命名文件夹

参数|描述|默认值|示例
---|----|----|----
folder\_to_rename|**必要** 初始文件夹名称||`Photoblogs`
new\_folder_name|**必要** 新的文件夹名称||`East Coast Photoblogs`
in_folder|**必要** 文件夹所处的目录——用于定位到正确的文件夹||`Blogs`

## POST /reader/delete_folder

* 删除一个文件夹，并同时取消内部所有订阅。

参数|描述|默认值|示例
----|-----|----|----
folder\_to_delete|**必要** 要删除的文件夹名称||`Photoblogs`
in_folder|**可选** 文件夹所处的目录。忽略则使用顶级目录||`Blogs`
feed_id|**可选** 要删除的订阅源标识列表。文件夹内部的订阅源同时删除。||`feed_id=12&feed_id=24`

## POST /reader/save_feed_order

* 重新排序订阅源，同时也可以改变订阅源所在的文件夹
* 整个文件夹结构都需要序列化

参数|描述|默认值|示例
----|----|----|-----
folder|**必要** 包含文件夹和订阅源id信息的对象||`[12, 24, 36, {'Blogs': [56, 67, 78,{'Photoblogs': [42]}]}]`


# 智能分类

## GET /classifier/:id

* 获取用户网站的智能分类
* 仅仅含有用户自己的分类。要获取流行分类，使用 `/reader/feeds_trainer` 。

### 提示

* 所有分类数据在获取订阅源`/reader/feed/:id`时，都一起下发
* 在获取订阅源信息后，没有必要再调用此接口。除非你还没有调用过订阅源信息获取。

## POST /classifier/save

* 为订阅保存智能分类（标签，标题，作者和订阅）

参数|描述|默认值|示例
----|---|----|----
feed_id|**必要** 订阅信息ID||`42`
like\_[TYPE]|**可选** 类型内容相似匹配 <- [tag, author, title, feed]||`like_author=Samuel Clay`
dislike\_[TYPE]|**可选** 类型内容不符合匹配 <- [tag, author, title, feed]||`dislike_title=New York Yankees`
remove\_like\_[TYPE]|**可选** 清除分类比较条件||`remove_like_author=Samuel Clay`
remove\_dislike\_[TYPE]|**可选** 功能类似 `remove_like_[TYPE]`||`remove_dislike_title=New York Yankees`

### 提示

* 此接口不同于其他接口。它需要大量的参数键值
* 你可以发送任意键值列表。如： `like_tag[]=tech&like_tag[]=mobile`

# 导入导出

## GET /import/opml_export

* 下载含有订阅源和文件夹信息的OPML备份文件
* 文件格式xml，内容包括订阅信息的文件夹及订阅源信息。多用于导入到另一个RSS阅读器。

## POST /import/opml_upload

* 上传OPML文件

参数|描述|默认值|示例
----|----|----|----
file|**必要** xml格式的OPML文件||	


# API 准则和服务条款

NewsBlur的API提供了以下功能：获取已订阅的订阅源、订阅数量、订阅源的图标、订阅情况统计，以及个人订阅的具体新闻故事。不需要任何API key，但是在使用API端点之前，都需要进行身份验证。另请不要攻击我们的服务器。

如果你的项目或应用允许用户与NewsBlur进行数据交互，你需要指明使用NewsBlur作为数据源。

本API可以用于商业用途，这意味着你可以从使用过本API的项目获得收入。但是，你不能利用从NewsBlur API获取的任何数据进行广告售卖。实际上是，你可以通过你的应用或服务获得收入，但排除将NewsBlur包装在广告中的方式。——*此处翻译不顺，待进一步理解*

我们保留修改准则的权利。违反必究——*如有违反这些条例的精神，将会直接阻止而不会有事先警告*。


译：[奇葩史][000]