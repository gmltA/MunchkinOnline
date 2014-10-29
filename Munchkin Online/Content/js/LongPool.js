function LongPool()
{
}

LongPool.prototype = {
    connect: function ()
    {
        var url = '/LongPoolHandler.ashx?cmd=' + currAction;
        var self = this;
        $.ajax({
            type: "POST",
            url: url,
            success: function (response)
            {
                self.processResponse(response)
            },
            error: function ()
            {
                self.connectionRefused();
            }
        });
    },
    disconnect: function ()
    {
        var url = '/LongPoolHandler.ashx?cmd=unregister';
        $.ajax({
            type: "POST",
            url: url
        });
    },
    sendRequest: function ()
    {
        var url = '/LongPoolHandler.ashx';
        var self = this;
        $.ajax({
            type: "POST",
            url: url,
            success: function (response)
            {
                self.processResponse(response)
            },
            error: function ()
            {
                self.sendRequest();
            }
        });
    },
    processFindGameResult: function (message)
    {
        if (notificationMgr.isStickyInfoActive)
            notificationMgr.updateStickyText(message);
        else
            notificationMgr.addNotification(message, 0, true);
    },
    processResponse: function (transport)
    {
        transport = JSON.parse(transport);
        switch (transport.Type) {
            case 3: $(".find").html("<a onclick=\"accept()\">Принять</a>"); break;
            case 4: renderInvite(transport.Data.Id); break;
            case 5: notificationMgr.addNotification(transport.Data); break;
            case 6: break;
            case 7: this.processFindGameResult(transport.Data); break;
            case 8: return;
            case 9: if (typeof lobbyUpdated != "undefined") lobbyUpdated(); break;
        }
        this.sendRequest();
    },
    accept: function (transport)
    {
        var url = '/LongPoolHandler.ashx?cmd=MatchConfirmation';
        var self = this;
        $.ajax({
            url: url,
            type: "POST",
            success: function (response)
            {
                self.processResponse(response)
            },
            error: function()
            {
                self.sendRequest();
            }
        });
    },
    connectionRefused: function ()
    {
        //$("body").html("не удалось подключиться к серверу.Попробуем через 3 секунды...");
        //setTimeout(Connect(), 3000);
    }
}

var longPool = new LongPool();

function findGame()
{
    currAction = 'FindMatch';
    longPool.connect();
}

$(document).ready(function ()
{
    //findGame();
    //TODO: WTF is this shit?
    if (typeof currAction === 'undefined') {
        currAction = '';
    }
    longPool.connect();
});

$(window).unload(function () {
    //Disconnect();
});
