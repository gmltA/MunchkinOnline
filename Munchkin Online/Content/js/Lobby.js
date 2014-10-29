﻿function invitePlayerToLobby(playerId)
{
    $.ajax({
        type: "POST",
        url: '/Lobby/InvitePlayer',
        data: "playerGuid=" + playerId
    });
}

function closeMenuCallback(object)
{
    $(object).toggleClass("menu-on");
    $(object).find(".menu").remove();
    $('#blackout').stop().fadeOut("fast");
}

function loadFriendToInviteList(container)
{
    $.ajax({
        type: "POST",
        url: '/Lobby/FriendToInviteList',
        cache: false,
        success: function (response)
        {
            console.log(response);
            $(container).append(response);
            $(container).toggleClass("menu-on");
            $(container).find(".menu").slideDown(200);
        }
    });
}

$(document).ready(function ()
{
    $(".lobby-players .player").click(function ()
    {
        if ($(this).hasClass("creator"))
            return;

        if ($(this).hasClass("menu-on"))
        {
            var obj = this;
            $(this).find(".menu").slideUp(200, function () { closeMenuCallback(obj) });
        }
        else
        {
            loadFriendToInviteList(this);
            $('#blackout').stop().fadeIn("fast");
        }
    });

    $(".lobby-players .player.menu-on").live("mouseleave", function ()
    {
        var obj = this;
        $(this).find(".menu").slideUp(200, function() { closeMenuCallback(obj) });
    });

    $(".lobby-players .player .menu P").live("click", function ()
    {
        invitePlayerToLobby($(this).data("id"));
    });
});