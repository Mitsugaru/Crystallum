using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class BattleUIManager : View
{

    [Inject]
    public IEventManager EventManager { get; set; }

    [Inject]
    public IEntityManager EntityManager { get; set; }

    [Inject]
    public IBattleManager BattleManager { get; set; }

    public GameObject CharacterPanelPrefab;

    public Transform enemyPartyPanel;

    public Transform playerPartyPanel;

    public GameObject enemyGenerateButton;

    public GameObject enemyClearButton;

    public GameObject playerGenerateButton;

    public GameObject playerClearButton;

    public GameObject configButton;

    public GameObject battleButton;

    public GameObject commandPanel;

    public BattleConfig EnemyParty;

    public BattleConfig PlayerParty;

    private Dictionary<Entity, CharacterPanelScript> panelScripts = new Dictionary<Entity, CharacterPanelScript>();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        configButton.GetComponent<Button>().onClick.AddListener(HandleConfig);
        enemyGenerateButton.GetComponent<Button>().onClick.AddListener(HandleEnemyGenerate);
        enemyClearButton.GetComponent<Button>().onClick.AddListener(HandleEnemyClear);
        playerGenerateButton.GetComponent<Button>().onClick.AddListener(HandlePlayerGenerate);
        playerClearButton.GetComponent<Button>().onClick.AddListener(HandlePlayerClear);
        battleButton.GetComponent<Button>().onClick.AddListener(HandleBattleButton);
    }

    // Update is called once per frame
    void Update()
    {
        battleButton.GetComponent<Button>().interactable = BattleManager.EnemyParty.Count > 0 && BattleManager.PlayerParty.Count > 0;
    }

    private void HandleConfig()
    {
        commandPanel.SetActive(!commandPanel.activeInHierarchy);
    }

    private void HandleEnemyGenerate()
    {
        if (EnemyParty.partySize > 0)
        {
            StartCoroutine(GenerateEnemyParty());
            enemyGenerateButton.GetComponent<Button>().interactable = false;
        }
    }

    private void HandleEnemyClear()
    {
        BattleManager.ClearEnemies();

        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in enemyPartyPanel)
        {
            children.Add(child.gameObject);
            CharacterPanelScript script = child.gameObject.GetComponent<CharacterPanelScript>();
            if (script != null)
            {
                panelScripts.Remove(script.Character);
            }
        }
        children.ForEach(child => Destroy(child));
        enemyGenerateButton.GetComponent<Button>().interactable = true;
    }

    private IEnumerator GenerateEnemyParty()
    {
        for (int i = 0; i < EnemyParty.partySize; i++)
        {
            Entity entity = EntityManager.Generate();
            BattleManager.AddEnemyMember(entity);
            GameObject charPanel = Instantiate(CharacterPanelPrefab);
            CharacterPanelScript script = charPanel.GetComponent<CharacterPanelScript>();
            if (script != null)
            {
                script.Character = entity;
                panelScripts.Add(entity, script);
            }
            charPanel.transform.SetParent(enemyPartyPanel);
            charPanel.name = entity.Name + " Panel";
            yield return null;
        }
    }

    private void HandlePlayerGenerate()
    {
        if (PlayerParty.partySize > 0)
        {
            StartCoroutine(GeneratePlayerParty());
            playerGenerateButton.GetComponent<Button>().interactable = false;
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
                panelScripts.Add(entity, script);
            }
            charPanel.transform.SetParent(playerPartyPanel);
            charPanel.name = entity.Name + " Panel";
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
            CharacterPanelScript script = child.gameObject.GetComponent<CharacterPanelScript>();
            if (script != null)
            {
                panelScripts.Remove(script.Character);
            }
        }
        children.ForEach(child => Destroy(child));
        playerGenerateButton.GetComponent<Button>().interactable = true;
    }

    private void HandleBattleButton()
    {
        configButton.GetComponent<Button>().interactable = false;
        enemyGenerateButton.GetComponent<Button>().interactable = false;
        enemyClearButton.GetComponent<Button>().interactable = false;
        playerGenerateButton.GetComponent<Button>().interactable = false;
        playerClearButton.GetComponent<Button>().interactable = false;
        commandPanel.SetActive(false);
        //TODO tell battle manager to start a battle
        if (BattleManager.InBattle())
        {
            BattleManager.StepBattle();
        }
        else
        {
            BattleManager.StartBattle();
            battleButton.GetComponentInChildren<Text>().text = "Step Battle";
        }
    }
}
