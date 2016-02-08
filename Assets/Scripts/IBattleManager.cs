using UnityEngine;
using System.Collections;

public interface IBattleManager
{

    void AddPartyMember(Entity entity);

    void AddEnemyMember(Entity entity);

    void ClearParty();

    void ClearEnemies();
}
