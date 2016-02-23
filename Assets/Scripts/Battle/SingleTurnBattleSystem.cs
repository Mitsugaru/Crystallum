using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

public class SingleTurnBattleSystem : AbstractBattleSystem
{

    /// <summary>
    /// Current speed count, to tell where we are in terms of who has acted and who is next.
    /// </summary>
    protected int speedCount = 0;

    /// <summary>
    /// Flag to ensure we only cycle once through the entire list
    /// in the event that no one can act for a given turn.
    /// </summary>
    protected bool cycled = false;

    protected override void Start()
    {
        base.Start();

        speedCount = StatUtils.CalcMaxStat(StatUtils.MAX_LEVEL);

        foreach (Entity enemy in BattleManager.EnemyParty)
        {
            stats.Add(enemy, new BattleStats(enemy));
        }
        foreach (Entity member in BattleManager.PlayerParty)
        {
            stats.Add(member, new BattleStats(member));
        }
    }

    public override void BattleStep()
    {
        Debug.Log("=== Turn " + Turn);
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
                    int index = AcquireValidTarget(BattleManager.PlayerParty);
                    IncrementAction();
                    target = BattleManager.PlayerParty[index];
                }
                else if (BattleManager.PlayerParty.Contains(actor))
                {
                    int index = AcquireValidTarget(BattleManager.EnemyParty);
                    IncrementAction();
                    target = BattleManager.EnemyParty[index];
                }
                if (target != null)
                {
                    BattleStats targetStats;
                    if (stats.TryGetValue(target, out targetStats))
                    {
                        int dmg = 0;
                        // Calculate damage
                        if (actorStat.Virtue >= actorStat.Spirit)
                        {
                            dmg = Formula.Generate(actorStat.Virtue, targetStats.Resolve, Turn, ActionCount);
                        }
                        else
                        {
                            dmg = Formula.Generate(actorStat.Spirit, targetStats.Spirit, Turn, ActionCount);
                        }
                        targetStats.HP -= dmg;
                        Debug.Log(actor.Name + " damages " + target.Name + " for " + dmg);
                        IncrementAction();
                    }
                }
            }
            speedCount = actorStat.Deft - 1;

            if (ConditionCheck())
            {
                //Battle complete
                Debug.Log("=== Battle complete!");
                break;
            }
        }
        if (!ConditionResult())
        {
            IncrementTurn();
        }
    }

    public override Entity[] GetNextActingEntities()
    {
        List<Entity> acting = new List<Entity>();
        int highest = 0;
        foreach (KeyValuePair<Entity, BattleStats> entry in stats)
        {
            if (entry.Value.Deft > highest && entry.Value.Deft <= speedCount && CanAct(entry.Key))
            {
                highest = entry.Value.Deft;
                acting.Clear();
                acting.Add(entry.Key);
            }
            else if (entry.Value.Deft == highest && CanAct(entry.Key))
            {
                acting.Add(entry.Key);
            }
        }

        if (acting.Count == 0)
        {
            //Reset and try again
            speedCount = StatUtils.CalcMaxStat(StatUtils.MAX_LEVEL);
            if (!cycled)
            {
                cycled = true;
                return GetNextActingEntities();
            }
        }
        else
        {
            cycled = false;
        }

        return acting.ToArray();
    }

    protected int AcquireValidTarget(ReadOnlyCollection<Entity> targets)
    {
        int index = 0;

        List<Entity> available = new List<Entity>();
        foreach (Entity target in targets)
        {
            if (target.HP > 0)
            {
                available.Add(target);
            }
        }
        int availableIndex = random.Range(0, available.Count, actionCount);
        index = targets.IndexOf(available[availableIndex]);

        return index;
    }

}
