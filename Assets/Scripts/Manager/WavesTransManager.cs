using System;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WavesTransManager : MonoBehaviour,IGameStateListener
{
    [Header("Refs")]
    [SerializeField] private Upgrades[] upgrades;
    [SerializeField] private GameObject upgradesContainer;
    [SerializeField] private PlayerStatsManager statsManager;
    [SerializeField] private PlayerObject playerObject;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private RectTransform statsTransform;
    [Header("Chest Related")]
    private int chestCollected;
    [SerializeField] private ChestContainerUI chestContainer;
    [SerializeField] private Transform chestContainerParent;
    public static WavesTransManager instance;
    void Awake()
    {
        instance = this;
        Chest.onCollected += ChestCollected;
    }
    void OnDestroy()
    {
        Chest.onCollected -= ChestCollected;
    }

    public void GameStateChangeCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WAVETRANS :
                TryOpenChest();
                OpenStats();
                break;
        }    
    }
    [Button]
    private void Configure()
    {
        chestContainerParent.gameObject.SetActive(false);
        upgradesContainer.SetActive(true);

        for (int i = 0; i < upgrades.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0,Enum.GetValues(typeof(Stats)).Length);
            Stats stats = (Stats) Enum.GetValues(typeof(Stats)).GetValue(randomIndex);
            string randomUpg = Enums.FormatStatName(stats);

            string buttonString;
            Action action = GetPerformedAction(stats,out buttonString);
            Sprite upgradeIcon = ResourcesManager.GetStatsIcon(stats);
            upgrades[i].configure(upgradeIcon,randomUpg,buttonString);
            upgrades[i].button.onClick.RemoveAllListeners();
            upgrades[i].button.onClick.AddListener(() => action?.Invoke());
            upgrades[i].button.onClick.AddListener(() => BonusSelectedCallback());

        }
    }
    private void BonusSelectedCallback()
    {
        Time.timeScale = 1;
        GameManager.instance.SetGameState(GameState.GAME);
    }
    private Action GetPerformedAction(Stats stats,out string buttonString)
    {
        buttonString = "";
        float value;
        value = UnityEngine.Random.Range(1,10);


        switch (stats)
        {
            case Stats.Attack:
                value = UnityEngine.Random.Range(1,10);
                buttonString = "+ " + value.ToString() +"%"; 
                break;
            case Stats.AttackSpeed :
                value = UnityEngine.Random.Range(1,10);
                buttonString = "+ " + value.ToString() +"%";
                break;
            case Stats.CritChance :
                value = UnityEngine.Random.Range(1f,2f);
                buttonString = "+ " + value.ToString("F2") + "x";
                break;
            case Stats.CritDamage :
                value = UnityEngine.Random.Range(1,10);
                buttonString = "+ " + value.ToString() +"%";
                break;
            case Stats.MoveSpeed :
                value = UnityEngine.Random.Range(1,10);
                buttonString = "+ " + value.ToString() +"%";
                break;
            case Stats.MaxHp :
                value = UnityEngine.Random.Range(1,5);
                buttonString = "+ " + value;
                break;
            case Stats.Range :
                value = UnityEngine.Random.Range(0f,1f);
                buttonString = "+ " + value.ToString("F2");
                break;
            case Stats.HpRecoveryRate :
                value = UnityEngine.Random.Range(1,10);
                buttonString = "+ " + value.ToString() +"%";
                break;
            case Stats.Armor :
                value = UnityEngine.Random.Range(1,10);
                buttonString = "+ " + value.ToString() +"%";
                break;
            case Stats.Luck :
                value = UnityEngine.Random.Range(1,10);
                buttonString = "+ " + value.ToString() +"%";
                break;
            case Stats.Dodge :
                value = UnityEngine.Random.Range(1,10);
                buttonString = "+ " + value.ToString() +"%";
                break;
            case Stats.LifeSteal :
                value = UnityEngine.Random.Range(1,10);
                buttonString = "+ " + value.ToString() +"%";
                break;
        }
        return ()=> statsManager.AddStats(stats,value) ;
    }
    private void ChestCollected(Chest chest)
    {   
        chestCollected++;
        GameManager.instance.LevelUpAndChestCallback();
        Debug.Log(chestCollected);
    }
    public bool HasCollectedChest()
    {
        return chestCollected > 0;
    }
    private void OpenStats()
    {
        statsTransform.DOKill();
        statsTransform.anchoredPosition = new Vector2(-1000f,0);
        statsTransform.DOAnchorPosX(0,0.4f).SetEase(Ease.OutSine).SetUpdate(true);
    }
    private void TryOpenChest()
    {
        if(HasCollectedChest())
        {
            foreach (Transform child in chestContainerParent) {
                Destroy(child.gameObject);
            } 
            title.text = "Open chest!";
            ShowObject();
        }else if(PlayerLevel.instance.hasLevelUP())
        {
            title.text = "Level Up !";
            Configure();
        }else ContinueToPlay();
    }

    private void ContinueToPlay()
    {
        Time.timeScale = 1;
        GameManager.instance.SetGameState(GameState.GAME);
    }

    private void ShowObject()
    {
        chestCollected--;
        upgradesContainer.SetActive(false);
        chestContainerParent.gameObject.SetActive(true);

        ObjectDataSO[] objectDataSO = ResourcesManager.objectDataSOs;
        ObjectDataSO randomObj = objectDataSO[UnityEngine.Random.Range(0,objectDataSO.Length)];

        ChestContainerUI chestContainerUI = Instantiate(chestContainer,chestContainerParent);
        chestContainerUI.Configure(randomObj);
        chestContainerUI.TakeButton.onClick.RemoveAllListeners();
        chestContainerUI.TakeButton.onClick.AddListener(()=> TakeButtonCallback(randomObj));
        chestContainerUI.RecycleButton.onClick.AddListener(()=> RecycleButtonCallback(randomObj));

    }

    private void RecycleButtonCallback(ObjectDataSO randomObj)
    {
        CurrencyManager.instance.AddCurrency(randomObj.sellPrice);
        TryOpenChest();
    }

    private void TakeButtonCallback(ObjectDataSO randomObj)
    {
        Debug.Log("clickes");
        playerObject.AddObject(randomObj);
        TryOpenChest();
    }
}
