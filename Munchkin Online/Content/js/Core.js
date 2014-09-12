function showNotification(message) {
    $(".notification").addClass("anim");
    $(".notification").animate({ bottom: '0' }, 250, "swing", function () {
        $(".notification > .center").animate({ width: '190px' }, 500, "swing", function () {
            $(".notification > .scroll").hide();
            $(".notification").toggleClass("anim");
            $(".notification > P").text(message);
        })
    });
    setTimeout(function () {
        $(".notification").toggleClass("anim");
        $(".notification > P").text("");
        $(".notification > .scroll").show();
        $(".notification > .center").animate({ width: '0px' }, 500, "swing", function () {
            $(".notification").animate({ bottom: '-200px' }, 250, "swing")
        });
    }, 1000);
}

function toggleFriendsBar(controlButton) {
    if ($(controlButton).hasClass("down")) {
        $(controlButton).parent().find(".container").slideDown();
        $(controlButton).animate({ marginTop: "-19px" }, "fast");
    }
    else {
        $(controlButton).parent().find(".container").slideUp();
        $(controlButton).animate({ marginTop: "0px" }, "fast");
    }
    $(controlButton).toggleClass("down");
}

$(document).ready(function () {
    showNotification("Kyky!");

    $("HEADER .control").live("click", function () {
        toggleFriendsBar(this);
    });
});