function invitePlayerToLobby(playerId)
{
    $.ajax({
        type: "POST",
        url: '/Lobby/InvitePlayer',
        data: "playerGuid=" + playerId
    });
}

function kickPlayerFromLobby(playerId)
{
    $.ajax({
        type: "POST",
        url: '/Lobby/KickPlayer',
        data: "playerGuid=" + playerId
    });
}

function closeMenuCallback(object)
{
    $(object).toggleClass("menu-on");
    $(object).find(".menu").remove();
    $('#blackout').stop().fadeOut("fast");
}

function loadSlotContextMenu(container)
{
    var emptySlot = 1;
    if ($(container).data("id") != undefined) {
        emptySlot = 0;
    }

    $.ajax({
        type: "POST",
        url: '/Lobby/SlotContextMenu',
        data: 'emptySlot=' + emptySlot,
        cache: false,
        success: function (response)
        {
            $(container).append(response);
            $(container).toggleClass("menu-on");
            $(container).find(".menu").slideDown(200);
        }
    });
}

function lobbyUpdated()
{
    window.location.reload();
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
            loadSlotContextMenu(this);
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
        if ($(this).hasClass("button-kick"))
            kickPlayerFromLobby($(this).parent().parent().data("id"));
        else
            invitePlayerToLobby($(this).data("id"));
    });
});