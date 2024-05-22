using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private float distanceToPlayer;
    [SerializeField] Transform player;
    [SerializeField] float MaxDist;

    public static bool Key112;
    private Animator anim;
    public static bool isOpen;
    public static bool isLookingAt_Door = false;
    private float timer = 0f;
    
    public GameObject jumpTrig;
    public static bool isPuzzleDone;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        Key112 = false; 
    }

    // Update is called once per frame
    private void Update()
    {
        distanceToPlayer = (this.transform.position - player.position).magnitude;
        timer += Time.deltaTime;
        

        if (distanceToPlayer <= MaxDist)
        {
            MouseToLook.NearObj_Door = true;
        }
        else
        {
            MouseToLook.NearObj_Door = false;
        }


        if (Door_JumpScare.firstTime == true)
        {
            if (isOpen)
            {
                anim.SetTrigger("Close");
                //add sound from sound manager
                Invoke("DoorNoise", 0.5f);
                timer = 0;
                isOpen = false;

                Destroy(jumpTrig);
                Door_JumpScare.firstTime = false;
            }
            
            
        }

        if (isPuzzleDone)
        {
            if (isOpen == false)
            {
                anim.SetTrigger("Open");
                //add sound from sound manager
                //FindObjectOfType<SoundManager>().Play("Door");
                timer = 0;
                isOpen = true;

                Destroy(jumpTrig);
                isPuzzleDone = false;
            }
            else
            {
                isPuzzleDone = false;
            }
            
        }

    
        

        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)) && Key112 == true)
        {
            if (isLookingAt_Door && distanceToPlayer <= MaxDist)
            {
                if (isOpen == false && timer>1.5)
                {
                    anim.SetTrigger("Open");
                    //add sound from sound manager
                    FindObjectOfType<SoundManager>().Play("Door");
                    timer = 0;    
                    isOpen = true;
                }
                else if (isOpen == true && timer > 1.5)
                {
                    anim.SetTrigger("Close");
                    //add sound from sound manager
                    FindObjectOfType<SoundManager>().Play("Door");
                    timer = 0;
                    isOpen = false;

                }

                isLookingAt_Door = false;
            }
        }
    }

    private void DoorNoise()
    {
        FindObjectOfType<SoundManager>().Play("DoorSLAM");
    }

    
}
