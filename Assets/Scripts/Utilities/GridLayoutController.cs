using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutController : MonoBehaviour
{
    [SerializeField] private float widthPercofScreen;
    [SerializeField] private float heightPercofScreen;
    [SerializeField] private float horizontalMaxItem;

    private void Awake()
    {
        AdjustGridLayoutCellSize();
    }

    private void AdjustGridLayoutCellSize()
    {
        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Adjust the cell size based on screen size (example)
        gridLayout.cellSize = new Vector2(screenWidth / widthPercofScreen, screenHeight / heightPercofScreen);
            
        PostHorizontalFit();
    }

    private void PostHorizontalFit()
    {
        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();

        float screenWidth = Screen.width;
        float total = gridLayout.padding.horizontal + gridLayout.spacing.x * (horizontalMaxItem-1) +
                      (gridLayout.cellSize.x * horizontalMaxItem);

        while (total > screenWidth)
        {
            if (gridLayout.padding.left > 0)
                gridLayout.padding.left--;
            if (gridLayout.spacing.x > 0)
                gridLayout.spacing = new Vector2(gridLayout.spacing.x-1, gridLayout.spacing.y);

            if (gridLayout.cellSize.x > 0)
                gridLayout.cellSize = new Vector2(gridLayout.cellSize.x - 1, gridLayout.cellSize.y);
                
                
            total = gridLayout.padding.horizontal + gridLayout.spacing.x +
                    (gridLayout.cellSize.x * horizontalMaxItem);
                
        }
    }

}