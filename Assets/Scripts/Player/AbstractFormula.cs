/// <summary>
/// Abstract formula class with basic XXHash random function and multiplier values.
/// </summary>
public abstract class AbstractFormula : IFormula
{
    public MultiplierValues Multiplier { get; set; }

    /// <summary>
    /// XXHash based hash function
    /// </summary>
    protected HashFunction random;

    public abstract int Generate(int level);
    
    public void SetSeed(int seed)
    {
        random = new XXHash(seed);
    }
}
