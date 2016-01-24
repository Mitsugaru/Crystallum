using System;
using NUnit.Framework;

[TestFixture]
public class xxHashTest
{
    private static readonly Random random = new Random();
    private int seed;
    private HashFunction function;
    [SetUp]
    public void Init()
    {
        seed = random.Next(int.MinValue, int.MaxValue);
        function = new XXHash(seed);
    }

    [Test]
    public void SameSeedSameNumbers()
    {
        HashFunction duplicate = new XXHash(seed);
        for (int i = 0; i < 100; i++)
        {
            int key = random.Next(int.MinValue, int.MaxValue);
            Assert.AreEqual(function.Value(key), duplicate.Value(key), "Hash functions of same seed given same value should produce same output.");
        }
    }
}
