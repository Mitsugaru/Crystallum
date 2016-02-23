using UnityEngine;
using System.Collections;

public interface IEntityManager
{
    Entity Generate();

    Entity Generate(int level);
}
