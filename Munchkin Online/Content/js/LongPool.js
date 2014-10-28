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
            error: this.connectionRefused
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
            error: this.sendRequest
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
            case 5: notificationMgr.addNotification(transport.Data); break;
            case 6: break;
            case 7: this.processFindGameResult(transport.Data); break;
            case 8: return;
        }
        this.sendRequest();
    },
    accept: function (transport)
    {
        var url = '/LongPoolHandler.ashx?cmd=MatchConfirmation';
        var self = this;
        $.ajax({
            type: "POST",
            success: function (response)
            {
                self.processResponse(response)
            },
            error: this.sendRequest,
            url: url
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
});

$(window).unload(function () {
    //Disconnect();
});
