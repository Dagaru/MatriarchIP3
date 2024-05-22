using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryItem : MonoBehaviour
{
    [SerializeField] private float distanceToPlayer = 0;
    [SerializeField] float MaxDist;

    public static bool islooking_at = false;
    [SerializeField] Transform player;
    [SerializeField] private GameObject ArtifactSpawn;

    // public GameObject itemToPickup;
    private void FixedUpdate()
    {
        distanceToPlayer = (this.transform.position - player.position).magnitude;
        ArtifactSpawn.transform.position = this.transform.position;

        if(distanceToPlayer <= MaxDist)
        {
            MouseToLook.NearObj_Box = true;
        }
        else
        {
            MouseToLook.NearObj_Box = false;
        }


        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)) /*&& PickupItem.puzzleDone == true*/)
        {
            if (islooking_at && distanceToPlayer <= MaxDist)
            {
                
                PickupItem.counter++;
                
                              
                PickupItem.puzzleDone = false;
                islooking_at = false;

                //GameObject.Destroy(this.gameObject);
            }
        }
    }
}
