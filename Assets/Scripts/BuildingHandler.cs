using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHandler : MonoBehaviour
{
    [SerializeField] private string buildingID;
    [SerializeField] private int price;
    [SerializeField] private GameObject saleObject;
    [SerializeField] private TextMeshProUGUI priceText;
    
    private Image _buildingImage;

    private void Awake()
    {
        _buildingImage = GetComponent<Image>();
        Initialize();
    }

    private void Initialize()
    {
        GetComponentInChildren<CashConsumer>().Price = price;
        priceText.text = "$" + price;
    }

    private void OnEnable()
    {
        UpdateBuilding(PlayerPrefs.HasKey(BuildingKey()));
    }

    public void BuyBuilding()
    {
        PlayerPrefs.SetInt(BuildingKey(), 1);
        UpdateBuilding(true);
    }

    private void UpdateBuilding(bool bought)
    {
        _buildingImage.enabled = bought;
        saleObject.SetActive(!bought);
    }

    private string BuildingKey()
    {
        var sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        return sceneIndex + buildingID;
    }
}
