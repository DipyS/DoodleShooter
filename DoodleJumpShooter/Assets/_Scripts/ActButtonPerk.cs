using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
public class ActButtonPerk : Product
{
    [SerializeField] protected Sprite dashButtonSprite;
    [SerializeField] protected Image dashButtonImage;
    protected static UnityEvent<string> onSelect = new UnityEvent<string>();
    public override void BuyAction()
    {
        switch (productName)
        {
            case "Sheald":
                GameManager.Instance.player.actType = Player.ActType.Sheald; 
            break;
            case "Dash": 
                GameManager.Instance.player.actType = Player.ActType.Dash;
            break;
        }
    }
    public override void BuyAction(string namem)
    {
        switch (productName)
        {
            case "Sheald":
                GameManager.Instance.player.actType = Player.ActType.Sheald; 
            break;
            case "Dash": 
                GameManager.Instance.player.actType = Player.ActType.Dash;
            break;
        } 
        dashButtonImage.sprite = dashButtonSprite;
    }
}
