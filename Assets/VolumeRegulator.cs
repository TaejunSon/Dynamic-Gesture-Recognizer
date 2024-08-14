using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeRegulator : MonoBehaviour
{
    public GameObject Volume;

    private Transform VolumeTransform;
    private Queue<float> fingerHistory = new Queue<float>();
    private int maxLength = 12;
    // Start is called before the first frame update
    void Start()
    {
        VolumeTransform = Volume.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectCollision.Collision)
        {
            fingerHistory.Enqueue(transform.position.y);
            if (fingerHistory.Count > maxLength)
            {
                float[] fingerHistoryArray = fingerHistory.ToArray();
                float delta_y = fingerHistoryArray[0] - fingerHistoryArray[maxLength - 1];
                VolumeTransform.position = new Vector3(VolumeTransform.position.x, VolumeTransform.position.y - delta_y*2, VolumeTransform.position.z);
                fingerHistory = new Queue<float>();
            }
        }
        else
        {
            fingerHistory = new Queue<float>();
        }
    }
}
