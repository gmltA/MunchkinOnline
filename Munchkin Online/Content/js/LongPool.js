var clientGuid  
      
$(document).ready(function() {  
    Connect();  
});  
      
$(window).unload(function() {  
    Disconnect();  
});  
      
function SendRequest() {  
    var url = '/LongPoolHandler.ashx?guid=' + clientGuid;
    $.ajax({  
        type: "POST",  
        url: url,  
        success: ProcessResponse,  
        error: SendRequest  
    });  
}  
      
function Connect() {  
    var url = '/LongPoolHandler.ashx?cmd=register';
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
    document.getElementsByTagName("body")[0].innerHTML +=  
       ' <strong>' + transport + '</strong> : <br/>';  
    SendRequest();  
}  
      
function OnConnected(transport) {  
    clientGuid = transport;  
    SendRequest();  
}  
      
function ConnectionRefused() {  
    $("body").html("не удалось подключиться к серверу.Попробуем через 3 секунды...");  
    setTimeout(Connect(), 3000);  
}  
