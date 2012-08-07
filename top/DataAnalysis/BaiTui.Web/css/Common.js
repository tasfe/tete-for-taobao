function GetUserInfo() {
    try {
        $.ajax({
            url: 'BAjaxRequest.aspx',
            type: 'post',
            async: true,
            data: {
                "R": Math.random(),
                "OP": 'GetUserInfo'
            },
            success: function (data) {
                $("#userinfo").html(data);
            }
        })
    } catch (e) { }
}
$(function () {
    GetUserInfo();
});
function gopage(no) {
    GetContent(parseInt(no));
    SearchItemPager(parseInt(no));
}
function getQuery(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
function pre(id) {
    window.showModalDialog("ShowDetailes.aspx?id=" + id);
}
function exit() {
    try {
        $.ajax({
            url: 'BAjaxRequest.aspx',
            type: 'post',
            async: true,
            data: {
                "R": Math.random(),
                "OP": 'Exit'
            },
            success: function (data) {
                eval(data.toString());
            }
        })
    } catch (e) { }
}
function GetCategory() {
    try {
        $.ajax({
            url: 'BAjaxRequest.aspx',
            type: 'post',
            async: true,
            data: {
                "R": Math.random(),
                "OP": "GetSellercatsList"
            },
            success: function (data) {
                $("#Category").html(data);
            }
        })
    } catch (e) { }
}
function GetMoreFuwu() {
    try {
        $.ajax({
            url: 'BAjaxRequest.aspx',
            type: 'post',
            async: true,
            data: {
                "R": Math.random(),
                "OP": 'GetMoreFuwu'
            },
            success: function (data) {
                $("#morefuwu").html(data.toString());
            }
        })
    } catch (e) { }
}