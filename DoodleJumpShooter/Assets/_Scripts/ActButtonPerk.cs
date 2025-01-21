using UnityEngine.Events;

public class ActButtonPerk : Product
{
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
    }
}
