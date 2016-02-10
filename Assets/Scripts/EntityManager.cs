using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class EntityManager : View, IEntityManager
{
    [Inject]
    public INameGenerator NameGenerator { get; set; }

    public int seed = 0;
    protected int currentSeed = 0;

    protected int step = int.MinValue;

    protected HashFunction random;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        currentSeed = seed;
        random = new XXHash(currentSeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (seed != currentSeed)
        {
            currentSeed = seed;
            random = new XXHash(currentSeed);
        }
    }

    public Entity Generate()
    {
        int val = step;
        //ensure that we wrap around
        if (step == int.MaxValue)
        {
            step = int.MinValue;
        }
        else
        {
            step++;
        }
        return new Entity(NameGenerator.GenerateName(), unchecked((int)random.GetHash(val)));
    }
}
