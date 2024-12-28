using UnityEngine;

public class PerksUnlocker : Product
{
    [SerializeField] protected int priceMultiplier = 1;
    [SerializeField] protected int priceAdditionar = 0;
    public override void VisualBuy()
    {
        price *= priceMultiplier;
        price += priceAdditionar;
        UpdatePriceText();
    }
}
