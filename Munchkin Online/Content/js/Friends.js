
function SearchForFriends(nickname) {
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
            alert("error");
        }
    });
}

function AddFriend(friendPlate)
{
    $.ajax({
        type: "PUT",
        url: '/Friends/ManageFriend',
        data: 'id=' + friendPlate.id,
        cache: false,
        success: function (response)
        {
            notificationMgr.addNotification("Friend " + $(friendPlate).find(".friend-data .nickname").text() + " added");
            $(friendPlate).addClass("added");
        },
        error: function (xhr, ajaxOptions, thrownError)
        {
            alert("error");
        }
    });
}

function RemoveFriend(id, name)
{
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

$(document).ready(function () {
    $("#friend-search").keyup(function (key) {
        SearchForFriends($(this).val());
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
            AddFriend(this);
        }
    });

    $(".friends .container P[data-id]").bind("contextmenu", function (event)
    {
        event.preventDefault();
        $("<div class='custom-menu'><p id='menu-remove-friend'>Remove friend</p><p>Open chat</p></div>")
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
            RemoveFriend($(this).parent().data("id"), $(this).parent().data("friend"));
            $("div.custom-menu").remove();
        }
    });
});