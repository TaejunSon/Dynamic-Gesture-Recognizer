using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class HandInteraction : MonoBehaviour
{
    public OVRHand leftHand;
    public OVRHand rightHand;
    public float grabThreshold = 0.8f;
    private GameObject grabbedObject = null;

    void Update()
    {
        CheckGrab(leftHand);
        CheckGrab(rightHand);
    }

    void CheckGrab(OVRHand hand)
    {
        if (hand.GetFingerPinchStrength(OVRHand.HandFinger.Index) > grabThreshold)
        {
            if (grabbedObject == null)
            {
                TryGrabObject(hand);
            }
        }
        else
        {
            if (grabbedObject != null)
            {
                ReleaseObject();
            }
        }
    }

    void TryGrabObject(OVRHand hand)
    {
        Collider[] colliders = Physics.OverlapSphere(hand.transform.position, 0.05f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Grabbable"))
            {
                grabbedObject = collider.gameObject;
                grabbedObject.transform.SetParent(hand.transform);
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                break;
            }
        }
    }

    void ReleaseObject()
    {
        grabbedObject.transform.SetParent(null);
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject = null;
    }
}
