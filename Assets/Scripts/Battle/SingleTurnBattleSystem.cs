using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using System;

public class SingleTurnBattleSystem : View, IBattleSystem
{

    [Inject]
    public IBattleManager BattleManager { get; set; }

    protected int turn = 0;
    public int Turn
    {
        get
        {
            return turn;
        }
    }
    protected int actionCount = 0;

    protected int speedCount = 0;

    protected Dictionary<Entity, BattleStats> stats = new Dictionary<Entity, BattleStats>();

    protected HashFunction random;

    protected override void Start()
    {
        base.Start();

        foreach (Entity enemy in BattleManager.EnemyParty)
        {
            stats.Add(enemy, new BattleStats(enemy));
        }
        foreach (Entity member in BattleManager.PlayerParty)
        {
            stats.Add(member, new BattleStats(member));
        }
    }

    public void BattleStep()
    {
        Entity[] actors = GetNextActingEntities();
        foreach (Entity actor in actors)
        {
            BattleStats actorStat;
            if (stats.TryGetValue(actor, out actorStat))
            {
                //TODO select target that can be targeted, don't select downed opposing members
                Entity target = null;
                if (BattleManager.EnemyParty.Contains(actor))
                {
                    int index = random.Range(0, BattleManager.PlayerParty.Count, actionCount++);
                    target = BattleManager.PlayerParty[index];
                }
                else if (BattleManager.PlayerParty.Contains(actor))
                {
                    int index = random.Range(0, BattleManager.EnemyParty.Count, actionCount++);
                    target = BattleManager.EnemyParty[index];
                }
                if (target != null)
                {
                    BattleStats targetStats;
                    if (stats.TryGetValue(target, out targetStats))
                    {
                        //TODO Calculate damage
                        int dmg = 1;
                        targetStats.HP -= dmg;
                        Debug.Log(actor.Name + " damages " + target.Name + " for " + dmg);
                        actionCount++;
                    }
                }
            }
            speedCount = actorStat.Deft + 1;
            //TODO victory check
        }
    }

    public Entity[] GetNextActingEntities()
    {
        List<Entity> acting = new List<Entity>();
        int lowest = StatUtils.MAX_STAT;
        foreach (KeyValuePair<Entity, BattleStats> entry in stats)
        {
            if (entry.Value.Deft < lowest && entry.Value.Deft >= speedCount)
            {
                lowest = entry.Value.Deft;
                acting.Add(entry.Key);
            }
            else if (entry.Value.Deft == lowest)
            {
                acting.Add(entry.Key);
            }
        }

        if (acting.Count == 0)
        {
            //Reset and try again
            speedCount = 0;
            return GetNextActingEntities();
        }

        return acting.ToArray();
    }

    protected void DetermineNext() { }

    public void SetSeed(int seed)
    {
        if (random == null)
        {
            random = new XXHash(seed);
        }
    }
}
