
# 登录 post /api/login

    *请求方法错误，应为post请求*
    {
        "code": -1,
        "authenticated": false,
        "errors": {
            "method": "Invalid method. Use POST. You used GET"
        },
        "result": "ok"
    }

    *未输入用户名*
    {
        "code": -1,
        "authenticated": false,
        "errors": {
            "username": [
            "Please enter a username."
            ]
        },
        "result": "ok"
    }


    *密码错误*
    {
        "code": -1,
        "authenticated": false,
        "errors": {
            "__all__": [
            "Whoopsy-daisy, wrong password. Try again."
            ]
        },
        "result": "ok"
    }

    *登录成功*
    {
        "authenticated": true,
        "code": 1,
        "user_id": *****,   一个长整形数字
        "errors": null,
        "result": "ok"
    }

***

# 注销 /api/logout

    *注销成功*
    {
        "code": 1,
        "authenticated": false,
        "result": "ok"
    }

***

# 获取用户订阅列表 /reader/feeds

 > 注：以下应只有一个folders节点，多个只是展示不同情况的返回值，另folders和flat_folders亦只会出现其中一个。  

    {

      "folders": [
        6461499,
        6464950
      ],

      "folders": [
        6464950,
        {
          "test": [
            6461499,
            {
              "second": [
                23551
              ]
            }
          ]
        }
      ],

      "flat_folders": {
        "test": [
          6461499
        ],
        " ": [
          6464950
        ],
        "test - second": [
          23551
        ]
      },

      "user_id": 430911,
      "social_profile": {
        "website": null,
        "following_user_ids": [],
        "following_count": 0,
        "shared_stories_count": 0,
        "private": null,
        "large_photo_url": "https://www.newsblur.com/media/img/reader/default_profile_photo.png",
        "id": "social:430911",
        "feed_address": "http://www.newsblur.com/social/rss/430911/huaxia283611",
        "user_id": 430911,
        "feed_link": "http://huaxia283611.newsblur.com/",
        "follower_user_ids": [],
        "location": null,
        "popular_publishers": null,
        "follower_count": 0,
        "username": "huaxia283611",
        "bio": null,
        "average_stories_per_month": 0,
        "feed_title": "huaxia283611's blurblog",
        "photo_service": null,
        "stories_last_month": 0,
        "photo_url": "https://www.newsblur.com/media/img/reader/default_profile_photo.png",
        "num_subscribers": 0,
        "protected": null
      },
      "user_profile": {
        "hide_getting_started": false,
        "preferences": {
          "hide_read_feeds": "0",
          "intro_page": "4",
          "story_titles_pane_size": "510"
        },
        "tutorial_finished": false,
        "has_setup_feeds": true,
        "is_premium": false,
        "dashboard_date": "2016-10-23 14:08:34.924868",
        "has_trained_intelligence": false,
        "has_found_friends": false
      },
      "starred_counts": [
        {
          "count": 0,
          "feed_address": "http://www.newsblur.com/reader/starred_rss/430911/eb2627817d5a/",
          "tag": null,
          "feed_id": null
        }
      ],
      "starred_count": 0,
      "is_staff": false,
      "result": "ok",
      "authenticated": true,
      "feeds": {
        "6461499": {
          "subs": 1,
            "favicon": "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAADf0lEQVR4nKXXTWhdRRQH8F+fMdQg\nIYRQpZQgWkooJUipIiGEIloEXXTVhbgOJbiSEsRVkSIuioiIFhFxIS4kFJESREIIIhJLkRBcSKlF\ng5QgXZQSQo2xz8WZ2zu5ufe9xPxheJcz52tmztfbZ+foxQRO4hiG0Y8W7mIFy/geC9jYhe6OOIB3\n8BfaO1y3cREH92K4hbO4kymexxsYxyEMpt8JnBOnzx1ZS/Se3Rrvx+WkZBOf48gOZY/iq4oj3yVn\nd4Qh/JwEr+O5nQpW8BJWMyeWxXN2xKO4pvR64H8aLzCMXzInFtHXxNxSXt0c9if6KM5gTDzNbnEA\nv2ZOfNTE+GpiuGnre32QCW/gCp7dpRMjIlULPSerDH34M20+X9n7wvY028RkB4PDaf+S8sSTmfw1\nceMP8Hra+LpG2aUaBwonqgF6CDNpr+C7mfZayuBu41QuuJSI4zUOFM7VrdmM74itUV+s6xnPmYw+\nUxAPZ4xbriXhIO41OLAhMqeFqw0885muXlEl21hHX0tUMfgW92scuIV3a+jwsAiwMTzTwPND9r2R\n7MAjGGuJxkIERhPexscNe0Nqojrhb3xWoS1m3ydaInDg9w4O3McUXsZPlb1NEfV1OFej90b2fbhH\nvCHxJt0wm9YTImBHRJRXY+cPTIvCVsXd7HuoR5xA5kgTRkXqDCclP+J8kv9GtOtVkWqLmd4q8s7Y\n25MEiVPV4UlRC16s2fsNr4mWfTkZrwvkHPmMsN5SvkldeT0u0qvOODyVHFwWJXsJp7s48HT2fQte\nEHm5YutbDiRap8lnRdnd+sWztMX80NvgwFImPykpWEuEVzLG812MrytrSIFBZeebsT04j1d0jBQb\nXybC1UzofVtrer6WNXfEY8m5thjfcsxUdDzAeLYxldGHxVz4IT7FBfFkdSU7x4Wk645yqDlVOcTZ\nqtCc8mpPdDHQDY8rb29SRH7R7tuiOO2vCo2KWt0W+Xx0j04Uo9iscswr1ukmoemM6bbtw8lusKA+\nfj7pJNRSBmQxdFy0+1mwJdp71fiCmquvotf2uX4Vb+n+T6clWvOVGuPzdQfZ16CoR9SBN/FQRv9X\nWetviJ7QmxwbTcYfq+j6B+8lffe6HGAbJkS+dipIndamSNs9oUfMcnOaC1PT6hhwND9BEwZF0RoV\n3XMgObgm8nxZTEhT4p/VtC7X/h+BElOkLeo1nwAAAABJRU5ErkJggg==\n",
          "favicon_url": "https://s3.amazonaws.com/icons.newsblur.com/6461499.png",
          "is_push": false,
          "feed_opens": 44,
          "id": 6461499,
          "s3_icon": true,
          "feed_link": "http://www.shisujie.com:80/",
          "updated_seconds_ago": 18638,
          "favicon_fetching": false,
          "ng": 0,
          "favicon_border": "000000",
          "last_story_date": "2016-10-26 15:00:00",
          "nt": 2,
          "not_yet_fetched": false,
          "updated": "5 hours",
          "average_stories_per_month": 7,
          "ps": 0,
          "feed_address": "http://www.shisujie.com/rss?containerid=31",
          "feed_title": "博客",
          "favicon_fade": "232323",
          "is_newsletter": false,
          "last_story_seconds_ago": 62587,
          "favicon_color": "000000",
          "stories_last_month": 14,
          "active": true,
          "fetched_once": true,
          "favicon_text_color": "white",
          "subscribed": true,
          "num_subscribers": 1,
          "s3_page": false,
          "min_to_decay": 384,
          "search_indexed": null
        },
        "6464950": {
          "subs": 1,
          "favicon_url": "/rss_feeds/icon/6464950",
          "exception_code": 301,
          "is_push": false,
          "feed_opens": 6,
          "id": 6464950,
          "has_exception": true,
          "s3_icon": false,
          "feed_link": "uuid:60a15e8a-3e6b-4a3d-9917-443e290be35a;id=154",
          "updated_seconds_ago": 15877,
          "favicon_fetching": false,
          "ng": 0,
          "favicon_border": null,
          "last_story_date": "2016-10-26 15:16:00",
          "nt": 10,
          "not_yet_fetched": false,
          "updated": "4 hours",
          "average_stories_per_month": 5,
          "ps": 0,
          "feed_address": "http://feed.cnblogs.com/blog/u/137549/rss",
          "feed_title": "博客园_奇葩史",
          "favicon_fade": null,
          "exception_type": "page",
          "is_newsletter": false,
          "last_story_seconds_ago": 61627,
          "favicon_color": null,
          "stories_last_month": 11,
          "active": true,
          "fetched_once": true,
          "favicon_text_color": null,
          "subscribed": true,
          "num_subscribers": 1,
          "s3_page": false,
          "min_to_decay": 456,
          "search_indexed": null,
          "disabled_page": true
        }
      },
      "social_services": {
        "facebook": {
          "syncing": false,
          "facebook_picture_url": null,
          "facebook_uid": null
        },
        "twitter": {
          "twitter_username": null,
          "syncing": false,
          "twitter_picture_url": null,
          "twitter_uid": null
        },
        "gravatar": {
          "gravatar_picture_url": "https://www.gravatar.com/avatar/3c2abfff69e97166655305f4126d06b3"
        },
        "appdotnet": {
          "syncing": false,
          "appdotnet_uid": null,
          "appdotnet_picture_url": null
        },
        "upload": {
          "upload_picture_url": null
        }
      },
      "categories": null,
      "social_feeds": []

    }


***

# 获取订阅源图标 /reader/favicons

{
  "6461499": "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAADf0lEQVR4nKXXTWhdRRQH8F+fMdQg\nIYRQpZQgWkooJUipIiGEIloEXXTVhbgOJbiSEsRVkSIuioiIFhFxIS4kFJESREIIIhJLkRBcSKlF\ng5QgXZQSQo2xz8WZ2zu5ufe9xPxheJcz52tmztfbZ+foxQRO4hiG0Y8W7mIFy/geC9jYhe6OOIB3\n8BfaO1y3cREH92K4hbO4kymexxsYxyEMpt8JnBOnzx1ZS/Se3Rrvx+WkZBOf48gOZY/iq4oj3yVn\nd4Qh/JwEr+O5nQpW8BJWMyeWxXN2xKO4pvR64H8aLzCMXzInFtHXxNxSXt0c9if6KM5gTDzNbnEA\nv2ZOfNTE+GpiuGnre32QCW/gCp7dpRMjIlULPSerDH34M20+X9n7wvY028RkB4PDaf+S8sSTmfw1\nceMP8Hra+LpG2aUaBwonqgF6CDNpr+C7mfZayuBu41QuuJSI4zUOFM7VrdmM74itUV+s6xnPmYw+\nUxAPZ4xbriXhIO41OLAhMqeFqw0885muXlEl21hHX0tUMfgW92scuIV3a+jwsAiwMTzTwPND9r2R\n7MAjGGuJxkIERhPexscNe0Nqojrhb3xWoS1m3ydaInDg9w4O3McUXsZPlb1NEfV1OFej90b2fbhH\nvCHxJt0wm9YTImBHRJRXY+cPTIvCVsXd7HuoR5xA5kgTRkXqDCclP+J8kv9GtOtVkWqLmd4q8s7Y\n25MEiVPV4UlRC16s2fsNr4mWfTkZrwvkHPmMsN5SvkldeT0u0qvOODyVHFwWJXsJp7s48HT2fQte\nEHm5YutbDiRap8lnRdnd+sWztMX80NvgwFImPykpWEuEVzLG812MrytrSIFBZeebsT04j1d0jBQb\nXybC1UzofVtrer6WNXfEY8m5thjfcsxUdDzAeLYxldGHxVz4IT7FBfFkdSU7x4Wk645yqDlVOcTZ\nqtCc8mpPdDHQDY8rb29SRH7R7tuiOO2vCo2KWt0W+Xx0j04Uo9iscswr1ukmoemM6bbtw8lusKA+\nfj7pJNRSBmQxdFy0+1mwJdp71fiCmquvotf2uX4Vb+n+T6clWvOVGuPzdQfZ16CoR9SBN/FQRv9X\nWetviJ7QmxwbTcYfq+j6B+8lffe6HGAbJkS+dipIndamSNs9oUfMcnOaC1PT6hhwND9BEwZF0RoV\n3XMgObgm8nxZTEhT4p/VtC7X/h+BElOkLeo1nwAAAABJRU5ErkJggg==\n",
  "6464950": null,
  "authenticated": true,
  "user_id": 430911,
  "result": "ok"
}

***