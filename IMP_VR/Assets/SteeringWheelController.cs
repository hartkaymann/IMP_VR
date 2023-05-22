using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheelController : MonoBehaviour
{
    // Right Hand
    [SerializeField] private GameObject rightHand;
    private Transform rightHandOriginalParent;
    private bool rightHandOnWheel = false;

    // Left Hand
    [SerializeField] private GameObject leftHand;
    private Transform leftHandOriginalParent;
    private bool leftHandOnWheel = false;

    [SerializeField] private Transform[] snapPositions;

    // Wheel / objects controller by wheel
    [SerializeField] private GameObject vehicle;
    private Rigidbody vehicleRigidbody;

    public float currentWheelRotation = 0f;
    private float turnDampening = 250;

    [SerializeField] private Transform directionalObject;

    private void Start()
    {
        if (!vehicle.TryGetComponent(out vehicleRigidbody))
        {
            Debug.LogWarning("Couldn't get vehicle rigidbody!");
        }
    }

    private void Update()
    {
        ReleaseHandsFromWheel();

        ConvertHandRotationToSteeringWheelRotation();

        currentWheelRotation = -transform.rotation.eulerAngles.z;

    }

    private void FixedUpdate()
    {
        TurnVehicle();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            if (rightHandOnWheel == false && false) // When R primary trigger pressed
            {
                PlaceHandOnWheel(ref rightHand, ref rightHandOriginalParent, ref rightHandOnWheel);
            }
            if (leftHandOnWheel == false && false) // When L primary trigger pressed
            { 
                PlaceHandOnWheel(ref leftHand, ref leftHandOriginalParent, ref leftHandOnWheel);
            }
        }
    }

    private void PlaceHandOnWheel(ref GameObject hand, ref Transform originalParent, ref bool handOnWheel)
    {
        // Find closest snap position
        var shortestDistance = Vector3.Distance(snapPositions[0].position, hand.transform.position);
        var bestSnap = snapPositions[0];

        foreach(var snapPosition in snapPositions)
        {
            if(snapPosition.childCount == 0)
            {
                var distance = Vector3.Distance(snapPosition.position, hand.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestSnap = snapPosition;
                }
            }
        }

        // So we can reset parent later
        originalParent = hand.transform.parent;

        // Set best snap as parent
        hand.transform.parent = bestSnap.transform;
        hand.transform.position = bestSnap.transform.position;

        handOnWheel = true;
    }

    private void ReleaseHandsFromWheel()
    {
        if (rightHandOnWheel == true && false) // When R primary trigger released
        {
            rightHand.transform.parent = rightHandOriginalParent;
            rightHand.transform.position = rightHandOriginalParent.position;
            rightHandOnWheel = false;
        }

        if (leftHandOnWheel == true && false) // When L primary trigger released
        {
            leftHand.transform.parent = leftHandOriginalParent;
            leftHand.transform.position = leftHandOriginalParent.position;
            leftHandOnWheel = false;
        }

        if(!leftHandOnWheel && !rightHandOnWheel)
        {
            transform.parent = null;
        }
    }

    private void ConvertHandRotationToSteeringWheelRotation()
    {
        if(rightHandOnWheel && !leftHandOnWheel)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, rightHandOriginalParent.transform.rotation.eulerAngles.z);
            directionalObject.rotation = newRot;
            transform.parent = directionalObject;
        } 
        else if(!rightHandOnWheel && leftHandOnWheel)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, leftHandOriginalParent.transform.rotation.eulerAngles.z);
            directionalObject.rotation = newRot;
            transform.parent = directionalObject;
        }
        else if(rightHandOnWheel && leftHandOnWheel)
        {
            Quaternion newRotLeft = Quaternion.Euler(0, 0, leftHandOriginalParent.transform.rotation.eulerAngles.z);
            Quaternion newRotRight = Quaternion.Euler(0, 0, rightHandOriginalParent.transform.rotation.eulerAngles.z);
            Quaternion finalRot = Quaternion.Slerp(newRotLeft, newRotRight, 1.0f / 2.0f);
            directionalObject.rotation = finalRot;
            transform.parent = directionalObject;
        }
    }

    private void TurnVehicle()
    {
        var turn = -transform.rotation.eulerAngles.z;
        if (turn < -360)
            turn += 360;

        float t = Time.deltaTime * turnDampening;
        vehicleRigidbody.MoveRotation(Quaternion.RotateTowards(vehicle.transform.rotation, Quaternion.Euler(0, turn, 0), t));
    }
}
