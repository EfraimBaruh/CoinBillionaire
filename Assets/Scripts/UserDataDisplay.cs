using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UserDataDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text followerCountText;
    [SerializeField] private GameObject assetPrefab;
    [SerializeField] private Transform ownedAssets;
    
    [Header("Settings")]
    [SerializeField] private float updateInterval = 1f;
    [SerializeField] private List<AssetData> availableAssets; // Reference to all possible assets

    private void OnEnable()
    {
        if (User.Instance != null)
        {
            User.Instance.assetsUpdated.AddListener(UpdateDisplay);
        }
    }

    private void OnDisable()
    {
        if (User.Instance != null)
        {
            User.Instance.assetsUpdated.RemoveListener(UpdateDisplay);
        }
    }

    private void Start()
    {
        UpdateDisplay();
        // Remove the InvokeRepeating since we'll update via events
        // InvokeRepeating(nameof(UpdateDisplay), 0f, updateInterval);
    }

    private void UpdateDisplay()
    {
        if (User.Instance == null || User.Instance.userData == null) return;

        // Update username
        usernameText.text = $"{User.Instance.userData.UserName ?? "Meme Trader"}";

        // Update follower count with formatting
        Debug.LogWarning(FormatNumber(User.Instance.GetFollowerCount()));
        followerCountText.text = $"{FormatNumber(User.Instance.GetFollowerCount())}";

        // Update owned assets list
        UpdateOwnedAssetsList();
    }

    private void UpdateOwnedAssetsList()
    {
        // Get current owned assets
        List<string> ownedAssetIds = User.Instance.GetOwnedAssetIds();
        
        // Get all currently displayed assets
        var displayedAssets = new List<GameObject>();
        for (int i = 0; i < ownedAssets.childCount; i++)
        {
            displayedAssets.Add(ownedAssets.GetChild(i).gameObject);
        }

        foreach (string assetId in ownedAssetIds)
        {
            AssetData asset = availableAssets.Find(a => a.assetId == assetId);
            if (asset == null) continue;

            // Check if this asset is already displayed
            bool isDisplayed = false;
            foreach (var displayed in displayedAssets)
            {
                if (displayed.GetComponentInChildren<TextMeshProUGUI>().text == asset.assetName)
                {
                    isDisplayed = true;
                    displayedAssets.Remove(displayed);
                    break;
                }
            }

            // Only instantiate if not already displayed
            if (!isDisplayed)
            {
                var ownedAsset = Instantiate(assetPrefab, ownedAssets);
                ownedAsset.GetComponentInChildren<TextMeshProUGUI>().text = asset.assetName;
            }
        }

        // Remove any remaining displayed assets that are no longer owned
        foreach (var unusedDisplay in displayedAssets)
        {
            Destroy(unusedDisplay);
        }
    }

    private string FormatNumber(float number)
    {
        if (number >= 1000000)
            return $"{(number / 1000000f):0.#}M";
        if (number >= 1000)
            return $"{(number / 1000f):0.#}K";
        return $"{number:0}";
    }
} 