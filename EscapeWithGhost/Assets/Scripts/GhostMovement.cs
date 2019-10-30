using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    int mainLayer;

    [SerializeField]
    int playerIndex = 0;
    [SerializeField]
    bool canInput = true;

    [SerializeField]
    float moveSpeed = 3.0f;
    [SerializeField]
    float turnSpeed = 90.0f;

    [SerializeField]
    bool isPossessing = false;
    [SerializeField]
    float expelForce = 5.0f;

    [SerializeField]
    bool invertVertCameraLook = false;

    [Space]
    [Header("Lock Settings")]
    [SerializeField]
    bool lockVertCamera = false;
    [SerializeField]
    bool lockHorizCamera = false;
    [SerializeField]
    bool lockVertMovement = false;
    [SerializeField]
    bool lockHorizMovement = false;

    // Start is called before the first frame update
    void Start()
    {
        mainLayer = gameObject.layer;
    }

    public void Move(Vector3 direction)
    {
        transform.root.Translate(direction * moveSpeed * Time.deltaTime, Space.Self);

        //set triggers for Animator Controller
    }

    // Update is called once per frame
    void Update()
    {
        if (canInput)
        {
            #region Basic Moving Mecahnic
            float horizAxis = Input.GetAxis("Axis1_P" + playerIndex);
            float vertAxis = Input.GetAxis("Axis2_P" + playerIndex);

            if (!lockHorizMovement)
            {
                if (horizAxis > 0 || horizAxis < 0)
                {
                    //move the character in the direction of the camera
                    Move(Vector3.right * horizAxis);

                    //rotate the player
                    transform.root.Rotate(Vector3.up * horizAxis * turnSpeed * Time.deltaTime, Space.World);
                }

                if (vertAxis > 0 || vertAxis < 0)
                    Move(Vector3.forward * -vertAxis);
            }
            #endregion

            #region Ghost Flying Mechanic
            if (!lockVertMovement)
            {
                //Falling
                if (Input.GetButton("Button1_P" + playerIndex))
                    Move(Vector3.down);

                //Rising
                if (Input.GetButton("Button0_P" + playerIndex))
                    Move(Vector3.up);
            }
            #endregion

            #region Moving Only Camera
            if (!lockHorizCamera)
            {
                float horizCameraAxis = Input.GetAxis("Axis4_P" + playerIndex);
                if ((horizAxis < 0.1f || horizAxis > -0.1f) && (horizCameraAxis > 0 || horizCameraAxis < 0))
                    transform.root.Rotate(Vector3.up * horizCameraAxis * turnSpeed * Time.deltaTime, Space.World); //rotate the root object
            }

            if (!lockVertCamera)
            { 
                float vertCameraAxis = Input.GetAxis("Axis5_P" + playerIndex);
                if (vertCameraAxis > 0 || vertCameraAxis < 0)
                {
                    if (!invertVertCameraLook)
                        transform.root.Rotate(Vector3.right * -vertCameraAxis * turnSpeed * Time.deltaTime, Space.Self);
                    else
                        transform.root.Rotate(Vector3.right * vertCameraAxis * turnSpeed * Time.deltaTime, Space.Self);
                }
                /* If anyone wants to help with this = this is the most fullproof code for locking rotation between angles but still doesnt fully lock
                //Vector3 angles = transform.root.eulerAngles;

                //if (vertCameraAxis < 0 && angles.x < 90 && angles.x >= 85)
                //    transform.root.rotation = Quaternion.Euler(85, angles.y, angles.z);
                //else if (vertCameraAxis > 0 && angles.x > 270 && angles.x < 275)
                //    transform.root.rotation = Quaternion.Euler(275, angles.y, angles.z);
                */
            }

            #endregion

            #region GhostPossess
            //input to possess an object
            if (Input.GetButtonDown("Button2_P" + playerIndex))
            {
                if (!isPossessing)
                    PossessObject();
                else
                    EscapePossession();
            }

            #endregion
        }
    }

    public void PossessObject()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position + transform.forward.normalized, transform.forward, out hitInfo))
        {
            Transform hitTransform = hitInfo.transform;

            //if the object can be possessed
            if (!hitTransform.CompareTag("StaticObject"))
            {
                transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.GetComponent<Collider>().enabled = false;
                transform.position = hitTransform.gameObject.transform.position;
                transform.rotation = hitTransform.rotation;
                transform.parent = hitTransform;
                gameObject.layer = hitTransform.gameObject.layer;
                LockSettings(true, false, true, false);

                isPossessing = true;
            }
        }
    }

    public void LockSettings(bool VertCamera, bool HorizCamera, bool VertMove, bool HorizMove)
    {
        lockVertCamera = VertCamera;
        lockVertMovement = VertMove;
        lockHorizCamera = HorizCamera;
        lockHorizMovement = HorizMove;
    }

    public void EscapePossession()
    {
        transform.parent = null;
        LockSettings(false, false, false, false);
        StartCoroutine(ExpelFromObject());
        gameObject.layer = mainLayer;
        isPossessing = false;
    }

    IEnumerator ExpelFromObject()
    {
        bool SafeAway = false;
        Rigidbody rbody = gameObject.GetComponent<Rigidbody>();
        canInput = false;

        while (!SafeAway)
        {
            rbody.AddForce(Vector3.up * expelForce);

            Collider[] colliders = Physics.OverlapSphere(transform.position, transform.GetComponent<SphereCollider>().radius);
            if (colliders.Length <= 1)
                SafeAway = true;

            Debug.Log("Expelling");

            yield return new WaitForEndOfFrame();
        }

        canInput = true;
        rbody.velocity = Vector3.zero;
        Debug.Log("Finished Expelling");
        transform.GetComponent<Collider>().enabled = true;
        yield break;
    }
}
