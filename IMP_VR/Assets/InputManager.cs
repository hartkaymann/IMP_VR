using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputDevice LeftController { get; private set; }
    public InputDevice RightController { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);
        Debug.Log("Onput devices: " + + inputDevices.Count);

        if (LeftController == null || !LeftController.isValid)
        {
            var leftHandedControllers = new List<InputDevice>();
            var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
            InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);

            if (leftHandedControllers.Count > 0)
                LeftController = leftHandedControllers[0];

            foreach (var device in leftHandedControllers)
            {
                Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
            }
        }

        if (RightController == null || !RightController.isValid)
        {
            var rightHandedControllers = new List<InputDevice>();
            var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
            InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);

            if (rightHandedControllers.Count > 0)
                RightController = rightHandedControllers[0];

            foreach (var device in rightHandedControllers)
            {
                Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
            }
        }
    }

}
