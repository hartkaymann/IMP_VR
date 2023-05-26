using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{

    private InputDevice targetDevice;


    [SerializeField]
    private InputDeviceCharacteristics deviceCharacteristics;


    [SerializeField]
    private GameObject handPrefab;

    private GameObject spawnedHand;

    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        InitializeInput();
    }

    private void InitializeInput()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, devices);


        if (devices.Count > 0)
        {
            targetDevice = devices[0];

        }

        // spawn hand model
        if (spawnedHand == null)
        {
            spawnedHand = Instantiate(handPrefab, transform);
            handAnimator = spawnedHand.GetComponent<Animator>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            InitializeInput();
            return;
        }

        if(spawnedHand != null)
        {
            spawnedHand.SetActive(true);
            UpdateHandAnimation();
        }


    }
    private void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);

        }
        else
        {
            handAnimator.SetFloat("Trigger", 0); // couldn't get trigger value
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);

        }
        else
        {
            handAnimator.SetFloat("Grip", 0); // couldn't get trigger value
        }

    }

}
