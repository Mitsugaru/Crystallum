using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using strange.extensions.context.api;

public class RootContext : MVCSContext, IRootContext
{

    public RootContext(MonoBehaviour view) : base(view)
    {

    }

    public RootContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {

    }

    protected override void mapBindings()
    {
        base.mapBindings();

        GameObject managers = GameObject.Find("Managers");
        GameObject utilities = GameObject.Find("Utilities");

        injectionBinder.Bind<IRootContext>().ToValue(this).ToSingleton().CrossContext();

        NameGenerator nameGen = utilities.GetComponent<NameGenerator>();
        injectionBinder.Bind<INameGenerator>().ToValue(nameGen).ToSingleton().CrossContext();

        EventManager eventManager = managers.GetComponent<EventManager>();
        injectionBinder.Bind<IEventManager>().ToValue(eventManager).ToSingleton().CrossContext();

        GameManager gameManager = managers.GetComponent<GameManager>();
        injectionBinder.Bind<IGameManager>().ToValue(gameManager).ToSingleton().CrossContext();

        EntityManager entityManager = managers.GetComponent<EntityManager>();
        injectionBinder.Bind<IEntityManager>().ToValue(entityManager).ToSingleton().CrossContext();

        BattleManager battleManager = managers.GetComponent<BattleManager>();
        injectionBinder.Bind<IBattleManager>().ToValue(battleManager).ToSingleton().CrossContext();
    }

    public void Inject(Object o)
    {
        injectionBinder.injector.Inject(o);
    }
}
