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

    private void OnCollisionEnter(Collision collision)
    {
        isTouching = true;
        if (collision.transform.tag == "Enemy")
            Health -= 1;
    }
    //public void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.tag == "Enemy")
    //        Health -= 1;
    //}
}
