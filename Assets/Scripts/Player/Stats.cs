/// <summary>
/// Holds the stats for an entity, following a given formula for generation.
/// </summary>
public class Stats
{
    /// <summary>
    /// The generated values for the stats.
    /// </summary>
    protected int[] values;

    /// <summary>
    /// The formula that governs the value generation.
    /// </summary>
    /// <returns></returns>
    protected IStatFormula<int> Formula { get; set; }

    private int limit = 100;

    /// <summary>
    /// The upper level limit of the stats.
    /// When changed, will attempt to recalculate.
    /// </summary>
    /// <returns>Level limit</returns>
    public int Limit
    {
        get
        {
            return limit;
        }
        set
        {
            limit = value;
            if (Formula != null)
            {
                Generate();
            }
        }
    }

    /// <summary>
    /// Set the formula to use for value generation
    /// </summary>
    /// <param name="formula">Formula to use</param>
    public void SetFormula(IStatFormula<int> formula)
    {
        this.Formula = formula;
    }

    /// <summary>
    /// Generate the values for the stats
    /// </summary>
    public void Generate()
    {
        values = new int[limit];
        for (int i = 1; i <= limit; i++)
        {
            values[i - 1] = Formula.Generate(i);
        }
    }

    /// <summary>
    /// Get the value for a specified level.
    /// Assumes that the stats have been generated.
    /// </summary>
    /// <param name="level">Level value</param>
    /// <returns>Stat value of the given level</returns>
    public int GetValue(int level)
    {
        int stat = 0;

        if (Formula != null)
        {
            //Atempt to generate if we forgot to.
            if (values == null)
            {
                Generate();
            }

            if (level > 0 && level <= limit)
            {
                stat = values[level - 1];
            }
        }

        return stat;
    }
}
