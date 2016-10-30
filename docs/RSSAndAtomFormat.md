# RSS格式

> 我也不知道是不是官方的官方文档：<http://www.rssboard.org/rss-specification>

示例格式：

    <?xml version="1.0" encoding="utf-8"?>
    <rss version="2.0">
        <channel>
            <title>订阅源标题，如：博客</title>
            <link>订阅源地址，如：http://www.shisujie.com:80/</link>
            <description>订阅源描述</description>
            <item>
                <title>订阅源文章标题，如：Orchard教程索引页</title>
                <link>订阅源文章地址，如：http://www.shisujie.com:80/blog/OrchardIndex</link>
                <description>订阅源文章内容/摘要</description>
                <pubDate>文章发布时间，如：Thu, 27 Oct 2016 14:00:00 GMT</pubDate>
                <guid isPermaLink="true">文章唯一标识，如：http://www.shisujie.com:80/blog/OrchardIndex</guid>
                <category>文章分类，如：Orchard</category>
                <category>文章分类，如：教程</category>
                <category>文章分类，如：索引目录</category>
            </item>
        </channel>
    </rss>

维基百科示例-<https://en.wikipedia.org/wiki/RSS>：

    <?xml version="1.0" encoding="UTF-8" ?>
    <rss version="2.0">
        <channel>
            <title>RSS Title</title>
            <description>This is an example of an RSS feed</description>
            <link>http://www.example.com/main.html</link>
            <lastBuildDate>Mon, 06 Sep 2010 00:01:00 +0000 </lastBuildDate>
            <pubDate>Sun, 06 Sep 2009 16:20:00 +0000</pubDate>
            <ttl>1800</ttl>

            <item>
                <title>Example entry</title>
                <description>Here is some text containing an interesting description.</description>
                <link>http://www.example.com/blog/post/1</link>
                <guid isPermaLink="true">7bd204c6-1655-4c27-aeee-53f933c5395f</guid>
                <pubDate>Sun, 06 Sep 2009 16:20:00 +0000</pubDate>
            </item>

        </channel>
    </rss>

# Atom格式

> 标准却烦人的文档：<http://www.ietf.org/rfc/rfc4287.txt> 或 <https://tools.ietf.org/html/rfc4287>

示例格式(来自维基百科-<https://en.wikipedia.org/wiki/Atom_(standard)>)：

    <?xml version="1.0" encoding="utf-8"?>
    <feed xmlns="http://www.w3.org/2005/Atom">
        <title>Example Feed</title>
        <subtitle>A subtitle.</subtitle>
        <link href="http://example.org/feed/" rel="self" />
        <link href="http://example.org/" />
        <id>urn:uuid:60a76c80-d399-11d9-b91C-0003939e0af6</id>
        <updated>2003-12-13T18:30:02Z</updated>
        <entry>
            <title>Atom-Powered Robots Run Amok</title>
            <link href="http://example.org/2003/12/13/atom03" />
            <link rel="alternate" type="text/html" href="http://example.org/2003/12/13/atom03.html"/>
            <link rel="edit" href="http://example.org/2003/12/13/atom03/edit"/>
            <id>urn:uuid:1225c695-cfb8-4ebb-aaaa-80da344efa6a</id>
            <updated>2003-12-13T18:30:02Z</updated>
            <summary>Some text.</summary>
            <content type="xhtml">
                <div xmlns="http://www.w3.org/1999/xhtml">
                    <p>This is the entry content.</p>
                </div>
            </content>
            <author>
                <name>John Doe</name>
                <email>johndoe@example.com</email>
            </author>
        </entry>
    </feed>

如果你有多个Atom文档，使用下面的方式整合到一块：

    <link href="atom.xml" type="application/atom+xml" rel="alternate" title="Sitewide ATOM Feed" />