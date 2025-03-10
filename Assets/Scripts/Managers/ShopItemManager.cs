using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class ShopItemManager : MonoBehaviour
    {
        [SerializeField] private AssetData assetData;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI followerInfoText;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button sellButton;
        [SerializeField] private Button sellButtonOp;
    
        private void Awake()
        {
            InitializeUI();
            buyButton.onClick.AddListener(HandleBuyButtonClick);
            sellButton.onClick.AddListener(HandleSellButtonClick);
        }

        private void InitializeUI()
        {
            if (assetData == null)
            {
                Debug.LogError("Asset Data not assigned to Shop Item!");
                return;
            }

            // Set up the UI elements
            if (iconImage != null) iconImage.sprite = assetData.icon;
            if (nameText != null) nameText.text = assetData.assetName;
            if (priceText != null) priceText.text = Utils.CurrencyToString(assetData.price);
            if (followerInfoText != null) 
            {
                followerInfoText.text = $"+{assetData.followerPerHour:F0}/hr";
            }

            UpdateButtonStates();
        }

        private void HandleBuyButtonClick()
        {
            if (User.Instance.BuyAsset(assetData))
            {
                // Purchase successful
                UpdateButtonStates();
                // You might want to play a success sound or show a notification here
            }
            else
            {
                // Purchase failed - you could show a message to the player
                Debug.Log("Not enough cash or already owned!");
            }
        }

        private void HandleSellButtonClick()
        {
            if (User.Instance.SellAsset(assetData))
            {
                // Sale successful
                UpdateButtonStates();
                // You might want to play a success sound or show a notification here
            }
            else
            {
                Debug.Log("Failed to sell asset!");
            }
        }

        private void UpdateButtonStates()
        {
            if (buyButton == null || sellButton == null) return;

            bool canBuy = User.Instance.userData.Cash >= assetData.price;
            bool isOwned = User.Instance.GetOwnedAssetIds().Contains(assetData.assetId);

            // Update buy button
            buyButton.gameObject.SetActive(!isOwned);
            buyButton.interactable = canBuy;
        
            // Update sell button
            sellButton.gameObject.SetActive(isOwned);
            sellButton.interactable = true;
        
            // Update sell button
            sellButtonOp.gameObject.SetActive(isOwned);
            sellButtonOp.interactable = true;

            // Update button texts
            TextMeshProUGUI buyButtonText = buyButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buyButtonText != null)
            {
                buyButtonText.text = "BUY";
            }

            TextMeshProUGUI sellButtonText = sellButton.GetComponentInChildren<TextMeshProUGUI>();
            if (sellButtonText != null)
            {
                sellButtonText.text = $"SELL \n {Utils.CurrencyToString(assetData.SellPrice)}";
            }
        
            TextMeshProUGUI sellButtonOpText = sellButtonOp.GetComponentInChildren<TextMeshProUGUI>();
            if (sellButtonOpText != null)
            {
                sellButtonOpText.text = $"SELL \n {Utils.CurrencyToString(assetData.price)}";
            }
        }

        private void OnEnable()
        {
            if (User.Instance != null)
            {
                User.Instance.cashUpdated.AddListener(OnCashUpdated);
                UpdateButtonStates();
            }
        }

        private void OnDisable()
        {
            if (User.Instance != null)
            {
                User.Instance.cashUpdated.RemoveListener(OnCashUpdated);
            }
        }

        private void OnCashUpdated(string newValue)
        {
            UpdateButtonStates();
        }
    }
} 