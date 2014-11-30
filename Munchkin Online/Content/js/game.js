function Enum(enumValues)
{
    this.values = enumValues.concat();
}

Enum.prototype.getValue = function (index)
{
    return this.values[index];
}

Enum.prototype.getIndex = function (value)
{
    for (var prop in this.values) {
        if (this.values.hasOwnProperty(prop)) {
            if (this.values[prop] === value)
                return prop;
        }
    }
}

var slotClass = [];
slotClass[0] = "item";
slotClass[1] = "monster";
slotClass[2] = "spell";
slotClass[3] = "class";
slotClass[4] = "class-combo";
slotClass[5] = "race";
slotClass[6] = "race-combo";
slotClass[7] = "head";
slotClass[8] = "legs";
slotClass[9] = "body";
slotClass[10] = "weapon-1h";
slotClass[11] = "weapon-2h";

var slotClass = new Enum(slotClass);

function Game(battleState)
{
    this.filedCards = battleState.FieldCards;

    var rp = [];
    rp[0] = "left";
    rp[1] = "top";
    rp[2] = "right";

    this.relativePosition = new Enum(rp);

    this.me = new Player("bottom", battleState.Me, true);

    this.players = [];
    for (var i in battleState.Players) {
        var index = parseInt(i);
        this.players[index] = new Player(this.relativePosition.getValue(index), battleState.Players[index]);
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

Game.prototype.getPlayerByUserId = function (UserId)
{
    for (var i in this.players)
        if (this.players[i].srcData.UserId == UserId)
            return this.players[i];

    return;
}

Game.prototype.addCardsToField = function (cards)
{
    var self = this;
    cards.forEach(function (element, index, array)
    {
        self.addCardToField(new Card(element));
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

function Card(srcCardData)
{
    cardId = srcCardData.Id | 0;
    this.id = cardId;
    this.type = srcCardData.Type;
    this.class = srcCardData.Class;
}

Card.prototype.getHTML = function ()
{
    var type = this.type == 0 ? "door" : "treasure";
    return "<div class='card " + type + "' data-card-id='" + this.id + "'><figure class='back'></figure><figure class='face'></figure></div>";
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

Player.prototype.syncStack = function ()
{
    var cards = this.srcData.Board;
    if (this.isMe)
        cards = this.srcData.Board.Cards;

    if (!cards)
        return;

    cards = cards.slice();
    var self = this;
    for (var i in cards) {
        var card = cards[i];
        self.getCardMgr().find("[data-accept-class='" + slotClass.getValue(card.Class) + "']:not(:has(.card))").first().append(new Card(card).getHTML());
        delete cards[i];
    }

    if (this.isMe)
        this.getCardMgr().find(".card").each(function (index, element) { $(element).setDraggable() });
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
            self.addCardToStack(new Card(element));
        });
        this.getStack().children().each(function (index, element) { $(element).setDraggable() });
    }
    else {
        this.getCardsFromDeck($(".deck.treasure"), this.srcData.TreasuresCount);
        this.getCardsFromDeck($(".deck.door"), this.srcData.DoorsCount);
    }
    this.syncStack();
}

Player.prototype.getRandomCard = function ()
{
    var cardCount = this.getStack().children().length;
    return this.getStack().children().eq(Math.floor(Math.random() * cardCount));
}

/**
 * Force functions
 * Those functions are called by server as a reaction to battle messages
 */
Player.prototype.equipCard = function (cardClass, slot)
{
    var card = this.getRandomCard();
    card.moveTo(this.getCardMgr().find("[data-accept-class='" + slot + "']").first(), function ()
    {
        flipCard(card, cardClass);
    });
    return card;
}

Player.prototype.unequipCard = function (card)
{
    var self = this;
    card.moveTo(this.getStack(), function ()
    {
        updateStack(self.getStack());
        flipCard(card);
    });
    return card;
}

Player.prototype.makeMove = function (cardClass)
{
    var card = this.getRandomCard();
    card.moveTo($(".table"), function ()
    {
        flipCard(card, cardClass);
    });
    return card;
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

        $(card).toggleClass("flipped");
    }, 20);
}

function requestCard(deck, target, callback)
{
    var deckClass = 0;
    if ($(deck).hasClass("treasure"))
        deckClass = 1;

    var card = $(deck).append(new Card({Type : deckClass}).getHTML()).children(".card");

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
    console.log(battleMessage);

    var actionInvoker = game.getPlayerByUserId(battleMessage.UserId);

    var actionInfo = battleMessage.Action;
    var card = battleMessage.Card;
    if (actionInfo.SourceEntry == 0 && actionInfo.TargetEntry == 3) {
        var deckClass = actionInfo.SourceParam == 0 ? "door" : "treasure";
        actionInvoker.getCardsFromDeck($(".deck." + deckClass), 1);
    }
    else if (actionInfo.SourceEntry == 3 && actionInfo.TargetEntry == 1) {
        actionInvoker.equipCard(card.CSSClass, slotClass.getValue(battleMessage.Card.Class)).attr("data-card-id", battleMessage.Card.Id);
    }
    else if (actionInfo.SourceEntry == 1 && actionInfo.TargetEntry == 3) {
        var card = actionInvoker.getCardMgr().find("[data-card-id='" + battleMessage.Card.Id + "']");
        actionInvoker.unequipCard(card).attr("data-card-id", 0);
    }
    else if (actionInfo.SourceEntry == 3 && actionInfo.TargetEntry == 5) {
        actionInvoker.makeMove(card.CSSClass).attr("data-card-id", battleMessage.Card.Id);
    }
}

function onMatchStart(boardState)
{
    game = new Game(boardState);

    game.syncCards();
    updateStack(game.getMe().getStack());
}

function revertAction(actionInfo, source)
{
    if (!actionInfo.CardId)
        return;

    $("[data-card-id='" + actionInfo.CardId + "']").moveTo(source, function (card)
    {
        card.appendTo(source);
    });
}

function commitAction(actionInfo, source, callback)
{
    $.ajax({
        type: "POST",
        url: '/Game/ProcessAction',
        data: actionInfo,
        cache: false,
        success: function (response)
        {
            if (response == "ERROR") {
                revertAction(actionInfo, source);
            }
            else
                if (typeof callback == "function")
                    callback();
        }
    });
}

function dropAction(event, ui)
{
    var droppedCard = ui.draggable;
    var sourceObject = $(droppedCard).parent();
    if (sourceObject.get(0) == $(this).get(0))
        return;

    $(droppedCard).detach();
    var sourceEntry = 1; // deck
    if ($(sourceObject).hasClass("card-slot"))
        targetEntry = 1;
    else if ($(sourceObject).hasClass("stack"))
        sourceEntry = 3;

    var targetEntry = 5; // field
    if ($(this).hasClass("card-slot"))
        targetEntry = 1;
    else if ($(this).hasClass("stack"))
        targetEntry = 3;

    commitAction({ SourceEntry: sourceEntry, TargetEntry: targetEntry, CardId: $(droppedCard).data("card-id") }, sourceObject);
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
        var self = this;
        commitAction({ SourceEntry: 0, SourceParam: deckClass, TargetEntry: 3, CardId: 0 }, $(this), function ()
        {
            requestCard(self, $(".player-hand.bottom .stack"));
        })
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