using UnityEngine;
using System.Collections;
using System;

public class BaseStatFormula : AbstractFormula<int>
{
    private Stats stat;

    public BaseStatFormula(Stats stat)
    {
        this.stat = stat;
    }

    public override int Generate(int level)
    {
        int nextValue = 0;

        if (level > 1)
        {
            // get the previous value and add a random number between
            // the current min and max multiplier values
            nextValue = stat.GetValue(level - 1) + random.Range(Multiplier.Minimum, Multiplier.Maximum, level);
        }
        else if (level == 1)
        {
            nextValue = random.Range(Multiplier.Minimum, Multiplier.Maximum, level);
            // Due to the randomness, we may want to ensure that the level 1 stat
            // should be rerolled if landed on a 1, until it is larger than 1.
            // This is because a stat of 1 (especially for attack) is so weak that
            // it usually results in either 1 or less than 1 values. For more consistent
            // values, we ought to allow the beginning traits to be 2 or 3 (to be
            // determined if these are sufficiently high enough) at minimum before
            // using a random minimum value of 1.
            if (nextValue <= 1)
            {
                if (Multiplier.Maximum > 1)
                {
                    int reroll = level + 1;
                    while (nextValue <= 1)
                    {
                        nextValue = random.Range(Multiplier.Minimum, Multiplier.Maximum, reroll++);
                    }
                }
                else
                {
                    nextValue = 2;
                }
            }
        }

        return nextValue;
    }

}
