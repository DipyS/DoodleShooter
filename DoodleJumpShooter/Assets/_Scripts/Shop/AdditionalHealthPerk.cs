using YG;

public class AdditionalHealthPerk : PerksUnlocker
{
    public new void Start()
    {
        price = (YandexGame.savesData.StartHealth - 2) * price * priceMultiplier + priceAdditionar * (YandexGame.savesData.StartHealth - 2);
        base.Start();
    }
    public override void VisualBuy()
    {
        base.VisualBuy();
        GameManager.Instance.player.startHealth += 1;
        YandexGame.savesData.StartHealth += 1;
        YandexGame.SaveProgress();
    }

    public new void OnEnable()
    {
        base.OnEnable();
    }
}
