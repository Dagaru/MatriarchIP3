using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resume : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject pauseScreen;
   

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        
        Time.timeScale = 1f;
        PauseScreen.gameIsPaused = false;
        player.GetComponent<PlayerController>().enabled = true;
        //camera.GetComponent<MouseToLook>().enabled = true;
        player.GetComponent<PickupItem>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }
}
