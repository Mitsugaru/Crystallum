using UnityEngine;
using System.Collections;
using System;

public class TaperedSquaredBattleFormula : AbstractBattleFormula<int>
{
    public override int Generate(BattleActorInfo attacker, BattleActorInfo defender, int turn, int actionCount)
    {
        //POWER(current stat, 2) / (current stat / RANDBETWEEN(3, 10))
        int dmg = (int)(Mathf.Pow(attacker.Value, 2) / (attacker.Value / random.Range(Multiplier.Minimum, Multiplier.Maximum, turn, actionCount)));
        return ApplyDefense(dmg, defender, turn, actionCount);
    }
}
