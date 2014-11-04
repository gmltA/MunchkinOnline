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

$(document).ready(function ()
{
    jQuery.fn.extend({
        moveTo: function (target, callback)
        {
            if (target.length == 0) {
                console.error("No move target provided");
                return;
            }
            return this.each(function ()
            {
                var oldPosition = $(this).css("position");
                $(this).css({ "position": "fixed", "top": $(this).offset().top, "left": $(this).offset().left, "transition": "none" }).animate(
                    {
                        "top": target.offset().top, "left": target.offset().left
                    }, 600, "easeOutCubic", function ()
                    {
                        $(this).attr("style", "");
                        target.append($(this));
                        if (typeof callback == "function")
                            callback();
                    }
                );
            });
        }
    });

    $(".stack").each(function(index, elem){updateStack(elem)});
    $(".stack").on({
        mouseenter: function() {
            var currIndex = $(this).parent().children().index(this);
            cardHover(this, $(this).parent(), currIndex);
        },
        mouseleave: function() {
            updateStack($(this).parent());
        }
    }, ".card");

    $("BODY").on({
        dblclick: function ()
        {
            setPopupCardBG(this);
            showPopup();
        }
    }, ".card");
    
    $(".card-mgr").click(function () { $(this).parent().toggleClass("pinned") });
    $("#blackout").click(function ()
    {
        closePopup();
    });
    
    $(".card:not(#popup-container)").draggable({
        start: function ()
        {
            $(this).css("transition", "none");
        },
        stop: function ()
        {
            $(this).attr("style", "");
        },
        revert: "invalid",
        addClasses: false
    });

    $(".card-mgr:not(.small) .card-slot").droppable({
        drop: function (event, ui)
        {
            var droppedCard = ui.draggable;
            var stackParent = undefined;
            if ($(droppedCard).parent().hasClass("stack"))
                stackParent = $(droppedCard).parent();

            $(this).append(droppedCard);

            if (stackParent != undefined)
                updateStack(stackParent);

            $(droppedCard).attr("style", "");
        },
        accept: function(draggable)
        {
            if ($(this).children(".card").length != 0)
                return false;

            //todo: remove after WIP stage
            if (draggable.data("card-class") == undefined)
                return true;

            if ($(this).data("accept-class") != draggable.data("card-class"))
                return false;

            return true;
        },
        hoverClass: "draggable-hover",
        activeClass: "draggable-accept",
        addClasses: false
    });

    $(".stack, .table").droppable({
        drop: function (event, ui)
        {
            var droppedCard = ui.draggable;
            $(this).append(droppedCard);

            $(droppedCard).attr("style", "");
        },
        hoverClass: "draggable-hover",
        activeClass: "draggable-accept",
        addClasses: false
    });

    $(".deck").click(function ()
    {
        var deckClass = "door";
        if ($(this).hasClass("treasure"))
            deckClass = "treasure";
        $(this).append("<div class='card " + deckClass + "'></div>").children(".card").moveTo($(".player-hand.bottom .stack"), function ()
        {
            updateStack($(".player-hand.bottom .stack"))
        });
    });
});