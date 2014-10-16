var currCard;

function ActionProcess(card1, card2)
{

}

$(document).ready(
    function () {
        currCard = null;
        $(".card").click(
            function (a) {
                //if (!($(this).hasClass("door") || $(this).hasClass("treasure")))
                //{
                if ($(this).hasClass("my"))
                {
                    if (currCard == null)
                    {
                        $(this).toggleClass("selected");
                    } else
                    {
                        $(this).toggleClass("selected");
                        if (currCard == this) {
                            currCard = null;
                            return;
                        }
                        $(currCard).toggleClass("selected");
                    }
                    currCard = this;
                } else {
                        ActionProcess(currCard, this);
                    }
                //}

                
            });

        //todo: BURN IT WITH FIRE!
        /*$(".main-block").bind("contextmenu",
            function (a) {
                a.preventDefault();
                if (currCard == null) return;
                $(currCard).toggleClass("selected");
                currCard = null;
            });*/
    });