using System;

/// <summary>
/// Formula for handling the defensive damage mitigation values.
/// Takes the ratio of the resolve stat to the max multiplier * max stat for highest level
/// </summary>
public class DefenseFormula : IFormula<double>
{
    public MultiplierValues Multiplier { get; set; }

    /// <summary>
    /// The resolve stat reference
    /// </summary>
    private Stats resolveStat;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="resolve">Resolve references</param>
    public DefenseFormula(Stats resolve)
    {
        resolveStat = resolve;
    }

    public double Generate(int level)
    {
        double ratio = 0;
        int raw = resolveStat.GetValue(level);
        ratio = (double)raw / (StatUtils.CalcMaxStat(100) * Multiplier.Maximum);
        return ratio;
    }

    public void SetSeed(int seed)
    {
        //Ignore for now, nothing is randomized here yet.
    }
}
