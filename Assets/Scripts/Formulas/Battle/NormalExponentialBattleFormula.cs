using UnityEngine;
using System.Collections;
using System;

public class NormalExponentialBattleFormula : AbstractBattleFormula<int>
{
    public override int Generate(BattleActorInfo attacker, BattleActorInfo defender, int turn, int actionCount)
    {
        int dmg = (int)(attacker.Value * Mathf.Pow(random.Range(Multiplier.Minimum, Multiplier.Maximum), StatUtils.CalcEValue(attacker.Level, attacker.Stats.Limit)));
        return ApplyDefense(dmg, defender, turn, actionCount);
    }
}
