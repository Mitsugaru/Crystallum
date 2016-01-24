using UnityEngine;

/// <summary>
/// Holds the multiplier min/max values for random number generation.
/// </summary>
public class MultiplierValues
{
    /// <summary>
    /// Minimum multiplier value
    /// </summary>
    /// <returns>Min value</returns>
    public int Minimum { get; protected set; }

    /// <summary>
    /// Maximum multiplier value
    /// </summary>
    /// <returns>Max value</returns>
    public int Maximum { get; protected set; }

    /// <summary>
    /// Costructor
    /// </summary>
    /// <param name="valueOne">First value of the multipler</param>
    /// <param name="valueTwo">Second value of the multipler</param>
    public MultiplierValues(int valueOne, int valueTwo)
    {
        Minimum = Mathf.Min(valueOne, valueTwo);
        Maximum = Mathf.Max(valueOne, valueTwo);
    }
}
