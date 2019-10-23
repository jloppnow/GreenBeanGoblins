using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    int playerIndex = 0;
    [SerializeField]
    bool canInput = true;

    [SerializeField]
    float moveSpeed = 3.0f;
    [SerializeField]
    float jumpForce = 50.0f;

    Animator anim = null;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

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
            float horizAxis = Input.GetAxis("Axis1_P" + playerIndex);
            float vertAxis = Input.GetAxis("Axis2_P" + playerIndex);

            if (horizAxis > 0)
                Move(Vector3.right * horizAxis);
            else if (horizAxis < 0)
                Move(Vector3.right * horizAxis);

            if (vertAxis < 0)
                Move(Vector3.forward * -vertAxis);
            else if (vertAxis > 0)
                Move(Vector3.forward * -vertAxis);


            #region Jump Mechanic
            if (Input.GetButton("Button0_P" + playerIndex))
            {
                transform.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
            }

            #endregion
        }
    }
}
