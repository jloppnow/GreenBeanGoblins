using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject interact = null;
    public bool isUsed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Interaction()
    {
        if (gameObject.tag == "Interact-Switch") // Cause object interact to rotate.
            interact.GetComponent<Animator>().SetBool("Interact", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(isUsed)
        {
            //if (gameObject.tag == "Interact-Switch") // Cause object interact to rotate.
            //    interact.transform.Rotate(Vector3.forward);
            //else if (gameObject.tag == "Interact-SmallObject") // Update object to interact's location.
            //{
            //    //Debug.Log("Picked up item:" + gameObject.name);
            //    transform.position = interact.transform.position;
            //    transform.Translate(Vector3.up);
            //}
               
        }   
        else if(gameObject.tag == "Interact-SmallObject")
        {
            // Set current object to use gravity again.
            gameObject.GetComponent<Rigidbody>().useGravity = true;

        }
    }
}
