using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

public class Product : MonoBehaviour
{
    [SerializeField] protected int price;
    [SerializeField] protected string productName;
    [SerializeField] protected TextMeshProUGUI textPrice;
    [SerializeField] AudioClip buySound;
    [HideInInspector] public bool isSelected;
    [SerializeField] protected bool ParentsOnSelect = true;
    protected bool isUnlocked;
    protected TextMeshProUGUI textTip;
    static UnityEvent<string> onSelect = new UnityEvent<string>();
    protected static UnityEvent onBuy = new UnityEvent();

    public void Start()
    {
        if (ParentsOnSelect) onSelect.AddListener(TryDeselect);
        onBuy.AddListener(CheckPrice);
        textTip = GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>();
        if (Inventory.singleton.CheckUnlocked(productName)) {
            UnlockProduct(false);
        }
        if (productName == "Pistol") {
            BuyAction("Pistol");
        }
        UpdatePriceText();
    }

    public virtual void TryBuy() {
        if (isUnlocked) {
            if (isSelected) {
                Deselect();
                BuyAction();
            } 
            else {
                Select();
            }
        }
        else {
        
        if (YandexGame.EnvironmentData.language == "ru") textTip.text = "Купить";
        else if (YandexGame.EnvironmentData.language == "en") textTip.text = "Buy";
        else if (YandexGame.EnvironmentData.language == "tr") textTip.text = "Almak";
        
        if (Inventory.singleton.Money >= price) {
            Inventory.singleton.Money -= price;
            VisualBuy();
            }
        }
    }
    public virtual void BuyAction() {
        WeaponSwitcher.singleton.SwitchWeaponTo();
    }
    public virtual void BuyAction(string Name) {
        WeaponSwitcher.singleton.SwitchWeaponTo(Name);
    }
    protected void CheckPrice() {
        if (isUnlocked) return;
        
        UpdatePriceText();
    }

    protected void TryDeselect(string newSelectedItem) {
        if (!isUnlocked) return;

        if (newSelectedItem != productName) {
            Deselect();
        }
    }
    protected virtual void Select() {
        BuyAction(productName);
        onSelect.Invoke(productName);
        isSelected = true;

        if (YandexGame.EnvironmentData.language == "ru") textTip.text = "Выбран";
        else if (YandexGame.EnvironmentData.language == "en") textTip.text = "Selected";
        else if (YandexGame.EnvironmentData.language == "tr") textTip.text = "Seçme";
    }

    protected virtual void Deselect() {
        if (YandexGame.EnvironmentData.language == "ru") textTip.text = "Выбрать";
        else if (YandexGame.EnvironmentData.language == "en") textTip.text = "Select";
        else if (YandexGame.EnvironmentData.language == "tr") textTip.text = "Seçmek";
        isSelected = false;
    }

    public virtual void UnlockProduct(bool needSelect = true) {
        onBuy.Invoke();
        textPrice.gameObject.SetActive(false);
        if (!Inventory.singleton.CheckUnlocked(productName)) Inventory.singleton.Unlock(productName);
        isUnlocked = true;
        if (needSelect) {
            GameManager.Instance.PlaySound(buySound,2);
            Select();
        }
        else Deselect();
    }
    virtual public void VisualBuy() {
        UnlockProduct();
    }
    protected void UpdatePriceText() {
        if (Inventory.singleton.Money >= price) textPrice.text = "<color=green>" + price;
        else textPrice.text = "<color=red>" + price;
    }

    public void OnEnable()
    {
        UpdatePriceText();
        if (textTip == null) textTip = GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>();
        if (isUnlocked) 
        {
            if (!isSelected) 
            {
                if (YandexGame.EnvironmentData.language == "ru") textTip.text = "Выбрать";
                else if (YandexGame.EnvironmentData.language == "en") textTip.text = "Select";
                else if (YandexGame.EnvironmentData.language == "tr") textTip.text = "Seçmek";
            }
            else 
            {
                if (YandexGame.EnvironmentData.language == "ru") textTip.text = "Выбран";
                else if (YandexGame.EnvironmentData.language == "en") textTip.text = "Selected";
                else if (YandexGame.EnvironmentData.language == "tr") textTip.text = "Seçme";
            }
        }
        else 
        {
            if (YandexGame.EnvironmentData.language == "ru") textTip.text = "Купить";
            else if (YandexGame.EnvironmentData.language == "en") textTip.text = "Buy";
            else if (YandexGame.EnvironmentData.language == "tr") textTip.text = "Almak";
        }
    }
}
