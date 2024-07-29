using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System;
using UnityEngine.UI;
using TMPro;
using Oculus.Interaction;
using System.Collections;
public class SendAndReceive : MonoBehaviour
{
    Thread mThread; 
    public string connectionIP = "192.168.0.203";
    public int connectionPort = 25001;
    public GameObject cylinder;
    public GameObject sphere;
    TcpListener listener;
    TcpClient client;
    Vector3 sendPos = Vector3.zero;
    bool running;
    
    bool Down = false;
    bool up = false;
    bool right = false;
    bool left = false;
    bool clockwise = false;
    bool counterclockwise = false;

    public TextMeshProUGUI GestureRecognizer;
    private Renderer CubeRenderer;


    static public int DownCount = 0;
    private void Awake()
    {
        sendPos = transform.position;
        CubeRenderer = GetComponent<Renderer>();
        mThread = new Thread(new ThreadStart(GetInfo));
        mThread.Start();
    }

    private void Update()
    {
        sendPos = transform.position;

        Renderer cylinderRenderer = cylinder.GetComponent<Renderer>();
        Renderer sphereRenderer = sphere.GetComponent<Renderer>();


        // Status log from Classify Data
        if (CylinderGrab.CylinderGrabbed)
            {
                if (Down)
            {
                //CubeRenderer.material.color = Color.black;
                //Down = false;
                cylinderRenderer.material.color = Color.black;
                GestureRecognizer.text = "Down";
                Down = false;
                DownCount++;
            }
            if (up)
            {
                //CubeRenderer.material.color = Color.green;
                //up = false;
                cylinderRenderer.material.color = Color.green;
                GestureRecognizer.text = "Up";
                up = false;
            }
            /*
            if (right)
            {
                cylinderRenderer.material.color = Color.red;
                GestureRecognizer.text = "right";
                right = false;
            }
            if (left)
            {
                cylinderRenderer.material.color = Color.yellow;
                GestureRecognizer.text = "left";
                left = false;
            }
            */
        }
        else if (SphereGrab.SphereGrabbed)
        {
           
            if (counterclockwise)
            {
                GestureRecognizer.text = "counterclockwise";
                sphereRenderer.material.color = Color.gray;
                counterclockwise = false;

            }
            if(clockwise)
            {
                GestureRecognizer.text = "clockwise";
                sphereRenderer.material.color = Color.magenta;
                clockwise = false;
            }
        }
    }

    void GetInfo()
    {
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();
        client = listener.AcceptTcpClient();
        running = true;
        while (running)
        {
            SendAndReceiveData();
        }
        listener.Stop();
    }

    void SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();

        while (running)
        {
            if (ObjectCollision.Collision)
            {
                // Convert the float values to byte array and send
                float[] floatsToSend = { (float)sendPos.x, (float)sendPos.y };
                byte[] send = new byte[8]; // 2 floats * 4 bytes each = 8 bytes
                System.Buffer.BlockCopy(floatsToSend, 0, send, 0, 8);
                nwStream.Write(send, 0, send.Length);
            }
            else
            {
                float[] floatsToSend = { -1000.0f, 0.0f };
                byte[] send = new byte[8]; // 2 floats * 4 bytes each = 8 bytes
                System.Buffer.BlockCopy(floatsToSend, 0, send, 0, 8);
                nwStream.Write(send, 0, send.Length);
            }

            byte[] buffer = new byte[4];
            int bytesRead = nwStream.Read(buffer, 0, buffer.Length);
            Debug.Log("Receiving is okay");
            if (bytesRead <= 0)
            {
               continue;
            }
            int message = BitConverter.ToInt32(buffer, 0);
            if (message == 4)
            {
                up = true;

            }
            else if (message == 5)
            {
                Down = true;
            }
            else if (message == 6)
            {
                right = true;
            }
            else if (message == 7)
            {
                left = true;
            }
            else if (message == 1)
            {
                clockwise = true;
            }
            else if (message == 2)
            {
                counterclockwise = true;
            }
            else
            {
                continue;
            }
        }
    }

}
