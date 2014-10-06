function FriendMgr()
{
}

FriendMgr.prototype.searchForFriends = function (nickname) {
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
        },
        error: function (xhr, ajaxOptions, thrownError)
        {
            alert(thrownError);
        }
    });
}

var friendMgr = new FriendMgr();

$(document).ready(function () {
    $("#friend-search").keyup(function (key) {
        friendMgr.searchForFriends($(this).val());
        return true;
    });

    $(".friend-plate").live({
        click:
        function ()
        {
            if ($(this).hasClass("added")) {
                notificationMgr.addNotification("You've already this friend", "error");
                return false;
            }
            friendMgr.addFriend(this.id, $(this).find(".friend-data .nickname").text(), $(this).addClass("added"));
        }
    });

    $(".friends .container P[data-id]").bind("contextmenu", function (event)
    {
        $("div.custom-menu").remove();
        event.preventDefault();
        $("<div class='custom-menu'><div class='top-scroll'></div><div class='container'><p id='menu-remove-friend'>Remove friend</p><p>Open chat</p></div><div class='bottom-scroll'></div></div>")
            .appendTo("body")
            .css({ top: event.pageY + "px", left: event.pageX + "px" }).slideDown(100).data("id", $(this).data('id')).data("friend", $(this).text());
    });

    $(document).bind("click", function (event)
    {
        $("div.custom-menu").remove();
    });

    $("div.custom-menu #menu-remove-friend").live({
        click:
        function ()
        {
            friendMgr.removeFriend($(this).parent().data("id"), $(this).parent().data("friend"));
            $("div.custom-menu").remove();
        }
    });
});