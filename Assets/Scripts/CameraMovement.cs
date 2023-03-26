using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    [Header("Movement Clamp")] 
    [SerializeField]
    private Vector2 min;
    [SerializeField]
    private Vector2 max;

    private Rigidbody _rigidbody;

    private void Start()
    {
        SetMinMax();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector2 direction = new Vector2(horizontal, vertical);
        
        _rigidbody.AddForce(direction / speed);
        /*transform.position += (Vector3)direction / 10;*/
    }

    private void Update()
    {
        Vector3 position = transform.position;
        transform.position = new Vector3(Mathf.Clamp(position.x, min.x, max.x),
            Mathf.Clamp(position.y, min.y, max.y), position.z);
    }

    private void SetMinMax()
    {
        float ratio = Screen.height / Screen.width;

        min.x = (1.15f * Mathf.Pow(ratio, 2)) - 5.86f * ratio + 0.33f;
        max.x = (-1.15f * Mathf.Pow(ratio, 2)) + 5.86f * ratio - 0.33f;

    }
}
