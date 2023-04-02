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

    private Vector3 previousPosition;

    private void Start()
    {
        SetMinMax();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            previousPosition = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - previousPosition;
            Vector2 direction = new Vector2(delta.x, delta.y);

            _rigidbody.AddForce(-direction * speed);
            /*transform.position += (Vector3)direction / 10;*/

            previousPosition = Input.mousePosition;
        }

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
