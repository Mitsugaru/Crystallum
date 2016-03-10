using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract battle formula class with basic XXHash random function and multiplier values.
/// </summary>
public abstract class AbstractBattleFormula<T> : IBattleFormula<T>
{
    public MultiplierValues Multiplier { get; set; }

    public MultiplierValues DefenseMultiplier { get; set; }

    /// <summary>
    /// XXHash based hash function
    /// </summary>
    protected HashFunction random = new XXHash(0);

    public abstract T Generate(BattleActorInfo attacker, BattleActorInfo defender, int turn, int actionCount);

    public void SetSeed(int seed)
    {
        random = new XXHash(seed);
    }

    protected int ApplyDefense(int rawDamage, BattleActorInfo defender, int turn, int actionCount)
    {
        double value = 0.0;
        if (DefenseMultiplier.Minimum < DefenseMultiplier.Maximum)
        {
            value = defender.Value / (StatUtils.CalcMaxStat(defender.Stats.Limit) * random.Range(DefenseMultiplier.Minimum, DefenseMultiplier.Maximum, turn, actionCount));
        }
        else if (DefenseMultiplier.Maximum > 0)
        {
            value = defender.Value / (StatUtils.CalcMaxStat(defender.Stats.Limit) * DefenseMultiplier.Maximum);
        }
        else
        {
            value = defender.Value / (StatUtils.CalcMaxStat(defender.Stats.Limit));
        }
        int dmg = rawDamage - (int)value;
        if (dmg < 0)
        {
            dmg = 0;
        }
        return dmg;
    }

}
