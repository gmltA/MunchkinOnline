/**
 * Notification manager class.
 * Requires special markup (.notification div).
 */
function NotificationMgr() {
    this.notificationQueue = [];
    this.isNotificationActive = false;
}

/**
 * Adds notification to the queue.
 * Use it in code to init notification.
 */
NotificationMgr.prototype.addNotification = function (message) {
    this.notificationQueue.push(message);
    if (this.isNotificationActive == false)
        this.displayNextNotification();
}

/**
 * Displays notification.
 * Internal function. DO NOT USE IT DIRECTLY.
 */
NotificationMgr.prototype.showNotification = function (message) {
    this.isNotificationActive = true;
    $(".notification").addClass("anim");
    $(".notification").animate({ bottom: '0' }, 250, "swing", function () {
        $(".notification > .center").animate({ width: '190px' }, 500, "swing", function () {
            $(".notification > .scroll").hide();
            $(".notification").toggleClass("anim");
            $(".notification > P").text(message);
        })
    });
    var mgr = this;
    setTimeout(function () {
        $(".notification").toggleClass("anim");
        $(".notification > P").text("");
        $(".notification > .scroll").show();
        $(".notification > .center").animate({ width: '0px' }, 500, "swing", function () {
            $(".notification").animate({ bottom: '-200px' }, 250, "swing", function () {
                mgr.isNotificationActive = false;
                mgr.displayNextNotification();
            })
        });
    }, 1000);
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
        notificationMgr.addNotification("Kyky1");
        notificationMgr.addNotification("Kyky2");
    }, 1000);

    $("HEADER .control").live("click", function () {
        toggleFriendsBar(this);
    });
});