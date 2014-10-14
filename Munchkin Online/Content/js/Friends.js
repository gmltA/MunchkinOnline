function FriendMgr()
{
}

FriendMgr.prototype.searchForFriends = function (nickname)
{
    if (nickname == "") {
        $("#friend-list-container").html("");
        return;
    }
    $.ajax({
        type: "POST",
        url: '/Friends/FriendPlateList',
        data: 'name=' + nickname,
        cache: false,
        success: function (response)
        {
            $("#friend-list-container").html(response);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
        }
    });
}

FriendMgr.prototype.addFriend = function (id, name, callback)
{
    $.ajax({
        type: "PUT",
        url: '/Friends/ManageFriend',
        data: 'id=' + id,
        cache: false,
        success: function (response)
        {
            notificationMgr.addNotification("Friend " + name + " added");
            if (typeof (callback) == "function")
                callback();
            updateFriendList();
        },
        error: function (xhr, ajaxOptions, thrownError)
        {
            alert(thrownError);
        }
    });
}

FriendMgr.prototype.removeFriend = function (id, name) {
    $.ajax({
        type: "DELETE",
        url: '/Friends/ManageFriend',
        data: 'id=' + id,
        cache: false,
        success: function (response)
        {
            notificationMgr.addNotification("Friend " + name + " removed");
            updateFriendList();
        },
        error: function (xhr, ajaxOptions, thrownError)
        {
            alert(thrownError);
        }
    });
}

var friendMgr = new FriendMgr();

function updateFriendList()
{
    $.ajax({
        type: "POST",
        url: '/Friends/List',
        cache: false,
        success: function (response)
        {
            $("#friend-list").html(response);
        },
        error: function (xhr, ajaxOptions, thrownError)
        {
            alert(thrownError);
        }
    });
}

function showPopup()
{
    $.ajax({
        type: "POST",
        url: '/Friends/Search',
        cache: false,
        success: function (response)
        {
            $("#friend-search-popup").html(response);
            $('#blackout').stop().fadeIn("fast");
            $('#friend-search-popup').stop().fadeIn("fast");
            $(window).resize();
        },
        error: function (xhr, ajaxOptions, thrownError)
        {
            alert(thrownError);
        }
    });
}

function closePopup()
{
    $('#blackout').stop().fadeOut("fast");
    $('#friend-search-popup').stop().fadeOut("fast");
}

$(document).ready(function () {
    $(".friends .container P[data-id]").live("contextmenu", function (event) {
        event.preventDefault();
        $("div.custom-menu").remove();
        $("<div class='custom-menu'><div class='top-scroll'></div><div class='container'><p id='menu-remove-friend'>Remove friend</p><p>Open chat</p></div><div class='bottom-scroll'></div></div>")
            .appendTo("body")
            .css({ top: event.pageY + "px", left: event.pageX + "px" }).slideDown(100).data("id", $(this).data('id')).data("friend", $(this).text());
    });

    $("#friend-search").live("keyup", function ()
    {
        friendMgr.searchForFriends($(this).val());
        return true;
    });

    $(".friend-plate").live("click", function ()
    {
        if ($(this).hasClass("added")) {
            notificationMgr.addNotification("You've already this friend", "error");
            return false;
        }
        friendMgr.addFriend(this.id, $(this).find(".friend-data .nickname").text(), $(this).addClass("added"));
    });

    $(document).click(function (){
        $("div.custom-menu").remove();
    });

    $("#blackout").click(function (){
        closePopup();
    });

    $(".add-friend").click( function (){
        showPopup();
    });

    $("div.custom-menu #menu-remove-friend").live("click", function () {
        friendMgr.removeFriend($(this).parent().parent().data("id"), $(this).parent().parent().data("friend"));
        $("div.custom-menu").remove();
    });

    $(window).resize(function ()
    {
        $('#friend-search-popup').css({
            position: 'fixed',
            left: ($(window).width() - $('#friend-search-popup').outerWidth()) / 2,
            top: ($(window).height() - $('#friend-search-popup').outerHeight()) / 4
        });
    });
});