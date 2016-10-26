
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
        "user_id": 430911,
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