using UnityEngine;
using UnityEngine.XR;

public class CarController : MonoBehaviour
{

    [SerializeField] private Transform steeringWheel;
    [SerializeField] private Transform wheelFrontLeft;
    [SerializeField] private Transform wheelFrontRight;
    [SerializeField] private Transform wheelRearLeft;
    [SerializeField] private Transform wheelRearRight;

    private float speed = 1f;

    private Rigidbody rb;

    void Start()
    {
        if (!TryGetComponent<Rigidbody>(out rb))
        {
            Debug.LogWarning("Couldn't get rigidbody of car!");
        }
    }

    void Update()
    {
        if (InputManager.Instance.LeftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out var dir))
        {
            Debug.Log("Moving on y axis: " + dir.y);
            rb.AddForce(Vector3.forward * (dir.y * speed));
        }
    }
}
