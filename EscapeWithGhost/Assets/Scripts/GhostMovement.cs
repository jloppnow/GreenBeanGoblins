using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    [SerializeField]
    int playerIndex = 0;
    [SerializeField]
    bool canInput = true;

    [SerializeField]
    float moveSpeed = 3.0f;
    [SerializeField]
    float turnSpeed = 90.0f;

    [SerializeField]
    Transform cameraPivot = null;

    // Start is called before the first frame update
    void Start()
    {
        
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

            if (horizAxis > 0 || horizAxis < 0)
            {
                //move the character in the direction of the camera
                Move(Vector3.right * horizAxis);

                //rotate the player
                transform.root.Rotate(Vector3.up * horizAxis * turnSpeed * Time.deltaTime, Space.World);
            }

            if (vertAxis > 0 || vertAxis < 0)
                Move(Vector3.forward * -vertAxis);
            #endregion

            #region Ghost Flying Mechanic
            //Falling
            if (Input.GetButton("Button1_P" + playerIndex))
                Move(Vector3.down);

            //Rising
            if (Input.GetButton("Button0_P" + playerIndex))
                Move(Vector3.up);
            #endregion

            #region Moving Only Camera
            float horizCameraAxis = Input.GetAxis("Axis4_P" + playerIndex);
            float vertCameraAxis = Input.GetAxis("Axis5_P" + playerIndex);

            if ((horizAxis < 0.1f || horizAxis > -0.1f) && (horizCameraAxis > 0 || horizCameraAxis < 0))
                transform.root.Rotate(Vector3.up * horizCameraAxis * turnSpeed * Time.deltaTime, Space.World); //rotate the root object

            if (vertCameraAxis > 0 || vertCameraAxis < 0)
            {
                Quaternion rotationMin = Quaternion.Euler(new Vector3(-85.0f, 0, 0));
                Quaternion rotationMax = Quaternion.Euler(new Vector3(85.0f, 0, 0));

                Quaternion rotation = transform.root.rotation;

                rotation.x += Quaternion.Euler(new Vector3(-vertCameraAxis * turnSpeed * Time.deltaTime, 0, 0)).x;

                if (rotation.x < rotationMin.x)
                    rotation.x = rotationMin.x;
                else if (rotation.x > rotationMax.x)
                    rotation.x = rotationMax.x;

                //transform.root.localRotation.SetFromToRotation(transform.root.localEulerAngles, rotation.eulerAngles);

                //transform.root.Rotate(Vector3.right * -vertCameraAxis * turnSpeed * Time.deltaTime, Space.Self);
                
            }

            #endregion
        }
    }
}
