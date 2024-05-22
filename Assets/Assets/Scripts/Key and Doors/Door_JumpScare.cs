using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_JumpScare : MonoBehaviour
{

    [SerializeField] Animator doorAnim;
    public static bool firstTime = false;
    

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DoorScare")
        {
            firstTime = true;
        }
    }
}
