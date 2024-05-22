using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public static bool isLookingAtLetter;
    [SerializeField] GameObject letterGroup;
    private bool inLetterMode;

    // Start is called before the first frame update
    private void Start()
    {
        inLetterMode = false;
        isLookingAtLetter = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isLookingAtLetter)
            {
                InLetter();
            }
        }

        if (inLetterMode)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OutOfLetter();
                
            }
        }

    }

    private void InLetter()
    {
        letterGroup.SetActive(true);
        inLetterMode = true;
        GetComponent<PauseScreen>().enabled = false;
        Time.timeScale = 0f;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PickupItem>().enabled = false;
    }

    private void OutOfLetter()
    {
        letterGroup.SetActive(false);
        inLetterMode = false;
        GetComponent<PauseScreen>().enabled = true;
        Time.timeScale = 1f;
        GetComponent<PlayerController>().enabled = true;
        GetComponent<PickupItem>().enabled = true;
    }

    
}
