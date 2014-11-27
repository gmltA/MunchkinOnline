function Player(position)
{
    this.fieldPosition = position;
}

Player.prototype.getCardMgr = function ()
{
    return $("." + this.fieldPosition + " .card-mgr");
}

Player.prototype.getStack = function ()
{
    return $(".player-hand." + this.fieldPosition + " .stack");
}

Player.prototype.getCards = function (deck, counter, callback)
{
    if (counter > 0)
    {
        var self = this;
        requestCard(deck, this.getStack(), function ()
        {
            counter--;
            self.getCards(deck, counter, callback);
        });
    }
    else
    {
        if (typeof callback == "function")
            callback();
    }
}

Player.prototype.getRandomCard = function ()
{
    var cardCount = this.getStack().children().length;
    return this.getStack().children().eq(Math.floor(Math.random() * cardCount));
}

Player.prototype.equipCard = function (cardClass, slot)
{
    var card = this.getRandomCard();
    card.moveTo(this.getCardMgr().find("." + slot), function ()
    {
        flipCard(card, cardClass);
    });
}

Player.prototype.makeMove = function (cardClass)
{
    var card = this.getRandomCard();
    card.moveTo($(".table"), function ()
    {
        flipCard(card, cardClass);
    });
};

function updateStack(stack)
{
    var cardCount = $(stack).children().length;
    var stackWidth = $(stack).width();
    var cardWidth = $(stack).children().first().width();
    var margin = -1*(cardWidth - (stackWidth - cardWidth)/(cardCount - 1));
    $(stack).children(".card").each(function(index, elem) {
        if (index != cardCount-1)
            $(elem).css("margin-right", margin);
    });
}

function cardHover(card, stack, cardIndex)
{
    var cardCount = $(stack).children().length;
    if (cardIndex == cardCount - 1 || cardCount < 4)
        return;
        
    var stackWidth = $(stack).width();
    var cardWidth = $(stack).children().first().width();
    var margin = -1*(cardWidth - (stackWidth - cardWidth*2)/(cardCount - 2));
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

function flipCard(card, cardClass)
{
    setTimeout(function ()
    {
        if (typeof cardClass != "undefined")
            $(card).addClass(cardClass);

        $(card).addClass("flipped");
    }, 20);
}

function requestCard(deck, target, callback)
{
    var deckClass = "door";
    if ($(deck).hasClass("treasure"))
        deckClass = "treasure";

    var card = $(deck).append("<div class='card " + deckClass + "'><figure class='back'></figure><figure class='face'></figure></div>").children(".card");

    card.moveTo(target, function ()
    {
        if ($(target).hasClass("stack"))
            updateStack(target);

        if (typeof callback == "function")
            callback(card);
    }).setDraggable();
}

function battleMessageHandler(battleMessage)
{
}

function onMatchStart(boardState)
{
    var bottomPlayer = new Player("bottom");
    bottomPlayer.getCards($(".deck.door"), boardState.Me.Hand.length, function () { });

    var topPlayer = new Player("top");
    topPlayer.getCards($(".deck.treasure"), boardState.Players[0].TreasuresCount, function () { });
    topPlayer.getCards($(".deck.door"), boardState.Players[0].DoorsCount, function () { });
    var leftPlayer = new Player("left");
    leftPlayer.getCards($(".deck.treasure"), boardState.Players[1].TreasuresCount, function () { });
    leftPlayer.getCards($(".deck.door"), boardState.Players[1].DoorsCount, function () { });
    var rightPlayer = new Player("right");
    rightPlayer.getCards($(".deck.treasure"), boardState.Players[2].TreasuresCount, function () { });
    rightPlayer.getCards($(".deck.door"), boardState.Players[2].DoorsCount, function () { });
}

function dropAction(event, ui)
{
    var droppedCard = ui.draggable;
    var stackParent = undefined;
    if ($(droppedCard).parent().hasClass("stack"))
        stackParent = $(droppedCard).parent();

    $(droppedCard).attr("style", "");
    $(this).append(droppedCard);

    if (stackParent != undefined)
        updateStack(stackParent);

    if ($(this).hasClass("stack"))
        updateStack($(this));
}

var tutorialStep = 0;
var maxTutorialSteps = 4;

function loadTutorialState()
{
    tutorialStep = localStorage.getItem("tutorialStep");
    if (tutorialStep < maxTutorialSteps - 1)
        tutorialProcessStep(tutorialStep++);
}

function tutorialProcessStep(step)
{
    localStorage.setItem("tutorialStep", step);

    var hintClass;
    if (step == 0)
        hintClass = "drag";
    else if (step == 1)
        hintClass = "deck_click";
    else
        hintClass = "equip";

    if (tutorialStep >= maxTutorialSteps) {
        $("#hint").fadeOut("fast");
        $('#blackout').fadeOut("fast");
    }

    if (hintClass == "equip" && !$(".player-hand.bottom .card-mgr").parent().hasClass("pinned"))
        $(".player-hand.bottom .card-mgr").parent().addClass("pinned");

    $("#hint").attr("class", "").addClass(hintClass);
    if ($("#hint").css("display") != "block") {
        $("#hint").fadeIn("fast");
        $('#blackout').stop().fadeIn("fast");
    }

}

$(document).ready(function ()
{
    $(".stack").each(function (index, elem) { updateStack(elem) });
    $(".stack").on({
        mouseenter: function() {
            cardHover(this, $(this).parent(), $(this).parent().children().index(this));
        },
        mouseleave: function() {
            updateStack($(this).parent());
        }
    }, ".card");

    $("BODY").on({
        dblclick: function ()
        {
            $("#popup-container").attr("class", "").addClass($(this).attr("class"));
            setPopupCardBG(this);
            showPopup();
        }
    }, ".card");
    
    $(".card-mgr").click(function () { $(this).parent().toggleClass("pinned") });
    $("#blackout").click(closePopup);

    $("#hint").click(function ()
    {
        tutorialProcessStep(tutorialStep++);
    });
    
    $(".card:not(#popup-container)").setDraggable();

    $(".card-mgr:not(.small) .card-slot").droppable({
        drop: dropAction,
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
        drop: dropAction,
        hoverClass: "draggable-hover",
        activeClass: "draggable-accept",
        addClasses: false
    });

    $(".deck").click(function () { requestCard(this, $(".player-hand.bottom .stack")); });

    loadTutorialState();
});

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
            var offset = $(this).offset();
            $(this).appendTo($("body"));
            $(this).css({ "position": "fixed", "top": offset.top, "left": offset.left, "transition": "none" }).animate(
                {
                    "top": target.moveTargetOffset($(this)).top, "left": target.moveTargetOffset($(this)).left
                }, 600, "easeOutCubic", function ()
                {
                    $(this).attr("style", "");
                    target.append($(this));

                    callback($(this));
                }
            );
        });
    },
    moveTargetOffset: function(movable)
    {
            if ($(this).css("text-align") == "center")
                return { top: $(this).offset().top, left: $(this).offset().left + $(this).width() / 2 - $(movable).width() / 2}
            else
                return $(this).offset();
    },
    setDraggable: function()
    {
        return this.each(function ()
        {
            $(this).draggable({
                start: function ()
                {
                    $(this).css("transition", "none");
                },
                stop: function ()
                {
                    $(this).attr("style", "");
                },
                revert: "invalid",
                containment: "window",
                addClasses: false
            });
        });
    }
});