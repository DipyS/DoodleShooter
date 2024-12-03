using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
    [SerializeField] int price;
    [SerializeField] protected string productName;
    [SerializeField] protected ParticleSystem buyParticles;
    [SerializeField] TextMeshProUGUI textPrice;
    [HideInInspector] public bool isSelected;
    protected bool isUnlocked;
    protected TextMeshProUGUI textTip;
    protected static UnityEvent<string> onSelect = new UnityEvent<string>();
    protected static UnityEvent onBuy = new UnityEvent();

    void Start()
    {
        onSelect.AddListener(TryDeselect);
        onBuy.AddListener(CheckPrice);
        textTip = GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>();
        if (Inventory.singleton.CheckUnlocked(productName)) {
            UnlockProduct(false);
        }
        if (productName == "Pistol") {
            WeaponSwitcher.singleton.SwitchWeaponTo(productName);
        }
    }

    public void TryBuy() {
        if (isUnlocked) {
            if (isSelected) {
                Deselect();
                WeaponSwitcher.singleton.SwitchWeaponTo();
            } else {
                Select();
            }
        }
        else if (Inventory.singleton.Money >= price) {
            Inventory.singleton.Money -= price;
            UnlockProduct();
            VisualBuy();
        }
    }

    void CheckPrice() {
        if (isUnlocked) return;
        
        if (Inventory.singleton.Money >= price) textPrice.text = "<color=green>" + price;
        else textPrice.text = "<color=red>" + price;
    }

    void TryDeselect(string newSelectedItem) {
        if (!isUnlocked) return;

        if (newSelectedItem != productName) {
            Deselect();
        }
    }
    void Select() {
        WeaponSwitcher.singleton.SwitchWeaponTo(productName);
        textTip.text = "Selected";
        onSelect.Invoke(productName);
        isSelected = true;
    }

    void Deselect() {
        textTip.text = "Select";
        isSelected = false;
    }

    virtual public void UnlockProduct(bool needSelect = true) {
        onBuy.Invoke();
        Destroy(textPrice.gameObject);
        if (!Inventory.singleton.CheckUnlocked(productName)) Inventory.singleton.Unlock(productName);
        isUnlocked = true;
        if (needSelect) Select();
        else textTip.text = "Select";
    }
    virtual public void VisualBuy() {
        if (buyParticles != null) Instantiate(buyParticles, transform.position, Quaternion.identity);
    }

    void OnEnable()
    {
        if (Inventory.singleton.Money >= price) textPrice.text = "<color=green>" + price;
        else textPrice.text = "<color=red>" + price;
    }
}
