using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickupItem : MonoBehaviour
{
    
    public static int counter;
    public static bool puzzleDone = false;

    public static bool isLookingAt_Artifact = false;
    public static bool hasArtifactSpawned = false;
    public static bool changeAlpha = false;
    public static bool wasPlayingMaze, wasPlayingCryptext;


    [SerializeField] Camera playerCam;
    [SerializeField] GameObject mazeStuff;
    [SerializeField] GameObject cryptext;
    [SerializeField] PlayerController character;
    [SerializeField] private GameObject PointLight;

    [SerializeField] private GameObject cryptextPhysicalObj;
    [SerializeField] private GameObject CrossHair;
    [SerializeField] private GameObject GameManagerObj;

    [SerializeField] private Transform ArtifactSpawn;

    [SerializeField] private GameObject Box;

    [SerializeField] private GameObject Artifact1;
    [SerializeField] private GameObject Artifact2;
    [SerializeField] private GameObject Artifact3;

    [SerializeField] Animator mazeBoxAnim;
    
    private Animator artifact1Anim;
    
    // Start is called before the first frame update
    private void Start()
    {
        SetVariables();
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (puzzleDone == false && counter >0)
        {
            playerCam.enabled = false;
            character.enabled = false;
            PointLight.SetActive(false);
            RenderSettings.fog = false;
            //SoundManager.SetActive(false);
            GameManagerObj.SetActive(false);
            StopFoot();
            CrossHair.SetActive(false);
            if (PickupItem.counter == 1)
            {
                mazeStuff.SetActive(true);
                Box.SetActive(false);
                wasPlayingMaze = true;
            }
            //else if (PickupItem.counter == 2)
            //{
            //    cryptext.SetActive(true);
            //    wasPlayingCryptext = true;
            //}

        }

        if (puzzleDone == true)
        {
            if (wasPlayingMaze) //playing maze game
            {
                Box.SetActive(true);
                Box.GetComponent<DestoryItem>().enabled = false;
                Box.GetComponent<BoxCollider>().enabled = false;

                TurnOn();
                mazeStuff.gameObject.SetActive(false);
                
                cryptextPhysicalObj.SetActive(true);

                if (hasArtifactSpawned == true)
                {
                    Instantiate(Artifact1.gameObject, ArtifactSpawn.position, Quaternion.identity); //spawn artifact
                    
                    
                    mazeBoxAnim.SetTrigger("Open"); //play box open anim
                    FindObjectOfType<SoundManager>().Play("MazeBoxOpen"); //play scribble
                    artifact1Anim = GameObject.Find("Artifact1(Clone)").GetComponent<Animator>();
                    artifact1Anim.SetTrigger("Float");
                    Invoke("DestroyItem", 5f);

                    hasArtifactSpawned = false;

                }
            }

            //if (wasPlayingCryptext) //playing cryptext
            //{
            //    TurnOn();

            //    FindObjectOfType<SoundManager>().Play("Scribble");
            //    cryptext.gameObject.SetActive(false);
            //    GameManager.SpawnMatriarch = false;

            //    PauseScreen.amuletTextActive = true;
            //}
        }
    }

    private void SetVariables()
    {
        counter = 0;
        mazeStuff.gameObject.SetActive(false);
        cryptext.gameObject.SetActive(false);
        wasPlayingCryptext = false;
        wasPlayingMaze = false;
    }
    
    private void TurnOn()
    {
        playerCam.enabled = true;
        character.enabled = true;
        PointLight.SetActive(true);

        PlayFoot();
        RenderSettings.fog = true;
        GameManagerObj.SetActive(true);
        //SoundManager.SetActive(true);
        CrossHair.SetActive(true);
    }

    private void DestroyItem()
    {
        GameObject.Destroy(GameObject.Find("Artifact1(Clone)")); //delete artifact as been collected
        PauseScreen.ringTextActive = true; // text for journal activated
        FindObjectOfType<SoundManager>().Play("Scribble"); //play scribble
    }

    private void StopFoot()
    {
        SoundManager.volume = 0f;
    }

    private void PlayFoot()
    {
        SoundManager.volume = PlayerController.settingsDefaultVol;
    }
}
