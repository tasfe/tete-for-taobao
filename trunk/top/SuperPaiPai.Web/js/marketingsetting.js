function open_setting(type) {
    //判断一下用户店铺名是否太长，超过10个字就提示一下
    var shopName = $("#shopName").val();
    if (shopName.length > 10) {
        $.dialog.alert("您的店铺名字太长了，可能会浪费您的短信费用，请修改至不超过8个字", function () {
            window.location.href = "/setting/view.do";
        });
        return;
    }
    handle_setting("open", type);
}

function close_setting(type) {
    handle_setting("close", type);
}

function handle_setting(action_type, type) {
    var call_url = "huiyuansave.aspx?action_type=" + action_type + "&typ=" + type + "";
    var res_text = "open" == action_type ? "开启" : "关闭";
    var $d;
    $.ajax({
        url: call_url,
        data: { "typeId": type },
        beforeSend: function () {
            $d = $.dialog.tips('处理中...', 1000, 'loading.gif');
        },
        success: function (data) {
            var res = eval("(" + data + ")");
            if (res.success == true) {
                if ("open" == action_type) {
                    $("#setting_" + type + " img")[0].src = "images/on.gif";
                    $("#setting_" + type).attr("href", "javascript:close_setting(" + type + ")");
                } else {
                    $("#setting_" + type + " img")[0].src = "images/off.gif";
                    $("#setting_" + type).attr("href", "javascript:open_setting(" + type + ")");
                }
                $.dialog.tips(res_text + '成功', 1, 'tips.gif');
            } else {
                $d.close();
                if ("integral_not_enough" == res.errorType) {
                    $.dialog.alert(res_text + "失败，" + res.msg, function () {
                        //跳转到充值的地方
                        //window.location.href = 'msgbuy.aspx';
                    });
                } else {
                    if ("invalid_param" == res.errorType) {
                        $.dialog.alert(res_text + "失败，" + res.msg, function () {
                            window.location.href = '/marketing/marketingtemplate_view.do?type=' + type + "&rrid=" + new Date().getTime();
                        });
                    } else {
                        $.dialog.alert(res_text + "失败，原因：" + res.msg);
                    }
                }
            }
        }
    });
}