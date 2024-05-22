using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{

    [SerializeField] Animator switch1Anim, switch2Anim, switch3Anim;
    [SerializeField] Animator door1Anim, door2Anim, door3Anim;

    [SerializeField] Transform respawnPoint;

    private Rigidbody rigid;
    private bool isOutOfMap = false;

    private void Update()
    {
        rigid = GetComponent<Rigidbody>();

        if (rigid.velocity.magnitude == 0)
        {
          
            rigid.WakeUp();
        }

        if (isOutOfMap)
        {
            this.transform.position = respawnPoint.position;
            isOutOfMap = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Switch1")
        {
            switch1Anim.SetTrigger("Pressed");
            FindObjectOfType<SoundManager>().Play("MazeSwitch"); //play maze switch
            door1Anim.SetTrigger("Open");
            FindObjectOfType<SoundManager>().Play("MazeDoorsOpen"); //play maze door noise
        }

        if (other.gameObject.name == "Switch2")
        {
            switch2Anim.SetTrigger("Pressed");
            FindObjectOfType<SoundManager>().Play("MazeSwitch"); //play maze switch
            door2Anim.SetTrigger("Open");
            FindObjectOfType<SoundManager>().Play("MazeDoorsOpen"); //play maze door noise
        }

        if (other.gameObject.name == "Switch3")
        {
            switch3Anim.SetTrigger("Pressed");
            FindObjectOfType<SoundManager>().Play("MazeSwitch"); //play maze switch
            door3Anim.SetTrigger("Open");
            FindObjectOfType<SoundManager>().Play("MazeDoorsOpen"); //play maze door noise
        }

        if (other.gameObject.name == "RespawnCatcher")
        {
            isOutOfMap = true;
        }


        if (other.gameObject.tag == "triggerfloor")
        {
            PickupItem.wasPlayingMaze = true;
            PickupItem.puzzleDone = true;
            PickupItem.hasArtifactSpawned = true;
            OpenDoor.isPuzzleDone = true;
            
        }
    }

    
    
   
}
