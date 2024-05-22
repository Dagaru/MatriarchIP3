using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseToLook : MonoBehaviour
{

    [SerializeField] float maxDist = 2.5f;
    [SerializeField] Camera playerCam;

    [SerializeField] GameObject col1;
    [SerializeField] GameObject col2;

    public static bool NearObj_Box = false;
    public static bool NearObj_Crypt = false;
    public static bool NearObj_Drawer = false;
    public static bool NearObj_Door = false;
    public static bool NearObj_Artifact = false;
    public static bool NearObj_Key = false;
    public static bool NearObj_Note = false;

    [SerializeField] RawImage CrossHair;
    private Color colour = Color.white;

    public bool isLookingAt_Collectable = false;
    public bool isLookingAt_Door = false;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //hide and locks our cursor to the center of the screen
        col1.GetComponent<DestoryItem>().enabled = false;
        col2.GetComponent<DestoryItem>().enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        CrossHair.color = colour;

        RaycastHit hit;

        // this shoots a line at the distance of "maxDist" and once it hits the target returns what is hit.
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, maxDist))
        {
            if (hit.transform.tag == "Door")
            {
                OpenDoor.isLookingAt_Door = true;
            }
            else
            {
                OpenDoor.isLookingAt_Door = false;
            }

            if (hit.transform.tag == "Key")
            {
                Keys.isLookingAt_Key = true;
            }
            else
            {
                Keys.isLookingAt_Key = false;
            }

            if (hit.transform.tag == "Drawer")
            {
                Drawer.isLookingAtDrawer = true;
            }
            else
            {
                Drawer.isLookingAtDrawer = false;
            }

            if (hit.transform.tag == "Letter")
            {
                Letter.isLookingAtLetter = true;
            }
            else
            {
                Letter.isLookingAtLetter = false;
            }

            if(hit.transform.tag == "Artifact")
            {
                PickupItem.isLookingAt_Artifact = true;
            }
            else
            {
                PickupItem.isLookingAt_Artifact = false;
            }


            if (hit.transform.tag == "Collectable")
            {
                if (col1 != null)
                {
                    if (hit.transform.name == "col1")
                    {
                        col1.GetComponent<DestoryItem>().enabled = true;
                    }
                    else
                    {
                        col1.GetComponent<DestoryItem>().enabled = false;
                    }
                }

                if (col2 != null)
                {
                    if (hit.transform.name == "col2")
                    {
                        col2.GetComponent<DestoryItem>().enabled = true;
                    }
                    else
                    {
                        col2.GetComponent<DestoryItem>().enabled = false;
                    }
                }
                DestoryItem.islooking_at = true;
            }
            else
            {
                DestoryItem.islooking_at = false;
            }

            if(NearObj_Drawer == true)
            {
                if (hit.transform.tag == "Drawer" || hit.transform.name == "Key112")
                {
                    VisableCrosshair();
                }
                else
                {
                    DimmedCrosshair();
                }
            }
            else if(NearObj_Door == true)
            {
                if (hit.transform.tag == "Door") 
                {
                    VisableCrosshair();
                }
                else
                {
                    DimmedCrosshair();
                }
            }
            else if (NearObj_Box == true)
            {
                if (hit.transform.tag == "Collectable")
                {
                    VisableCrosshair();
                }
                 else
                {
                    DimmedCrosshair();
                }
            }
            else if(hit.transform.tag == "Letter")
            {
                VisableCrosshair();
            }
            else
            {
                DimmedCrosshair();
            }

        }
    }
        private void VisableCrosshair()
        {
           colour.a = 1f;
        }

        private void DimmedCrosshair()
        {
           colour.a = 0.3f;
        }
}
