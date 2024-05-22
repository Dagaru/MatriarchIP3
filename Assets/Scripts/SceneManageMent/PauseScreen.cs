using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{

    public static bool gameIsPaused;

    [SerializeField] GameObject pauseScreen;

    [SerializeField] GameObject Page1;
    [SerializeField] GameObject Page2;

    [SerializeField] GameObject backButtonOnPage2;
    [SerializeField] GameObject forwardButtonOnPage1;

    public static bool ringTextActive, amuletTextActive;
    [SerializeField] GameObject ringText, amuletText;

    public static bool isOnPage1=false, isOnPage2=false;

    private void Start()
    {
        Page1.SetActive(false);
        Page2.SetActive(false);
        gameIsPaused = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                
            }
            else
            {
                Paused();
            }
        }

        if (isOnPage1)
        {
            Page1.SetActive(true);
            Page2.SetActive(false);
            forwardButtonOnPage1.SetActive(true);

            if (ringTextActive)
            {
                ringText.SetActive(true);
            }
            else
            {
                ringText.SetActive(false);
            }

            if (amuletTextActive)
            {
                amuletText.SetActive(true);
            }
            else
            {
                amuletText.SetActive(false);
            }

            backButtonOnPage2.SetActive(false);
        }


        else if (isOnPage2)
        {
            backButtonOnPage2.SetActive(true);
            forwardButtonOnPage1.SetActive(false);
            Page1.SetActive(false);
            Page2.SetActive(true);
        }
        
        
        /*else
        {
            Page1.SetActive(false);
            Page2.SetActive(false);
        }*/
    }

    /*void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        GetComponent<PlayerController>().enabled = true;
        //camera.GetComponent<MouseToLook>().enabled = true;
        GetComponent<PickupItem>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }*/

    private void Paused()
    {
        pauseScreen.SetActive(true);
        
        Page1.SetActive(true);
        isOnPage1 = true;

        Time.timeScale = 0f;
        gameIsPaused = true;
        GetComponent<PlayerController>().enabled = false;
        //camera.GetComponent<MouseToLook>().enabled = false;
        GetComponent<PickupItem>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    

}