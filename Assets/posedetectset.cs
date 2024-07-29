using Meta.Voice.Samples.Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class posedetectset : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Cube1;
    public GameObject Cube2;
    public GameObject Cube3;
    public GameObject Cube4;

    private Renderer CubeMaterial1;
    private Renderer CubeMaterial2;
    private Renderer CubeMaterial3;
    private Renderer CubeMaterial4;

    void Start()
    {
        CubeMaterial1 = Cube1.GetComponent<Renderer>();
        CubeMaterial2 = Cube2.GetComponent<Renderer>();
        CubeMaterial3 = Cube3.GetComponent<Renderer>();
        CubeMaterial4 = Cube4.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CylinderGrab.CylinderGrabbed)
        {
            if (SendAndReceive.DownCount >= 1)
            {
                CubeMaterial1.material.color = Color.red;
            }
            if (SendAndReceive.DownCount>= 2)
            {
                CubeMaterial2.material.color = Color.yellow;
            }
            if (SendAndReceive.DownCount >= 3)
            {
                CubeMaterial3.material.color = Color.green;
            }
            if (SendAndReceive.DownCount >= 4)
            {
                CubeMaterial4.material.color = Color.blue;
            }
        }
    }
}
