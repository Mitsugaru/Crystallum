/// <summary>
/// Interface for all formulas.
/// </summary>
public interface IFormula<T> {
    /// <summary>
    /// Multiplier rules for the formula
    /// </summary>
    /// <returns>The min / max multiplier or limit values</returns>
    MultiplierValues Multiplier {get; set;}
    
    /// <summary>
    /// Set the seed of the hash function
    /// </summary>
    /// <param name="seed">seed value</param>
    void SetSeed(int seed);
    
    /// <summary>
    /// Generate a value for the given level.
    /// Most of this will assume that the immediate previous level has an
    /// already generated value. It is not safe to generate values where
    /// previous values have not been generated.
    /// </summary>
    /// <param name="level">level to generate a value for</param>
    /// <returns>value for the given level</returns>
    T Generate(int level);
}
