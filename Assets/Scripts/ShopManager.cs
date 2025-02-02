using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject[] shopItems;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    
    private int currentIndex = 0;

    private void Start()
    {
        // Set up button listeners
        if (nextButton != null)
            nextButton.onClick.AddListener(ShowNextItem);
        if (previousButton != null)
            previousButton.onClick.AddListener(ShowPreviousItem);

        // Initialize the display
        UpdateShopDisplay();
    }

    private void UpdateShopDisplay()
    {
        // Hide all items first
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopItems[i].SetActive(i == currentIndex);
        }

        // Update navigation buttons
        if (previousButton != null)
            previousButton.interactable = currentIndex > 0;
        if (nextButton != null)
            nextButton.interactable = currentIndex < shopItems.Length - 1;
        
    }

    private void ShowNextItem()
    {
        if (currentIndex < shopItems.Length - 1)
        {
            currentIndex++;
            UpdateShopDisplay();
        }
    }

    private void ShowPreviousItem()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateShopDisplay();
        }
    }

    // Optional: Public method to jump to a specific index
    public void ShowItem(int index)
    {
        if (index >= 0 && index < shopItems.Length)
        {
            currentIndex = index;
            UpdateShopDisplay();
        }
    }
} 