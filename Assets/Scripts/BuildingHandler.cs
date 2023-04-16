using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sequence = Unity.VisualScripting.Sequence;

public class BuildingHandler : MonoBehaviour
{
    [SerializeField] private string buildingID;
    [SerializeField] private int price;
    [SerializeField] private GameObject construction;
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
        ControlBuilding(PlayerPrefs.HasKey(BuildingKey()));
    }

    public void BuyBuilding()
    {
        PlayerPrefs.SetInt(BuildingKey(), 1);
        
        Construct();
       // UpdateBuilding(true);
    }

    private void Construct()
    {
        construction.SetActive(true);
    }

    private void ControlBuilding(bool bought)
    {
        _buildingImage.enabled = bought;
        saleObject.SetActive(!bought);
    }

    public void UpdateBuilding(bool bought)
    {
        transform.localScale = Vector3.one * 0.6f;
        _buildingImage.enabled = bought;
        saleObject.SetActive(!bought);

        transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
    }

    private string BuildingKey()
    {
        var sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        return sceneIndex + buildingID;
    }
}
