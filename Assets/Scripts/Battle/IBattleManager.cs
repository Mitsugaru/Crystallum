using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;

public interface IBattleManager
{

    ReadOnlyCollection<Entity> EnemyParty { get; }

    ReadOnlyCollection<Entity> PlayerParty { get; }

    void AddPartyMember(Entity entity);

    void AddEnemyMember(Entity entity);

    void ClearParty();

    void ClearEnemies();

    void StartBattle();

    void StepBattle();

    bool InBattle();
}
