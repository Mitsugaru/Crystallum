using UnityEngine;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class BattleManager : View, IBattleManager
{

    public int StartingSeed = 0;

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

    protected IBattleSystem CurrentBattle;

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
            //CurrentBattle = SingleTurnBattleSystem(StartingSeed++);
            SingleTurnBattleSystem system = gameObject.AddComponent<SingleTurnBattleSystem>();
            system.SetSeed(StartingSeed++);
            CurrentBattle = system;
        }
    }

    public void StepBattle()
    {
        if (CurrentBattle != null)
        {
            CurrentBattle.BattleStep();
        }
    }

    public bool InBattle()
    {
        return CurrentBattle != null;
    }
}
