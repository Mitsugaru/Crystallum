using UnityEngine;
using System.Collections;
using System;

public class LinearBattleFormula : AbstractBattleFormula<int>
{
    /// <summary>
    /// Multiplier range of what can be added to the raw damage
    /// </summary>
    public MultiplierValues AdditionMultiplier { get; set; }

    /// <summary>
    /// The range of the defense multiplier
    /// </summary>
    public MultiplierValues DefenseMultiplier { get; set; }

    /// <summary>
    /// Consturctor
    /// </summary>
    /// <param name="addition">Additional raw damage range</param>
    /// <param name="defense">Defense range</param>
    public LinearBattleFormula(MultiplierValues slope, MultiplierValues addition, MultiplierValues defense)
    {
        this.Multiplier = slope;
        this.AdditionMultiplier = addition;
        this.DefenseMultiplier = defense;
    }

    public override int Generate(int attackerStat, int defenderStat, int turn, int actionCount)
    {
        int rawDmg = attackerStat * random.Range(Multiplier.Minimum, Multiplier.Maximum, turn, actionCount) + random.Range(AdditionMultiplier.Minimum, AdditionMultiplier.Maximum, turn, actionCount);
        rawDmg -= (int)CalculateDefense(defenderStat, turn, actionCount);
        if (rawDmg <= 0)
        {
            rawDmg = 0;
        }
        return rawDmg;
    }

    protected double CalculateDefense(int defenderStat, int turn, int actionCount)
    {
        double value = 0.0;
        if (DefenseMultiplier.Minimum < DefenseMultiplier.Maximum)
        {
            value = defenderStat / (StatUtils.CalcMaxStat(StatUtils.MAX_LEVEL) * random.Range(DefenseMultiplier.Minimum, DefenseMultiplier.Maximum, turn, actionCount));
        }
        else if (DefenseMultiplier.Maximum > 0)
        {
            value = defenderStat / (StatUtils.CalcMaxStat(StatUtils.MAX_LEVEL) * DefenseMultiplier.Maximum);
        }
        else
        {
            value = defenderStat / (StatUtils.CalcMaxStat(StatUtils.MAX_LEVEL));
        }
        return value;
    }
}
