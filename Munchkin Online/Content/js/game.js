function RelativePosition()
{
    this.relativePosition = [];
    this.relativePosition["left"] = 0;
    this.relativePosition["top"] = 1;
    this.relativePosition["right"] = 2;
}

RelativePosition.prototype.getIndex = function (position)
{
    return this.relativePosition[position];
}

RelativePosition.prototype.getString = function (index)
{
    for (var prop in this.relativePosition) {
        if (this.relativePosition.hasOwnProperty(prop)) {
            if (this.relativePosition[prop] === index)
                return prop;
        }
    }
}

function Game(battleState)
{
    this.filedCards = battleState.FieldCards;

    this.relativePosition = new RelativePosition();
    this.me = new Player("bottom", battleState.Me, true);

    this.players = [];
    for (var i in battleState.Players) {
        var index = parseInt(i);
        this.players[index] = new Player(this.relativePosition.getString(index), battleState.Players[index]);
    }
}

Game.prototype.getMe = function ()
{
    return this.me;
}

Game.prototype.getPlayer = function (id)
{
    if (typeof id == "number")
        return this.players[id];
    else if (typeof id == "string")
        return this.players[this.relativePosition.getIndex(id)];

    return;
}

Game.prototype.addCardsToField = function (cards)
{
    var self = this;
    cards.forEach(function (element, index, array)
    {
        self.addCardToField(new Card(element.Type == 0 ? "door" : "treasure"));
    });
}

Game.prototype.addCardToField = function (card)
{
    $(".table").append(card.getHTML());
}

// Match front-end cards with back-end data
Game.prototype.syncCards = function ()
{
    this.addCardsToField(this.filedCards);

    for (var i in this.players)
        this.players[i].syncCardsWithSrc();

    this.me.syncCardsWithSrc();
}

var game;

function Card(cardType, cardId)
{
    cardId = cardId | 0;
    this.id = cardId;
    this.type = cardType;
}

Card.prototype.getHTML = function ()
{
    return "<div class='card " + this.type + "' data-card-id='" + this.id + "'><figure class='back'></figure><figure class='face'></figure></div>";
}

function Player(position, srcPlayerData, isMe)
{
    this.isMe = isMe | false;
    this.srcData = srcPlayerData;
    this.fieldPosition = position;
}

Player.prototype.getSrcData = function ()
{
    return this.srcData;
}

Player.prototype.getCardMgr = function ()
{
    return $("." + this.fieldPosition + " .card-mgr");
}

Player.prototype.getStack = function ()
{
    return $(".player-hand." + this.fieldPosition + " .stack");
}

Player.prototype.getCardsFromDeck = function (deck, counter, callback)
{
    if (counter > 0) {
        var self = this;
        requestCard(deck, this.getStack(), function ()
        {
            counter--;
            self.getCardsFromDeck(deck, counter, callback);
        });
    }
    else {
        if (typeof callback == "function")
            callback();
    }
}

//todo: impelement isAnimated parameter
Player.prototype.syncCardsWithSrc = function ()
{
    if (this.isMe) {
        var self = this;
        this.srcData.Hand.Cards.forEach(function (element, index, array)
        {
            self.addCardToStack(new Card(element.Type == 0 ? "door" : "treasure", element.Id));
        });
        this.getStack().children().each(function (index, element) { $(element).setDraggable() });
    }
    else {
        this.getCardsFromDeck($(".deck.treasure"), this.srcData.TreasuresCount);
        this.getCardsFromDeck($(".deck.door"), this.srcData.DoorsCount);
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

Player.prototype.addCardToStack = function (card)
{
    this.getStack().append(card.getHTML());
}

function updateStack(stack)
{
    var cardCount = $(stack).children().length;
    var stackWidth = $(stack).width();
    var cardWidth = $(stack).children().first().width();
    var margin = -1 * (cardWidth - (stackWidth - cardWidth) / (cardCount - 1));
    $(stack).children(".card").each(function (index, elem)
    {
        if (index != cardCount - 1)
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
    var margin = -1 * (cardWidth - (stackWidth - cardWidth * 2) / (cardCount - 2));
    $(stack).children(".card").each(function (index, elem)
    {
        if (index != cardCount - 1 && index != cardIndex)
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

    var card = $(deck).append(new Card(deckClass).getHTML()).children(".card");

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
    console.log(battleMessage.Action);
}

function onMatchStart(boardState)
{
    game = new Game(boardState);

    game.syncCards();
    updateStack(game.getMe().getStack());
}

function commitAction(actionInfo)
{
    $.ajax({
        type: "POST",
        url: '/Game/ProcessAction',
        data: actionInfo,
        cache: false,
        success: function (response)
        {

        }
    });
}

function dropAction(event, ui)
{
    var droppedCard = ui.draggable;
    var sourceObject = $(droppedCard).parent();

    var sourceEntry = 1;
    if ($(sourceObject).hasClass("stack"))
        sourceEntry = 3;

    commitAction({ SourceEntry: sourceEntry, TargetEntry: 5, CardId: $(droppedCard).data("card-id") });
    $(droppedCard).attr("style", "");
    $(this).append(droppedCard);

    if ($(sourceObject).hasClass("stack"))
        updateStack(sourceObject);

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
        mouseenter: function ()
        {
            cardHover(this, $(this).parent(), $(this).parent().children().index(this));
        },
        mouseleave: function ()
        {
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
        accept: function (draggable)
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

    //todo: consider commitAction result
    $(".deck").click(function ()
    {
        var deckClass = 0;
        if ($(this).hasClass("treasure"))
            deckClass = 1;

        //todo: return card object and assign card-id to newly created object
        commitAction({ SourceEntry: 0, SourceParam: deckClass, TargetEntry: 3, CardId: 0 });
        requestCard(this, $(".player-hand.bottom .stack"));
    });

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
    moveTargetOffset: function (movable)
    {
        if ($(this).css("text-align") == "center")
            return { top: $(this).offset().top, left: $(this).offset().left + $(this).width() / 2 - $(movable).width() / 2 }
        else
            return $(this).offset();
    },
    setDraggable: function ()
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