using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OtherButtons : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject settingsScreen;

    public void Credits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void Restart()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        GameManager.SpawnMatriarch = false;

        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        PauseScreen.gameIsPaused = false;
        player.GetComponent<PlayerController>().enabled = true;
        //camera.GetComponent<MouseToLook>().enabled = true;
        player.GetComponent<PickupItem>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Settings()
    {
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(true);

    }

    public void backButon()
    {
        pauseScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }

}
