using System;
using TMPro;
using UnityEngine;
using YG;

public class Inventory : MonoBehaviour
{
    public static Inventory singleton;
    [SerializeField] bool Reset;
    [SerializeField] TextMeshProUGUI moneyText;

    private int money;
    public int Money {
        get { return money; }

        set {
            money = value;
            if (money < 0) {
                money = 0;
            }
            moneyText.text = money.ToString();
            Save();
        }
    }

    public string[] UnlockedItems {
        get { return YandexGame.savesData.UnlockedItems;}
        set { YandexGame.savesData.UnlockedItems = value;}
    }

    void Awake()
    {
        if (Reset) YandexGame.ResetSaveProgress();
        GameManager.onLoseGame.AddListener(Save);

        if (singleton != null) Destroy(gameObject);
        else singleton = this;
    }

    void Initialize()
    {
        Debug.Log(UnlockedItems.Length);
        
        Load();
    }

    private void Load() {
        GameManager.Instance.highScoresAll = YandexGame.savesData.HighScores;
        Money = YandexGame.savesData.Money;
    }

    public void Save() {
        YandexGame.savesData.HighScores = GameManager.Instance.highScoresAll;
        YandexGame.savesData.Money = Money;

        YandexGame.SaveProgress();
    }

    public void Unlock(string Name) {
        Array.Resize(ref YandexGame.savesData.UnlockedItems, UnlockedItems.Length + 1);
        UnlockedItems[UnlockedItems.Length - 1] = Name;

        string allWeapons = ""; 
        for (int i = 0; i < UnlockedItems.Length; i++) {
            allWeapons += UnlockedItems[i] + "\n";
        }
        Debug.Log(allWeapons);
    }

    public bool CheckUnlocked(string Name) {
        for (int i = 0; i < UnlockedItems.Length; i++) {
            if (UnlockedItems[i] == Name)
                return true;
        }
        return false;
    }

    public void OnEnable() {
        YandexGame.GetDataEvent += Initialize;
    }

    public void OnDisable() {
        YandexGame.GetDataEvent -= Initialize;
    }
}
