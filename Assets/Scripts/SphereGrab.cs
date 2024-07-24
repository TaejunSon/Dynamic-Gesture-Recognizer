using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SphereGrab : MonoBehaviour
{
    public OVRHand rightHand;
    public OVRSkeleton rightHandSkeleton;
    Vector3 objectPosition;
    public static bool SphereGrabbed = false;
    // Start is called before the first frame update

    //public TextMeshProUGUI sphereCheck;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SphereGrabbed)
        {   
            if (rightHand.IsTracked)
            {
                foreach (var b in rightHandSkeleton.Bones)
                {
                    if (b.Id == OVRSkeleton.BoneId.Hand_Middle3)
                    {
                        objectPosition = new Vector3(b.Transform.position.x - 0.05f, b.Transform.position.y, b.Transform.position.z);
                        transform.position = objectPosition;
                    }
                }
            }

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            SphereGrabbed = true;
        }
    }
}
