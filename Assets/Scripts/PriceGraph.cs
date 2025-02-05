using UnityEngine;
using System.Collections.Generic;

public class PriceGraph : MonoBehaviour
{
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int maxDataPoints = 500;  // How many points to show
    [SerializeField] private float graphWidth = 400f;
    [SerializeField] private float graphHeight = 200f;

    private List<float> priceHistory = new List<float>();
    private float minPrice = float.MaxValue;
    private float maxPrice = float.MinValue;

    private void Awake()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
            
        lineRenderer.positionCount = 0;
        lineRenderer.useWorldSpace = false;
    }

    public void AddPrice(float price)
    {
        priceHistory.Add(price);
        
        // Update min/max
        minPrice = Mathf.Min(minPrice, price);
        maxPrice = Mathf.Max(maxPrice, price);

        // Remove oldest point if we exceed maxDataPoints
        if (priceHistory.Count > maxDataPoints)
        {
            priceHistory.RemoveAt(0);
            
            // Recalculate min/max
            minPrice = float.MaxValue;
            maxPrice = float.MinValue;
            foreach (float p in priceHistory)
            {
                minPrice = Mathf.Min(minPrice, p);
                maxPrice = Mathf.Max(maxPrice, p);
            }
        }

        UpdateGraph();
    }

    private void UpdateGraph()
    {
        lineRenderer.positionCount = priceHistory.Count;
        float xStep = graphWidth / (maxDataPoints - 1);
        
        for (int i = 0; i < priceHistory.Count; i++)
        {
            float xPos = i * xStep;
            float yPos = ((priceHistory[i] - minPrice) / (maxPrice - minPrice)) * graphHeight;
            lineRenderer.SetPosition(i, new Vector3(xPos, yPos, 0));
        }
    }

    public void Clear()
    {
        priceHistory.Clear();
        lineRenderer.positionCount = 0;
        minPrice = float.MaxValue;
        maxPrice = float.MinValue;
    }
} 