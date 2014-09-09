using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Models
{
    public class Mechanic
    {
        public Guid Id { get; set; }

        public Actions Action { get; set; }

        public int Value { get; set; }

        public Targets Target { get; set; }


    }

    enum Actions
    {
        AddLevel,
        RemoveLevel,
        GetShmotkaFromHand,
        GetShmotkaFromBoard,
        GetFromSbros,
        BonusToSide,
        AddToSmyvka,
        Autosmyvka,
        RollDiceAgain,
        Hireling,
        KillHireing,
        TransferralBattle,
        EndBattle,
        InstantSmyvka,
        TakeTreasure
    }

    enum Targets
    {
        All,
        You,
        NotYou,
        Males,
        Females,
        Hufflings,
        Cheat,
        Thiefs,
        Wisards,
        NotWizards,
        Warriors,
        Clerics,
        Dwarfs
    }
}