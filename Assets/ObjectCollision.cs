using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    static public bool Collision = false;
    private Renderer TipRenderer;
    // Start is called before the first frame update
    void Start()
    {
        TipRenderer = GetComponent<Renderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Collision)
        {
            TipRenderer.material.color = Color.black;
        }
        if (!Collision)
        {
            TipRenderer.material.color = Color.white;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            Collision = true;
            Debug.Log("Collision Detect");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            Collision = false;
            Debug.Log("Collision out");
        }
    }
}
