using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class BattleUIManager : View
{

    [Inject]
    public IEntityManager EntityManager { get; set; }

    [Inject]
    public IBattleManager BattleManager { get; set; }

    public GameObject CharacterPanelPrefab;

    public Transform enemyPartyPanel;

    public Transform playerPartyPanel;

    public Button enemyGenerateButton;

    public Button enemyClearButton;

    public Button playerGenerateButton;

    public Button playerClearButton;

    public BattleConfig EnemyParty;

    public BattleConfig PlayerParty;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        enemyGenerateButton.onClick.AddListener(HandleEnemyGenerate);
        enemyClearButton.onClick.AddListener(HandleEnemyClear);
        playerGenerateButton.onClick.AddListener(HandlePlayerGenerate);
        playerClearButton.onClick.AddListener(HandlePlayerClear);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void HandleEnemyGenerate()
    {
        if (EnemyParty.partySize > 0)
        {
            StartCoroutine(GenerateEnemyParty());
            enemyGenerateButton.interactable = false;
        }
    }

    private void HandleEnemyClear()
    {
        BattleManager.ClearEnemies();

        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in enemyPartyPanel)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(child => Destroy(child));
        enemyGenerateButton.interactable = true;
    }

    private IEnumerator GenerateEnemyParty()
    {
        Debug.Log(EnemyParty.partySize);
        for (int i = 0; i < EnemyParty.partySize; i++)
        {
            Entity entity = EntityManager.Generate();
            BattleManager.AddEnemyMember(entity);
            GameObject charPanel = Instantiate(CharacterPanelPrefab);
            CharacterPanelScript script = charPanel.GetComponent<CharacterPanelScript>();
            if (script != null)
            {
                script.Character = entity;
            }
            charPanel.transform.SetParent(enemyPartyPanel);
            yield return null;
        }
    }

    private void HandlePlayerGenerate()
    {
        if (PlayerParty.partySize > 0)
        {
            StartCoroutine(GeneratePlayerParty());
            playerGenerateButton.interactable = false;
        }
    }

    private IEnumerator GeneratePlayerParty()
    {
        for (int i = 0; i < PlayerParty.partySize; i++)
        {
            Entity entity = EntityManager.Generate();
            BattleManager.AddPartyMember(entity);
            GameObject charPanel = Instantiate(CharacterPanelPrefab);
            CharacterPanelScript script = charPanel.GetComponent<CharacterPanelScript>();
            if (script != null)
            {
                script.Character = entity;
            }
            charPanel.transform.SetParent(playerPartyPanel);
            yield return null;
        }
        yield return null;
    }

    private void HandlePlayerClear()
    {
        BattleManager.ClearParty();

        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in playerPartyPanel)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(child => Destroy(child));
        playerGenerateButton.interactable = true;
    }

}
