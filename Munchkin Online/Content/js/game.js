function updateStack(stack)
{
    var cardCount = $(stack).children().length;
    var stackWidth = $(stack).width();
    var cardWidth = $(stack).children().first().width();
    var margin = -1*(cardWidth - (stackWidth - cardWidth)/(cardCount - 1)) - 4;
    $(stack).children(".card").each(function(index, elem) {
        if (index != cardCount-1)
            $(elem).css("margin-right", margin);
    });
}
function cardHover(card, stack, cardIndex)
{
    var cardCount = $(stack).children().length;
    if (cardIndex == cardCount-1)
        return;
        
    var stackWidth = $(stack).width();
    var cardWidth = $(stack).children().first().width();
    var margin = -1*(cardWidth - (stackWidth - (cardWidth+3)*2)/(cardCount - 2)) - 4;
    $(stack).children(".card").each(function(index, elem) {
        if (index != cardCount-1 && index != cardIndex)
            $(elem).css("margin-right", margin);
        else if (index == cardIndex)
            $(elem).css("margin-right", 0);
    });
}

function setPopupCardBG(card)
{
    var bgImg = $(card).css("background-image");
    var bgPos = $(card).css("background-position");
    $("#popup-container").css("background-image", bgImg);
    $("#popup-container").css("background-position", bgPos);
}

$(document).ready(function () {
    $(".stack").each(function(index, elem){updateStack(elem)});
    $(".stack .card").on({
        mouseenter: function() {
            var currIndex = $(this).parent().children().index(this);
            cardHover(this, $(this).parent(), currIndex);
        },
        mouseleave: function() {
            updateStack($(this).parent());
        }
    });

    $(".card").on({
        dblclick: function ()
        {
            setPopupCardBG(this);
            showPopup();
        }
    });
    
    $(".card-mgr").click(function () { $(this).parent().toggleClass("pinned") });
    $("#blackout").click(function ()
    {
        closePopup();
    });
    
    $(".card").draggable({
        start: function ()
        {
            $(this).css("transition", "none");
        },
        stop: function ()
        {
            $(this).attr("style", "");
            if ($(this).parent().hasClass("stack"))
                updateStack($(this).parent());
        },
        revert: "invalid"
    });

    $(".card-mgr:not(.small) .card-slot").droppable({
        drop: function (event, ui)
        {
            var droppedCard = ui.draggable;
            $(this).append(droppedCard);

            $(droppedCard).attr("style", "");
            $(this).droppable({ disabled: true });
        }
    });
});