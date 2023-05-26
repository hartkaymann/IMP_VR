using System.Collections.Generic;
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
        if (!TryGetComponent(out rb))
        {
            Debug.LogWarning("Couldn't get rigidbody of car!");
        }
    }

    void Update()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics characteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
        InputDevice device = devices[0];


    }
}
