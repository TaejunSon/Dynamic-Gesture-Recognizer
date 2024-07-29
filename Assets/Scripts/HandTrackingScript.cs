using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingScript : MonoBehaviour
{
    public Camera sceneCamera;
    public OVRHand leftHand;
    public OVRHand rightHand;
    public OVRSkeleton rightHandSkeleton;
    public GameObject trackingPointPrefab;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isRightIndexFingerTracked;

    private Transform rightHandIndexTipTransform;
    private Queue<Vector3> positionHistory;
    private List<GameObject> trackingPoints;
    private Queue<float> pre_process_history;
    private int maxLength = 16;

    void Start()
    {
        transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * 1.0f;
        positionHistory = new Queue<Vector3>(maxLength);
        trackingPoints = new List<GameObject>(maxLength);

        for (int i = 0; i < maxLength; i++)
        {
            GameObject point = Instantiate(trackingPointPrefab);
            point.SetActive(false);
            trackingPoints.Add(point);
        }
    }

    void Update()
    {
        if (rightHand.IsTracked)
        {
            foreach (var b in rightHandSkeleton.Bones)
            {
                if (b.Id == OVRSkeleton.BoneId.Hand_ThumbTip)
                {
                    rightHandIndexTipTransform = b.Transform;
                    break;
                }
            }

            if (rightHandIndexTipTransform != null)
            {
                if (positionHistory.Count >= maxLength)
                {
                    positionHistory.Dequeue();
                }
                positionHistory.Enqueue(rightHandIndexTipTransform.position);
                //Debug.Log("Position X" + rightHandIndexTipTransform.position.x);
                //Debug.Log("Position Y" + rightHandIndexTipTransform.position.y);
                transform.position = rightHandIndexTipTransform.position;
                transform.rotation = rightHandIndexTipTransform.rotation;

                UpdateTrackingPoints();
            }
        }
    }

    void UpdateTrackingPoints()
    {
        int index = 0;
        foreach (Vector3 pos in positionHistory)
        {
            trackingPoints[index].transform.position = pos;
            trackingPoints[index].SetActive(true);
            index++;
        }

        for (int i = index; i < trackingPoints.Count; i++)
        {
            trackingPoints[i].SetActive(false);
        }
    }

    void pre_process_point_history()
    {
       pre_process_history = new Queue<float>(maxLength*2);
       foreach(Vector3 pos in positionHistory)
        {

            pre_process_history.Enqueue(pos.x);
            pre_process_history.Enqueue(pos.y);  
        }
    }

}
