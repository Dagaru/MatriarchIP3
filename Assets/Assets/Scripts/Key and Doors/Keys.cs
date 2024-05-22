using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour
{
    
    private float distanceToPlayer;
    [SerializeField] Transform player;
    [SerializeField] GameObject itemToPickup;
    [SerializeField] float MaxDist;

    [SerializeField] AudioSource pickupAudioPlayer;
  
    public static bool isLookingAt_Key = false;

    // Update is called once per frame
    private void Update()
    {
        distanceToPlayer = (this.transform.position - player.position).magnitude;

        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)) /*&& PickupItem.puzzleDone == true*/)
        {
            if (isLookingAt_Key == true && distanceToPlayer <= MaxDist)
            {
                keyIdentifier(itemToPickup.name);
                Destroy(itemToPickup);
                FindObjectOfType<SoundManager>().Play("KeyPickup");

                isLookingAt_Key = false;
            }
           // print(distanceToPlayer);
        }
    }

    private bool keyIdentifier(string itemName)
    {
        if (itemName == "Key112")
        {
            OpenDoor.Key112 = true;
        }

        return true;
    }
}
