using System;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Assign child UI elements here.")]
    public Canvas[] childObjects; // Array of child objects

    public UnityEvent<int> onScreenChange;

    public void Awake()
    {
        OpenChild(0);
    }

    /// <summary>
    /// Activates the child at the specified index and disables all other children.
    /// </summary>
    /// <param name="childIndex">The index of the child to activate.</param>
    public void OpenChild(int childIndex)
    {
        if (childObjects == null || childObjects.Length == 0)
        {
            Debug.LogError("Child objects are not assigned.");
            return;
        }

        if (childIndex < 0 || childIndex >= childObjects.Length)
        {
            Debug.LogError($"Invalid child index: {childIndex}. Must be between 0 and {childObjects.Length - 1}.");
            return;
        }

        // Loop through all child objects
        for (int i = 0; i < childObjects.Length; i++)
        {
            childObjects[i].enabled = i == childIndex;
        }
        
        onScreenChange.Invoke(childIndex);
    }
}