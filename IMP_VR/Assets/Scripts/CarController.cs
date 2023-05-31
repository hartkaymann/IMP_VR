using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.InputManagerEntry;

public class CarController : MonoBehaviour
{

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    private Rigidbody rb;

    void Start()
    {
        if (!TryGetComponent(out rb))
        {
            Debug.LogWarning("Couldn't get rigidbody of car!");
        }

        GetComponentInChildren<SteeringWheelController>().OnWheelRotation += TurnWheels;
    }

    void Update()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics characteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
        InputDevice device = devices[0];

        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out var dir))
        {
            Debug.Log("Moving: " + dir.x);
            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = dir.x * maxMotorTorque;
                    axleInfo.rightWheel.motorTorque = dir.x * maxMotorTorque;
                }
            }
        }
    }

    public void TurnWheels(float diff)
    {
        Debug.Log("Turning Wheels " + diff);
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle += diff;
                axleInfo.rightWheel.steerAngle += diff;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}