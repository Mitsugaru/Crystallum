/// <summary>
/// Formula for handling the defensive damage mitigation values.
/// Takes the ratio of the resolve stat to the max multiplier * max stat for highest level
/// </summary>
public class DefenseFormula : AbstractStatFormula<double>
{
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

    public override double Generate(int level)
    {
        double ratio = 0;
        int raw = resolveStat.GetValue(level);
        ratio = (double)raw / (StatUtils.CalcMaxStat(100) * Multiplier.Maximum);
        return ratio;
    }

}
