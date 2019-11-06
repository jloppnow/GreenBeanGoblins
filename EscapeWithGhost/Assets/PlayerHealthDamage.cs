using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthDamage : MonoBehaviour
{
    public int Health = 10;
    private bool isTouching = true;

    private void Start()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        isTouching = true;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
            Health -= 1;
    }
}
