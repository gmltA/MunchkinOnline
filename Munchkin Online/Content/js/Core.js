var notificationQueue = [];
var isNotificationActive = false;

function showNotification(message)
{
    isNotificationActive = true;
    $(".notification").addClass("anim");
    $(".notification").animate({bottom : '0'}, 250, "swing", function(){
        $(".notification > .center").animate({width : '190px'}, 500, "swing", function(){
            $(".notification > .scroll").hide();
            $(".notification").toggleClass("anim");
            $(".notification > P").text(message);
        })
    });
    setTimeout(function()
    {
        $(".notification").toggleClass("anim");
        $(".notification > P").text("");
        $(".notification > .scroll").show();
        $(".notification > .center").animate({width : '0px'}, 500, "swing", function(){
            $(".notification").animate({bottom : '-200px'}, 250, "swing", function(){
                isNotificationActive = false;
                displayNextNotification();
            })
        });
	}, 1000);
}

function displayNextNotification()
{
    if (notificationQueue.length == 0)
        return;

    showNotification(notificationQueue.pop());
}

function addNotification(message)
{
    notificationQueue.push(message);
    if (isNotificationActive == false)
        displayNextNotification();
}

function toggleFriendsBar(controlButton)
{
    if ($(controlButton).hasClass("down"))
        {
            $(controlButton).parent().find(".container").slideDown();
            $(controlButton).animate({marginTop : "-19px"}, "fast");
        }
        else
        {
            $(controlButton).parent().find(".container").slideUp();
            $(controlButton).animate({marginTop : "0px"}, "fast");
        }
        $(controlButton).toggleClass("down");
}

$(document).ready(function(){
    addNotification("Kyky1");
    addNotification("Kyky2");
    
    $("HEADER .control").live("click", function(){
        toggleFriendsBar(this);
    });
});