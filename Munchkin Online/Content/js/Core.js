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
    this.isStickyInfoActive = false;

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
        url: url
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
        }
    });
}

/**
 * Adds notification to the queue.
 * Use it in code to init notification.
 */
NotificationMgr.prototype.addNotification = function (message, type, isSticky) {
    type = type || "";
    isSticky = isSticky || false;

    if ($.isNumeric(type))
    {
        if (type == 1)
            type = "warning";
        else if (type == 2)
            type = "error";
        else
            type = "";
    }

    if (isSticky == true && this.isStickyInfoActive == false) {
        this.showNotification([message, type], isSticky);
        return;
    }

    this.notificationQueue.push([message, type]);
    if (this.isNotificationActive == false)
        this.displayNextNotification();
}

/**
 * Displays notification.
 * Internal function. DO NOT USE IT DIRECTLY.
 */
NotificationMgr.prototype.showNotification = function (messageObj, isSticky)
{
    var opObject;
    if (!isSticky) {
        this.isNotificationActive = true;
        opObject = $(".notification");
    }
    else {
        this.isStickyInfoActive = true;
        opObject = $(".sticky-info");
    }
    opObject.find(".body").addClass(messageObj[1]);
    opObject.addClass("anim");
    opObject.animate({ bottom: '0' }, 250, "swing", function () {
        opObject.find(".center").animate({ width: '190px' }, 500, "swing", function () {
            opObject.find(".scroll").hide();
            opObject.toggleClass("anim");
            opObject.find(".body > P").text(messageObj[0]);
        })
    });
    if (!isSticky) {
        var mgr = this;
        setTimeout(function ()
        {
            $(".notification > .body").toggleClass(messageObj[1]);
            $(".notification").toggleClass("anim");
            $(".notification > .body > P").text("");
            $(".notification > .scroll").show();
            $(".notification > .center").animate({ width: '0px' }, 500, "swing", function ()
            {
                $(".notification").animate({ bottom: '-200px' }, 250, "swing", function ()
                {
                    mgr.isNotificationActive = false;
                    mgr.displayNextNotification();
                })
            });
        }, this.notificationWaitTime);
    }
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

NotificationMgr.prototype.updateStickyText = function (message)
{
    if (this.isStickyInfoActive == false)
        return;

    $(".sticky-info > .body > P").text(message);
}

var notificationMgr = new NotificationMgr();

function toggleFriendsBar()
{
    var controlButton = $("HEADER .control");
    if ($(controlButton).hasClass("down")) {
        $(controlButton).parent().find(".container").slideDown();
        $(controlButton).animate({ marginTop: "-19px" }, "fast");
        $(controlButton).parent().find(".social").slideDown();
        $(controlButton).parent().find(".add-friend").slideDown();

        localStorage.setItem("authBarState", 1);
    }
    else {
        $(controlButton).parent().find(".container").slideUp();
        $(controlButton).animate({ marginTop: "0px" }, "fast");
        $(controlButton).parent().find(".social").slideUp();
        $(controlButton).parent().find(".add-friend").slideUp();

        localStorage.setItem("authBarState", 0);
    }
    $(controlButton).toggleClass("down");
}

function setAuthBarState()
{
    if (localStorage.getItem("authBarState") == true) {
        toggleFriendsBar();
    }
}

function showPopup()
{
    $('#blackout').stop().fadeIn("fast");
    $('#popup-container').stop().fadeIn("fast");
    $(window).resize();
}

function closePopup()
{
    $('#blackout').stop().fadeOut("fast");
    $('#popup-container').stop().fadeOut("fast");
}

$(document).ready(function ()
{
    $.ajaxSetup({
        error: function (xhr, ajaxOptions, thrownError)
        {
            notificationMgr.addNotification(thrownError, "error");
        }
    });

    setAuthBarState();

    notificationMgr.init();
    notificationMgr.demandNotifications();

    $("HEADER .control").click(function () {
        toggleFriendsBar();
    });

    $(window).resize(function ()
    {
        $('#popup-container').css({
            position: 'fixed',
            left: ($(window).width() - $('#popup-container').outerWidth()) / 2,
            top: ($(window).height() - $('#popup-container').outerHeight()) / 4
        });
    });
});