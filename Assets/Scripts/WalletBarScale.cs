using System;
using UnityEngine;

public class WalletBarScale : MonoBehaviour
{

    private void Awake()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(GetScale(), scale.y, scale.z);
    }

    private float GetScale()
    {
        float ratio = (float)Screen.height / Screen.width;
        float scale = (168.7f * Mathf.Pow(ratio, 2)) - (975.61f * ratio) + 2171.2f;
        return scale;
    }
}
