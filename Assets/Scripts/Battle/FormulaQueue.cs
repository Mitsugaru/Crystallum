using UnityEngine;
using System.Collections.Generic;

public class FormulaQueue
{
    /// <summary>
    /// The set of available formulas.
    /// </summary>
    protected List<IBattleFormula<int>> availableFormulas;

    protected Queue<IBattleFormula<int>> currentQueue = new Queue<IBattleFormula<int>>();
    /// <summary>
    /// The current queue of formulas.
    /// </summary>
    public Queue<IBattleFormula<int>> Queue
    {
        get
        {
            return currentQueue;
        }
    }

    protected int limit = 16;
    /// <summary>
    /// The limit of items for the queue
    /// </summary>
    public int Limit
    {
        get
        {
            return limit;
        }
        set
        {
            if (value > 0)
            {
                limit = value;
                PopulateQueue();
            }
        }
    }

    /// <summary>
    /// Random hash function
    /// </summary>
    protected HashFunction random = new XXHash(0);

    /// <summary>
    /// Current step count for the hash funciton generation
    /// </summary>
    protected int count = 0;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="formulas">Collection of battle formulas</param>
    public FormulaQueue(ICollection<IBattleFormula<int>> formulas)
    {
        HashSet<IBattleFormula<int>> set = new HashSet<IBattleFormula<int>>(formulas);
        availableFormulas = new List<IBattleFormula<int>>(set);
    }

    public void SetSeed(int seed)
    {
        random = new XXHash(seed);
    }

    public void PopulateQueue()
    {
        if (availableFormulas.Count > 0)
        {
            for (int i = currentQueue.Count; i < limit; i++)
            {
                //TODO eventually attach weights to specific formulas
                // and use that to drive what is chosen
                int index = random.Range(0, availableFormulas.Count, count);
                IncrementCount();
                currentQueue.Enqueue(availableFormulas[index]);
            }
        }
    }

    protected void IncrementCount()
    {
        if (count == int.MaxValue)
        {
            count = int.MinValue;
        }
        else
        {
            count++;
        }
    }
}
