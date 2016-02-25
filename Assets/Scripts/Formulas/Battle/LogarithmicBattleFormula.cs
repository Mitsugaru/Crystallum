using UnityEngine;
using System.Collections;
using System;

public class LogarithmicBattleFormula : AbstractBattleFormula<int>
{
    public override int Generate(BattleActorInfo attacker, BattleActorInfo defender, int turn, int actionCount)
    {
        int maxStat = StatUtils.CalcMaxStat(attacker.Level);
        int modifier = Mathf.Abs(random.Range(Multiplier.Minimum, Multiplier.Maximum, turn, actionCount) - (maxStat - attacker.Value) / maxStat) * maxStat;
        int dmg = (int)(Mathf.Log10(attacker.Value) / Mathf.Log10(maxStat) * maxStat + modifier);
        return ApplyDefense(dmg, defender, turn, actionCount);
    }
}
