using UnityEngine;
using System.Collections;

public interface IStatFormula<T> : IFormula<T>
{

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
