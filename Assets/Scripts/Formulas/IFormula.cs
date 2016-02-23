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

}
