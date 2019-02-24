using UnityEngine;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class BattleManager : View, IBattleManager
{

    public int StartingSeed = 0;

    public int minSlopeMulti = 1;

    public int maxSlopeMulti = 5;

    public int minAddMulti = 1;

    public int maxAddMulti = 10;

    public int minDefMulti = 2;

    public int maxDefMulti = 5;

    private List<Entity> enemyParty = new List<Entity>();
    public ReadOnlyCollection<Entity> EnemyParty
    {
        get
        {
            return enemyParty.AsReadOnly();
        }
    }

    private List<Entity> playerParty = new List<Entity>();
    public ReadOnlyCollection<Entity> PlayerParty
    {
        get
        {
            return playerParty.AsReadOnly();
        }
    }

    protected AbstractBattleSystem CurrentBattle;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddEnemyMember(Entity entity)
    {
        enemyParty.Add(entity);
    }

    public void AddPartyMember(Entity entity)
    {
        playerParty.Add(entity);
    }

    public void ClearParty()
    {
        playerParty.Clear();
    }

    public void ClearEnemies()
    {
        enemyParty.Clear();
    }

    public void StartBattle()
    {
        if (CurrentBattle == null)
        {
            HealParties();
            //CurrentBattle = SingleTurnBattleSystem(StartingSeed++);
            SingleTurnBattleSystem system = gameObject.AddComponent<SingleTurnBattleSystem>();
            system.BattleFormulaQueue = new FormulaQueue(GenerateFormulas());
            system.BattleFormulaQueue.PopulateQueue();
            system.SetSeed(StartingSeed++);
            system.BattleFormulaQueue.SetSeed(StartingSeed++);
            system.VictoryCondition = new PartyWipeCondition(enemyParty);
            system.LossCondition = new PartyWipeCondition(playerParty);
            CurrentBattle = system;
        }
    }

    public void StepBattle()
    {
        if (CurrentBattle != null)
        {
            CurrentBattle.BattleStep();
        }
        if (CurrentBattle.ConditionResult())
        {
            Destroy(CurrentBattle);
            CurrentBattle = null;
        }
    }

    public bool InBattle()
    {
        return CurrentBattle != null;
    }

    protected void HealParties()
    {
        foreach (Entity entity in enemyParty)
        {
            entity.SetHP(entity.MaxHP);
        }
        foreach (Entity entity in playerParty)
        {
            entity.SetHP(entity.MaxHP);
        }
    }

    protected ICollection<IBattleFormula<int>> GenerateFormulas()
    {
        HashSet<IBattleFormula<int>> set = new HashSet<IBattleFormula<int>>();

        MultiplierValues defense = new MultiplierValues(minDefMulti, maxDefMulti);

        LogarithmicBattleFormula logarithmic = new LogarithmicBattleFormula();
        logarithmic.DefenseMultiplier = defense;
        logarithmic.Multiplier = new MultiplierValues(0, 2);
        logarithmic.SetSeed(StartingSeed);
        set.Add(logarithmic);

        LinearBattleFormula linear = new LinearBattleFormula();
        linear.AdditionMultiplier = new MultiplierValues(minAddMulti, maxAddMulti);
        linear.DefenseMultiplier = defense;
        linear.Multiplier = new MultiplierValues(minSlopeMulti, maxSlopeMulti);
        linear.SetSeed(StartingSeed);
        set.Add(linear);

        NormalExponentialBattleFormula normal = new NormalExponentialBattleFormula();
        normal.DefenseMultiplier = defense;
        normal.Multiplier = new MultiplierValues(1, 15);
        normal.SetSeed(StartingSeed);
        set.Add(normal);

        TaperedSquaredBattleFormula tapered = new TaperedSquaredBattleFormula();
        tapered.Multiplier = new MultiplierValues(3, 10);
        tapered.DefenseMultiplier = defense;
        tapered.SetSeed(StartingSeed);
        set.Add(tapered);

        ExponentialBattleFormula exponential = new ExponentialBattleFormula();
        exponential.Multiplier = new MultiplierValues(90, 100);
        exponential.DefenseMultiplier = defense;
        exponential.SetSeed(StartingSeed);
        set.Add(exponential);

        return set;
    }
}
