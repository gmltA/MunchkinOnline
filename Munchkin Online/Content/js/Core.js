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
 * Adds notification to the queue.
 * Use it in code to init notification.
 */
NotificationMgr.prototype.addNotification = function (message, type) {
    type = type || "";
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
    }
    else {
        $(controlButton).parent().find(".container").slideUp();
        $(controlButton).animate({ marginTop: "0px" }, "fast");
        $(controlButton).parent().find(".social").slideUp();
    }
    $(controlButton).toggleClass("down");
}

$(document).ready(function () {
    setTimeout(function () {
        notificationMgr.addNotification("Kyky1", "error");
        notificationMgr.addNotification("Kyky2");
    }, 1000);

    $("HEADER .control").live("click", function () {
        toggleFriendsBar(this);
    });
});