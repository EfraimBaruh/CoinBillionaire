using UnityEngine;
using UnityEngine.UI;
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

    private void Start()
    {
        // Start periodic updates
        InvokeRepeating(nameof(UpdateDisplay), 0f, updateInterval);
    }

    private void UpdateDisplay()
    {
        if (User.Instance == null || User.Instance.userData == null) return;

        // Update username
        usernameText.text = $"User Name: {User.Instance.userData.UserName ?? "Meme Trader"}";

        // Update follower count with formatting
        followerCountText.text = $"Followers: {FormatNumber(User.Instance.GetFollowerCount())}";

        // Update owned assets list
        UpdateOwnedAssetsList();
    }

    private void UpdateOwnedAssetsList()
    {
        List<string> ownedAssetIds = User.Instance.GetOwnedAssetIds();

        foreach (string assetId in ownedAssetIds)
        {
            AssetData asset = availableAssets.Find(a => a.assetId == assetId);
            if (asset != null)
            {
                var ownedAsset = Instantiate(assetPrefab, ownedAssets);
                ownedAsset.GetComponentInChildren<TextMeshProUGUI>().text = asset.assetName;
            }
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