using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexFingerTracking : MonoBehaviour
{
 
    public OVRHand leftHand;
    public OVRHand rightHand;
    public OVRSkeleton rightHandSkeleton;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isRightIndexFingerTracked;

    private Transform rightHandIndexTipTransform;

    private Queue<int> collisionHistory;

    private int maxLength = 36;

    public GameObject tappingCube;
    private Renderer cubeRenderer;
    private int FloorCollision;
    private Renderer tipRenderer;
    public static Vector3 indexVector = Vector3.zero;
    public static bool indexCollision;
    private bool DoubleTap;

    public GameObject sphere;
    private Transform gameObjectTransform;
    private Transform CurrentTransform;

    Queue<float> x_que = new Queue<float>();
    Queue<float> y_que = new Queue<float>();
    private int que_len = 12;
    
    void Start()
    {
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        rightHandIndexTipTransform = transform;

        collisionHistory = new Queue<int>();
        cubeRenderer = tappingCube.GetComponent<Renderer>();
        tipRenderer = GetComponent<Renderer>();

        gameObjectTransform = sphere.GetComponent<Transform>();
        CurrentTransform = gameObjectTransform;
        
    }

    void Update()
    {
        int tappingCount = 0;

        if (rightHand.IsTracked)
        {
            foreach (var b in rightHandSkeleton.Bones)
            {
                if (b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                {
                    rightHandIndexTipTransform = b.Transform;
                    break;
                }
            }

        }
        indexVector = rightHandIndexTipTransform.position;
        transform.position = rightHandIndexTipTransform.position;
        if (collisionHistory.Count >= maxLength)
        {
            collisionHistory.Dequeue();
        }
        collisionHistory.Enqueue(FloorCollision);

        int[] collisionHistoryList = collisionHistory.ToArray();


            
        for (int i = 0; i < collisionHistory.Count-2; i++)
        {
            if (collisionHistoryList[i] - collisionHistoryList[i+1] == 1 || collisionHistoryList[i] - collisionHistoryList[i+1]==-1)
            {
                tappingCount++;
            }
        }
        if (tappingCount > 4)
        {
            Debug.Log("Double tapping checking");
            cubeRenderer.material.color = Color.green;
            DoubleTap = true;
        }

        if(DoubleTap)
        {
            if (indexCollision)
            {
                x_que.Enqueue(transform.position.x);
                y_que.Enqueue(transform.position.z);
                Debug.Log("Enque success");
            }
            if (x_que.Count > que_len)
            {
                float[] x_que_array = x_que.ToArray();
                float[] y_que_array = y_que.ToArray();
                float delta_x = x_que_array[0] - x_que_array[que_len - 1];  
                float delta_y = y_que_array[0] - y_que_array[que_len - 1];
                gameObjectTransform.position = new Vector3(gameObjectTransform.position.x - delta_x, gameObjectTransform.position.y , gameObjectTransform.position.z - delta_y);

                x_que = new Queue<float>();
                y_que = new Queue<float>();
            }
        }
        Debug.Log(gameObjectTransform.position.x);
        Debug.Log(gameObjectTransform.position.y);


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tapping"))
        {
            indexCollision = true;
            FloorCollision = 1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tapping"))

        {
            indexCollision = false;
            FloorCollision = 0;
        }
    }

}
