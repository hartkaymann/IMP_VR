using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SteeringWheelController : XRBaseInteractable
{
    [SerializeField] private Transform wheelTransform;

    public event Action<float> OnWheelRotation;

    private float currentAngle = 0.0f;

    public float CurrentAngle { get; private set; }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        currentAngle = FindWheelAngle();
        Debug.Log("Select entered");
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        currentAngle = FindWheelAngle();
        Debug.Log("Select exited");
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
                RotateWheel();
        }
    }

    private void RotateWheel()
    {
        Debug.Log("Rotating wheel!");
        float totalAngle = FindWheelAngle();

        float angleDifference = currentAngle - totalAngle;
        wheelTransform.Rotate(0, 0, -angleDifference);

        currentAngle = totalAngle;
        OnWheelRotation?.Invoke(angleDifference);
    }

    private float FindWheelAngle()
    {
        float totalAngle = 0f;

        foreach(IXRSelectInteractor interactor in interactorsSelecting)
        {
            Vector2 direction = FindLocalPoint(interactor.transform.position);
            totalAngle += ConvertToAngle(direction) * FindRotationSensitivity();
        }

        return totalAngle;
    }

    private Vector2 FindLocalPoint(Vector3 position)
    {
        return transform.InverseTransformPoint(position).normalized;
    }

    private float ConvertToAngle(Vector2 direction)
    {
        return Vector2.SignedAngle(transform.up, direction);
    }

    private float FindRotationSensitivity()
    {
        return 1.0f / interactorsSelecting.Count;
    }
}
