using UnityEngine;

/// <summary>
/// Formula that calculates HP stat values
/// </summary>
public class HealthFormula : AbstractFormula<int>
{
    /// <summary>
    /// Health stat reference
    /// </summary>
    private Stats HealthStat;

    /// <summary>
    /// Vitality stat reference
    /// </summary>
    private Stats VitalityStat;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="healthStat">health stat reference</param>
    /// <param name="vitalityStat">vitality stat reference</param>
    public HealthFormula(Stats health, Stats vitality)
    {
        this.HealthStat = health;
        this.VitalityStat = vitality;
    }

    public override int Generate(int level)
    {
        int nextValue = 0;

        if (level > 1)
        {
            int modifier = Multiplier.Maximum;
            int min = VitalityStat.GetValue(level - 1) * Multiplier.Minimum;
            int max = VitalityStat.GetValue(level) * Multiplier.Maximum;
            // Make sure we don't divide by zero
            if (min != max)
            {
                modifier = random.Range(min, max, level);
            }
            else
            {
                //Be generous and just give max
                modifier = max;
                Debug.Log("occurred at lvl " + level);
            }
            // Take the previous health stat value
            // and add a random value between the previous vitality stat * min multiplier
            // and the current vitality stat * max multiplier and use current level
            // as the input value
            nextValue = HealthStat.GetValue(level - 1) + modifier;
        }
        else if (level == 1)
        {
            //Assume first level
            nextValue = VitalityStat.GetValue(level) * Multiplier.Maximum;
        }

        return nextValue;
    }
}
