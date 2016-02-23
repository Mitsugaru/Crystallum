using UnityEngine;
using System.Collections;
using System;

public class ExponentialBattleFormula : AbstractBattleFormula<int>
{

    public override int Generate(BattleActorInfo attacker, BattleActorInfo defender, int turn, int actionCount)
    {
        //Curent stat * EXP(current E Value x ((current level / RANDBETWEEN(98, 115)) * 3))
        int dmg = (int)(attacker.Value * Mathf.Exp(StatUtils.CalcEValue(attacker.Level, attacker.Stats.Limit) * ((attacker.Level / random.Range(Multiplier.Minimum, Multiplier.Maximum, turn, actionCount)) * 3)));
        return ApplyDefense(dmg, defender, turn, actionCount);
    }

}
