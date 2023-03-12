using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteSwapper : MonoBehaviour, ISwapper
{
    [SerializeField] private Sprite on;
    [SerializeField] private Sprite off;

    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void Swap(bool swap)
    {
        _image.sprite = swap ? on : off;
    }
}
