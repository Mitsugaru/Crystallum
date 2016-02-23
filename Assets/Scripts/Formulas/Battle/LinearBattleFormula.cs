using UnityEngine;
using System.Collections;
using System;

public class LinearBattleFormula : AbstractBattleFormula<int>
{
    /// <summary>
    /// Multiplier range of what can be added to the raw damage
    /// </summary>
    public MultiplierValues AdditionMultiplier { get; set; }

    public override int Generate(BattleActorInfo attacker, BattleActorInfo defender, int turn, int actionCount)
    {
        // current stat * RANDBETWEEN(1, 5) + RANDBETWEEN(1, 10)
        int dmg = attacker.Value * random.Range(Multiplier.Minimum, Multiplier.Maximum, turn, actionCount) + random.Range(AdditionMultiplier.Minimum, AdditionMultiplier.Maximum, turn, actionCount);
        return ApplyDefense(dmg, defender, turn, actionCount);
    }


}
