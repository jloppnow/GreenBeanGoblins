using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    int playerIndex = 0; //Used to determine which input to use for the player
    [SerializeField]
    bool canInput = true; //check for if user can control the player

    [SerializeField]
    float moveSpeed = 3.0f; //the speed at which the player can move
    [SerializeField]
    float jumpForce = 50.0f; //how powerful the character's jump is
    [SerializeField]
    bool isJumping = false; //check for if the player is in the middle of a jump

    Animator anim = null; //Controls all of the character's animations (not used yet)

    //Interaction variables
    bool isCarrying = false;          //Stores if the character is holding an item or not.
    GameObject objectCarrying = null; //Stores the current object being carried.

    // Start is called before the first frame update
    void Start()
    {
        //Get the animator if there is one (not used yet)
        anim = GetComponent<Animator>();

    }

    /// <summary>
    /// Moves the player in the direction of the vector passed in
    /// </summary>
    /// <param name="direction">The direction the character is to move in</param>
    public void Move(Vector3 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.Self);

        //increase speed to animate the character as moving
        if (anim != null)
            anim.SetFloat("Speed", direction.magnitude);
            
    }

    // Update is called once per frame
    void Update()
    {
        if (canInput)
        {
            #region Move Mechanic
            float horizAxis = Input.GetAxis("Axis1_P" + playerIndex); //horizontal axis (left/right)
            float vertAxis = Input.GetAxis("Axis2_P" + playerIndex); //vertical axis (forward/back)

            if (horizAxis > 0 || horizAxis < 0)
                Move(Vector3.right * horizAxis);

            if (vertAxis < 0 || vertAxis > 0)
                Move(Vector3.forward * -vertAxis);
            #endregion

            #region Jump Mechanic
            //Button 0 is the A button on a controller
            if (Input.GetButton("Button0_P" + playerIndex) && !isJumping)
            {
                //start a new task = coroutine plays outside of update
                StartCoroutine(CheckGround());
                isJumping = true; //the character is jumping                
                transform.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);                
            }

            #endregion

            if(Input.GetKeyDown("r") && isCarrying)
            {
               //Will be used later to "throw" an object. 
            }
        }
    }

    // Triggers used for interacting with switches, doors, etc.
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entered trigger: " + other.name);
    } 

    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown("e"))
        {
            //Debug.Log("Interacted with: " + other.name);
            
            // Toggle if an object has been in use or not.
            ObjectInteraction interactObject = other.gameObject.GetComponent<ObjectInteraction>();
            interactObject.Interaction();
            interactObject.isUsed = !interactObject.isUsed;           
        }
    }

    // Collisions used for interacting with physics objects.
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Entered collision: " + other.gameObject.name);
    }

    private void OnCollisionStay(Collision other)
    {
        if (Input.GetKeyDown("e") && other.gameObject.tag == "Interact-SmallObject")
        {
            //Debug.Log("Interacted with: " + other.gameObject.name);

            ObjectInteraction interactObject = other.gameObject.GetComponent<ObjectInteraction>();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            // Toggle object in use, remove gravity for object, set object to character.
            interactObject.isUsed = !interactObject.isUsed;
            rb.useGravity = false;
            interactObject.interact = gameObject;           
        }
    }

    /// <summary>
    /// A task that checks for the ground every fame after the player has jumped
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckGround()
    {
        //Delay to prevent ground check from happening right away
        yield return new WaitForSeconds(0.5f);

        //while player is jumping, check if there is any ground or object below
        while (isJumping)
        {
            //if the ground or an object is below the character, player is no longer is jumping
            if (Physics.Linecast(transform.position, transform.position + (Vector3.down * 1.2f), LayerMask.GetMask("Ground", "StaticObject")))
            {
                isJumping = false;                
            }

            //check every frame while character is jumping
            yield return new WaitForEndOfFrame();
        }

        //stop checking for the ground
        yield break;
    }
}
