   
$(document).ready(function() {  
    if (typeof currAction === 'undefined') {
        currAction = '';
    }
    Connect();
});  
      
$(window).unload(function() {  
    //Disconnect();  
});  
      
function SendRequest() {  
    var url = '/LongPoolHandler.ashx';
    $.ajax({  
        type: "POST",  
        url: url,  
        success: ProcessResponse,
        error: SendRequest  
    });  
}  
      
function Connect() {  
    var url = '/LongPoolHandler.ashx?cmd='+currAction;
    $.ajax({  
        type: "POST",  
        url: url,  
        success: OnConnected,  
        error: ConnectionRefused  
    });  
}  
      
 
function Disconnect() {  
    var url = '/LongPoolHandler.ashx?cmd=unregister';
    $.ajax({  
        type: "POST",  
        url: url  
    });  
}  
      
 
function ProcessResponse(transport) {  
    transport = JSON.parse(transport);
    switch (transport.Type)
    {
        case 3: $(".find").html("<a>Принять</a>"); break; //Матч найден, требуется подтверждение
        case 5: notificationMgr.addNotification(transport.Data); break;
        case 6: break; //Не все подтвердили
        case 7: $("#QueueStatus").html(transport.Data); break;
    } 
    SendRequest();  
}  
      
function OnConnected(transport) {  
    //clientGuid = transport;  
    SendRequest();  
}  
      
function ConnectionRefused() {  
    //$("body").html("не удалось подключиться к серверу.Попробуем через 3 секунды...");  
    //setTimeout(Connect(), 3000);  
}  
