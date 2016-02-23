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
        enemyParty.Clear();
    }

    public void ClearEnemies()
    {
        playerParty.Clear();
    }

    public void StartBattle()
    {
        if (CurrentBattle == null)
        {
            HealParties();
            //CurrentBattle = SingleTurnBattleSystem(StartingSeed++);
            SingleTurnBattleSystem system = gameObject.AddComponent<SingleTurnBattleSystem>();
            system.SetSeed(StartingSeed++);
            //system.Formula = new LinearBattleFormula(new MultiplierValues(minSlopeMulti, maxSlopeMulti), new MultiplierValues(minAddMulti, maxAddMulti), new MultiplierValues(minDefMulti, maxDefMulti));
            //system.Formula = new ExponentialBattleFormula();
            system.Formula = new TaperedSquaredBattleFormula();
            system.Formula.Multiplier = new MultiplierValues(3, 10);
            system.Formula.DefenseMultiplier = new MultiplierValues(minDefMulti, maxDefMulti);
            system.Formula.SetSeed(StartingSeed);
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
            entity.HP = entity.MaxHP;
        }
        foreach (Entity entity in playerParty)
        {
            entity.HP = entity.MaxHP;
        }
    }
}
