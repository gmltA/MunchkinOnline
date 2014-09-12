using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Models
{
    public class Mechanic
    {
        public Guid Id { get; set; }

        public Action Action { get; set; }

        public int Value { get; set; }

        public Target Target { get; set; }

        public Condition Condition { get; set; }
    }



    public enum Action
    {
        AddLevel,
        RemoveLevel,

        GetItemFromHand,
        GetItemFromBoard,
        GetItemFromTrash,
        LoseItemsFromHand,
        LoseItemsFromTable,
        LoseAllItems,
        GiveItemsToHighestLevelPlayers,
        LoseSmallItem,
        LoseBigItem,
        LoseFootgear,
        LoseBestItem,
        LoseArmor,
        LoseHeadgear,


        BonusToSide,

        MinusFromEscaping,
        AddToEscaping,
        AutoEscape,
        InstantEscape,
        NoEscape,

        RollDiceAgain,

        Hireling,
        KillHireing,

        NoBattleWithLevelsLess,
        NoBattle,
        BattleOnlyWithHufflings,
        TransferralBattle,
        EndBattle,
        BonusesOnly,
        LevelOnly,
        AddToMonster,
        MinusFromMonster,
        AdditionalLevel,
        ExchangeMonster,
        DoubleMonster,
        GiveTreasuresToCleric,
        AloneInTheBattle,
        BonusTreasure,

        TakeTreasure,
        
        Cheat,
        ProtectionFromCurse,
       
        Supermunchkin,
        Halfblooded,

        ChangeGender,
        ChangeRace,
        ChangeClass,
        LoseClass,
        LoseRace,

        BonusAgainstClerics,
        BonusAgainstDwarfs,
        BonusAgainstElfs,
        BonusAgainstWizards,
        BonusAgainstHufflings,

        Killing
    }

    public enum Target
    {
        All,
        Self,
        NotSelf,
        Males,
        Females,
        Men,
        Hufflings,
        NotHufflings,
        Thiefs,
        NotThiefs,
        Wisards,
        NotWizards,
        Warriors,
        NotWarriors,
        Clerics,
        NotClerics,
        Dwarfs,
        NotDwarfs
    }

    public enum Condition
    {
        Instant,
        AfterBattle,
        InBattle,
        AfterSuccessfullEscape,
        IfHirelingIsHere,
        AfterDiceRoll,
        IfElfsInBattle,
        WithoutSupport,
        WhenEscaped,

    }
}