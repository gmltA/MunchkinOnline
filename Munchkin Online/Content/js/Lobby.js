function invitePlayerToLobby(playerId)
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
            $(this).append('<div class="menu" hidden><p>Lalka</p><p>Tralka</p><p>Lalka</p><p>Tralka</p></div>');
            $(this).toggleClass("menu-on");
            $(this).find(".menu").slideDown(200);
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