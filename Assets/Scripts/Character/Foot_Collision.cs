using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot_Collision : MonoBehaviour
{
    public bool isGrounded;

    public void OnTriggerStay(Collider collision)
    {
        isGrounded = true;

    }

    public void OnTriggerExit(Collider collision)
    {
        isGrounded = false;

    }

}
