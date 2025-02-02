using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace DefaultNamespace
{
    public class LaunchPadItem : MonoBehaviour
    {
        public int unlockPrice;
        private Coin _coin;

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI buyButtonText;
        [SerializeField] private ProceduralImage buyButtonImage;

        public void SetCoin(Coin coin)
        {
            _coin = coin;
            icon.sprite = coin.icon;
            nameText.text = coin.name;
            priceText.text = "$" + unlockPrice;
        }
        
        // Call this method to unlock a coin
        public void UnlockCoin()
        {
            
            if (!AppData.UnlockedCoins.Contains(_coin.id))
            {
                AppData.UnlockedCoins.Add(_coin.id);
                AppData.SaveUnlockedCoins();
                buyButtonText.text = "Bought";
                buyButtonImage.color = Color.grey;
            }
        }
    }
}