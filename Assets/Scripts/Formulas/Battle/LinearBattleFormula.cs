using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Linear-based formula calculation.
/// Follows: m * x + b
/// Where the value of M is dicated by the parent Multiplier
/// Value of X is the attacker value
/// Value of b is dictated by the addition multiplier.
/// </summary>
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
