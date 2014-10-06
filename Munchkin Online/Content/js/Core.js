function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}


/**
 * Notification manager class.
 * Requires special markup (.notification div).
 */
function NotificationMgr() {
    this.notificationQueue = [];
    this.isNotificationActive = false;

    this.notificationWaitTime = 2000;
}

/**
 * Initial request.
 * Registers client server-side.
 */
NotificationMgr.prototype.init = function () {
    if (getCookie("__NOTIF_GUID_COOKIE") != undefined && getCookie("__NOTIF_TOKEN_COOKIE") != undefined)
        return;

    var url = '/NotificationsHandler.ashx';
    $.ajax({
        type: "POST",
        url: url,
        success: function (response) { },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

/**
 * Get notifications for current user.
 */
NotificationMgr.prototype.demandNotifications = function () {
    var url = '/NotificationsHandler.ashx?action=demandNotifications';
    $.ajax({
        type: "POST",
        url: url,
        dataType: 'json',
        success: function (response)
        {
            if (response == null)
                return;

            response.forEach(function (val, index, array) {
                notificationMgr.addNotification(val.Message, "error");
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("error");
        }
    });
}

/**
 * Adds notification to the queue.
 * Use it in code to init notification.
 */
NotificationMgr.prototype.addNotification = function (message, type) {
    type = type || "";
    if ($.isNumeric(type))
    {
        if (type == 1)
            type = "warning";
        else if (type == 2)
            type = "error";
        else
            type = "";
    }
    this.notificationQueue.push([message, type]);
    if (this.isNotificationActive == false)
        this.displayNextNotification();
}

/**
 * Displays notification.
 * Internal function. DO NOT USE IT DIRECTLY.
 */
NotificationMgr.prototype.showNotification = function (messageObj) {
    this.isNotificationActive = true;
    $(".notification > .body").addClass(messageObj[1]);
    $(".notification").addClass("anim");
    $(".notification").animate({ bottom: '0' }, 250, "swing", function () {
        $(".notification > .center").animate({ width: '190px' }, 500, "swing", function () {
            $(".notification > .scroll").hide();
            $(".notification").toggleClass("anim");
            $(".notification > .body > P").text(messageObj[0]);
        })
    });
    var mgr = this;
    setTimeout(function () {
        $(".notification > .body").toggleClass(messageObj[1]);
        $(".notification").toggleClass("anim");
        $(".notification > .body > P").text("");
        $(".notification > .scroll").show();
        $(".notification > .center").animate({ width: '0px' }, 500, "swing", function () {
            $(".notification").animate({ bottom: '-200px' }, 250, "swing", function () {
                mgr.isNotificationActive = false;
                mgr.displayNextNotification();
            })
        });
    }, this.notificationWaitTime);
}

/**
 * Pops next notification and displays it.
 * Internal function. DO NOT USE IT DIRECTLY.
 */
NotificationMgr.prototype.displayNextNotification = function () {
    if (this.notificationQueue.length == 0)
        return;

    this.showNotification(this.notificationQueue.pop());
}

var notificationMgr = new NotificationMgr();

function toggleFriendsBar(controlButton) {
    if ($(controlButton).hasClass("down")) {
        $(controlButton).parent().find(".container").slideDown();
        $(controlButton).animate({ marginTop: "-19px" }, "fast");
        $(controlButton).parent().find(".social").slideDown();
        $(controlButton).parent().find(".add-friend").slideDown();
    }
    else {
        $(controlButton).parent().find(".container").slideUp();
        $(controlButton).animate({ marginTop: "0px" }, "fast");
        $(controlButton).parent().find(".social").slideUp();
        $(controlButton).parent().find(".add-friend").slideUp();
    }
    $(controlButton).toggleClass("down");
}

$(document).ready(function () {
    notificationMgr.init();

    notificationMgr.demandNotifications();

    $("HEADER .control").live("click", function () {
        toggleFriendsBar(this);
    });
});