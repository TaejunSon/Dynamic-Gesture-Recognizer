using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class SendAndReceive : MonoBehaviour
{
    public string connectionIP = "192.168.0.203";
    public int connectionPort = 25001;
    public GameObject cylinder;
    public GameObject sphere;
    public GameObject music;
    public GameObject volume;

    TcpListener listener;
    TcpClient client;
    NetworkStream nwStream;
    Vector3 sendPos = Vector3.zero;
    Vector3 sendPos_ = Vector3.zero;

    bool Down = false;
    bool up = false;
    bool right = false;
    bool left = false;
    bool clockwise = false;
    bool counterclockwise = false;

    Queue<int> messageQueue = new Queue<int>();
    private int maxlength = 18;

    public TextMeshProUGUI GestureRecognizer;
    private Renderer CubeRenderer;
    private Renderer MusicRenderer;
    private Transform VolumeTransform;

    private Transform CurrentTransform;

    static public int DownCount = 0;

    private int counting = 0;
    private int counting_ = 0;

    private void Awake()
    {
        sendPos = transform.position;
        CubeRenderer = GetComponent<Renderer>();
        InitializeListener();
        VolumeTransform = volume.GetComponent<Transform>();
        CurrentTransform = VolumeTransform;
    }

    private void InitializeListener()
    {
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();
        Debug.Log("Listening for connections...");
    }

    private void Update()
    {
        Debug.Log(CurrentTransform.localScale.x);
        Debug.Log(CurrentTransform.localScale.y);
        HandleClientConnection();
        SendAndReceiveData();
        UpdateGesture();
    }

    private void HandleClientConnection()
    {
        if (listener.Pending())
        {
            client = listener.AcceptTcpClient();
            nwStream = client.GetStream();
            Debug.Log("Client connected.");
        }
    }

    private void UpdateGesture()
    {
        if (CylinderGrab.CylinderGrabbed || ObjectCollision.Collision || IndexFingerTracking.indexCollision)
        {
            sendPos = transform.position;
        }
        Renderer cylinderRenderer = cylinder.GetComponent<Renderer>();
        Renderer sphereRenderer = sphere.GetComponent<Renderer>();
        Renderer MusicRenderer = music.GetComponent<Renderer>();

        if (CylinderGrab.CylinderGrabbed)
        {
            if (Down)
            {
                cylinderRenderer.material.color = Color.black;
                GestureRecognizer.text = "Down";
                Down = false;
                DownCount++;
            }
            if (up)
            {
                cylinderRenderer.material.color = Color.green;
                GestureRecognizer.text = "Up";
                up = false;
            }

            /*if (right)
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


            //VolumeTransform.position = new Vector3(VolumeTransform.position.x, -0.12f + (float)(counting_ * 0.06), VolumeTransform.position.z);
            //VolumeTransform.localScale = new Vector3((float)(counting_ * 0.1+0.3), (float)(counting_ * 0.1+0.3), (float)0.3);
        }
        else if (SphereGrab.SphereGrabbed)
        {
            if (counterclockwise)
            {
                GestureRecognizer.text = "counterclockwise";
                sphereRenderer.material.color = Color.gray;
                counterclockwise = false;
            }
            if (clockwise)
            {
                GestureRecognizer.text = "clockwise";
                sphereRenderer.material.color = Color.magenta;
                clockwise = false;
            }
        }
        if (IndexFingerTracking.indexCollision) {
            if (counting % 6 == 0)
            {
                MusicRenderer.material.color = Color.red;

            }
            else if (counting % 6 == 1)
            {
                MusicRenderer.material.color = Color.green;
            }
            else if (counting % 6 == 2)
            {
                MusicRenderer.material.color = Color.yellow;
            }
            else if (counting % 6 == 3)
            {
                MusicRenderer.material.color = Color.white;
            }
            else if (counting % 6 == 4)
            {
                MusicRenderer.material.color = Color.gray;
            }
            else if (counting % 6 == 5)
            {
                MusicRenderer.material.color = Color.blue;
            }
        }

    }

    private void SendAndReceiveData()
    {
        if (client == null || !client.Connected || nwStream == null)
        {
            return;
        }
        float[] floatsToSend = { (float)sendPos.x * 10.0f, (float)sendPos.y * 10.0f };
        // Send data
        /*
        if (!ObjectCollision.Collision)
        {
            floatsToSend = new float[] { sendPos_.x*10.0f, sendPos_.y*10.0f };
        }
        else
        {
            sendPos_ = sendPos;
        }
        */
        if (IndexFingerTracking.indexCollision)
        {
            floatsToSend = new float[] { (float)IndexFingerTracking.indexVector.x * 10.0f, (float)IndexFingerTracking.indexVector.y * 10.0f };
            sendPos_ = sendPos;
        }
        if (!CylinderGrab.CylinderGrabbed&& !ObjectCollision.Collision&&!IndexFingerTracking.indexCollision)
        {
            sendPos = sendPos_;
        }
        byte[] send = new byte[8]; // 2 floats * 4 bytes each = 8 bytes
        System.Buffer.BlockCopy(floatsToSend, 0, send, 0, 8);
        nwStream.Write(send, 0, send.Length);

        // Receive data
        if (nwStream.DataAvailable)
        {
            byte[] buffer = new byte[4];
            int bytesRead = nwStream.Read(buffer, 0, buffer.Length);
            Debug.Log("Receiving is okay");
            if (bytesRead > 0)
            {
                int message = BitConverter.ToInt32(buffer, 0);
                messageQueue.Enqueue(message);

                int upNum = 0;
                int downNum = 0;
                int rightNum = 0;
                int leftNum = 0;
                int clockwiseNum = 0;
                int counterNum = 0;
                if (messageQueue.Count == maxlength)
                {
                    foreach (int item in messageQueue)
                    {
                        if (item == 4)
                        {
                            upNum++;
                        }
                        if (item == 5)
                        {
                            downNum++;
                        }
                        if (item == 6)
                        {
                            rightNum++;
                        }
                        if (item == 7)
                        {
                            leftNum++;
                        }
                        if (item == 1)
                        {
                            clockwiseNum++;
                        }
                        if (item == 2)
                        {
                            counterNum++;
                        }

                    }
                    if (CylinderGrab.CylinderGrabbed)
                    {
                        if (upNum > maxlength * 0.3)
                        {
                            up = true;
                            if (counting_ < 5)
                            {
                                counting_++;
                            }
                        }
                        if (downNum > maxlength * 0.3)
                        {
                            Down = true;
                            if (counting_ > 0)
                            {
                                counting_--;
                            }
                        }
                    }
                    if (IndexFingerTracking.indexCollision)
                    {
                        if (leftNum > maxlength * 0.1)
                        {
                            left = true;
                            if (counting > 0)
                            {
                                counting--;
                            }
                        }
                        if (rightNum > maxlength * 0.1)
                        {
                            right = true;
                            counting++;
                        }
                    }
                    if (clockwiseNum > maxlength * 0.2)
                    {
                        clockwise = true;
                    }
                    if (counterNum > maxlength * 0.2)
                    {
                        counterclockwise = true;
                    }
                    messageQueue = new Queue<int>();
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (listener != null)
        {
            listener.Stop();
        }
        if (client != null)
        {
            client.Close();
        }
    }
}
