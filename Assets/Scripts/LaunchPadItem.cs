using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class LaunchPadItem : MonoBehaviour
{
    private Coin _coin;

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI buyButtonText;
    [SerializeField] private ProceduralImage buyButtonImage;
    
    [SerializeField] private ProceduralImage team;
    [SerializeField] private ProceduralImage product;
    [SerializeField] private ProceduralImage community;
    
    [SerializeField] private TextMeshProUGUI teamVal;
    [SerializeField] private TextMeshProUGUI productVal;
    [SerializeField] private TextMeshProUGUI communityVal;


    public void SetCoin(Coin coin)
    {
        _coin = coin;
        icon.sprite = coin.icon;
        nameText.text = coin.Name;
        priceText.text = "$" + coin.unlockPrice;

        teamVal.text = coin.Team.ToString();
        productVal.text = coin.Product.ToString();
        communityVal.text = coin.Community.ToString();

        team.fillAmount = coin.Team / 100f;
        product.fillAmount = coin.Product / 100f;
        community.fillAmount = coin.Community / 100f;

        buyButtonText.text = "Buy";
        buyButtonImage.color = new Color(0,1,0,0.72f);
    }
        
    // Call this method to unlock a coin
    public void UnlockCoin()
    {
        if (User.Instance.UnlockCoin(_coin))
        {
            buyButtonText.text = "Bought";
            buyButtonImage.color = Color.grey;
        }
    }
}