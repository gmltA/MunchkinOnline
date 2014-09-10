﻿using System;
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

        public Conditions Condition { get; set; }
    }



    public enum Actions
    {
        AddLevel,
        RemoveLevel,

        GetItemFromHand,
        GetItemFromBoard,
        GetItemFromSbros,
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

        ChangeSex,
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

    public enum Targets
    {
        All,
        You,
        NotYou,
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

    public enum Conditions
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