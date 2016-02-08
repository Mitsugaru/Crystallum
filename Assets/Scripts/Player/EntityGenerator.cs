using UnityEngine;
using System.Collections;

public class EntityGenerator : MonoBehaviour
{

    public int seed = 0;
    protected int currentSeed = 0;

    public int start = 0;

    protected HashFunction random;

    // Use this for initialization
    void Start()
    {
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
        return new Entity(unchecked((int)random.GetHash(start++)));
    }
}
