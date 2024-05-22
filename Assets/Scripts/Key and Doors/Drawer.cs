using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Drawer : MonoBehaviour
{
    public static bool isLookingAtDrawer;
    [SerializeField] Animator draw;
    private bool isOpen;
    private float timer;
    private bool firstTime = true;

    [SerializeField] Transform player;

    private float distanceToPlayer = 0;
    [SerializeField] float MaxDist;

    // Start is called before the first frame update
    private void Start()
    {
        isOpen = false;
        draw.SetBool("toOpen", false);
        draw.SetBool("toClose", true);
    }

    // Update is called once per frame
    private void Update()
    {
        distanceToPlayer = (this.transform.position - player.position).magnitude;
        CheckLook();
        timer = timer + Time.deltaTime;

        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)) && firstTime && isLookingAtDrawer)// had to add this to fix the drawer not opening on 1st click bug
        {
            OpenDraw();
        }
        
        if (isLookingAtDrawer && timer >= 0.5)
        {
            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)))
            {
                if (isOpen && distanceToPlayer <= MaxDist)
                {
                    OpenDraw();
                }
                else if (isOpen == false)
                {
                    CloseDraw();
                }
            }
            
        }


    }

    private void CheckLook()
    {
        if (distanceToPlayer <= MaxDist)
        {
            MouseToLook.NearObj_Drawer = true;
        }
        else
        {
            MouseToLook.NearObj_Drawer = false;
        }
    }

    private void OpenDraw()
    {
        draw.SetBool("toOpen", true);
        draw.SetBool("toClose", false);
        FindObjectOfType<SoundManager>().Play("Drawer");
        isOpen = false;
        timer = 0;
        firstTime = false;
    }

    private void CloseDraw()
    {
        draw.SetBool("toOpen", false);
        draw.SetBool("toClose", true);
        FindObjectOfType<SoundManager>().Play("Drawer");
        isOpen = true;
        timer = 0;
    }

}
