using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;

    [SerializeField] float walkSpeed_Default = 6.0f;
    [SerializeField] float SprintSpeed_Default = 5.0f;

    [SerializeField] private Animator anim;
    private bool isCrouching = false;

    private float walkSpeed = 6.0f;

    [SerializeField] float Sound_Volume = 1;
    [SerializeField] float sound_Pitch = 1;


    public static int health = 0;
    private int MaxHP = 40;
    public static float settingsDefaultVol;

    [SerializeField] float gravity = -9.81f;

    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    [SerializeField] private PostProcessVolume PVolume;
    private ColorGrading _cGrading;
    private ChromaticAberration _cAberation;

    [SerializeField] bool lockCursor = true;

    private float cameraPitch = 0.0f;
    private float velocityY = 0.0f;
    private CharacterController controller = null;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        PVolume.profile.TryGetSettings(out _cGrading);
        PVolume.profile.TryGetSettings(out _cAberation);

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        health = MaxHP;
    }

    private void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
        PlayerHealth();
    }

    private void PlayerHealth()
    {
        if(health >= 30)
        {
            health = 30;

            _cAberation.intensity.value = 0.133f;
            _cGrading.hueShift.value = 0;
        }
        else if(health == 20)
        {
            // make screen slightly red
            _cAberation.intensity.value = 0.67f;
            _cGrading.hueShift.value = 11f;
        }
        else if(health == 10)
        {
            // make screen very Red
            _cAberation.intensity.value = 1f;
            _cGrading.hueShift.value = 40;
        }
        else if(health <= 0)
        {
            health = 0;

            _cAberation.intensity.value = 1f;
            _cGrading.hueShift.value = 45;

            SceneManager.LoadScene("BlackScreen");
        }

       // Debug.Log(health);
    }

    private void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    private void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (controller.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("isCrouching", true);
            SoundManager.pitch = sound_Pitch = 0.2f;
            SoundManager.Volume = 0.002f;
            isCrouching = true;
            walkSpeed = (walkSpeed_Default - 3) / 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetBool("isCrouching", false);
            SoundManager.pitch = sound_Pitch = 0.2f;
            isCrouching = false;
            walkSpeed = walkSpeed_Default;
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            walkSpeed = SprintSpeed_Default;
            SoundManager.pitch = sound_Pitch = 1.5f;
        }
        else
        {
            walkSpeed = walkSpeed_Default;
            SoundManager.pitch = sound_Pitch = 0.9f;
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
        {
            SoundManager.Volume = 0f;
        }
        else
        {
            if(isCrouching == false)
            {
              SoundManager.Volume = settingsDefaultVol = 0.07f;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "StartSound")
        {
            FindObjectOfType<SoundManager>().Play("NormalWalk");
            FindObjectOfType<SoundManager>().Play("Ambience");
            FindObjectOfType<SoundManager>().Play("Rain");
            FindObjectOfType<SoundManager>().Play("Chandlier");
            SoundManager.NormalWalk_Loop = true;
        }

        if(collision.gameObject.tag == "MatriarchSpawn")
        {
            GameManager.SpawnMatriarch = true;
        }

        if (collision.gameObject.name == "basement1" || collision.gameObject.name == "Basement2")
        {
            FindObjectOfType<SoundManager>().Play("StairsBasement");
            FindObjectOfType<SoundManager>().stop("NormalWalk");
            FindObjectOfType<SoundManager>().stop("StairsMainHall");

            SoundManager.basementStairs_Loop = true;
            SoundManager.NormalWalk_Loop = false;
        }

        if (collision.gameObject.name == "mainHall1" || collision.gameObject.name == "MainHall2" || collision.gameObject.name == "MainHall3" || collision.gameObject.name == "MainHall4")
        {
            FindObjectOfType<SoundManager>().Play("StairsMainHall");
            FindObjectOfType<SoundManager>().stop("StairsBasement");
            FindObjectOfType<SoundManager>().stop("NormalWalk");

            SoundManager.hallWayStairs_Loop = true;
            SoundManager.NormalWalk_Loop = false;
            SoundManager.basementStairs_Loop = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "MatriarchSpawn")
        {
            GameManager.SpawnMatriarch = false;
        }

        if (collision.gameObject.name == "basement1" || collision.gameObject.name == "Basement2")
        {
            FindObjectOfType<SoundManager>().Play("NormalWalk");
            FindObjectOfType<SoundManager>().stop("StairsMainHall");
            FindObjectOfType<SoundManager>().stop("StairsBasement");

            SoundManager.NormalWalk_Loop = true;
            SoundManager.basementStairs_Loop = false;
        }

        if (collision.gameObject.name == "mainHall1" || collision.gameObject.name == "MainHall2" || collision.gameObject.name == "MainHall3" || collision.gameObject.name == "MainHall4")
        {
            FindObjectOfType<SoundManager>().Play("NormalWalk");
            FindObjectOfType<SoundManager>().stop("StairsMainHall");
            FindObjectOfType<SoundManager>().stop("StairsBasement");

            SoundManager.NormalWalk_Loop = true;
            SoundManager.basementStairs_Loop = false;
        }
    }
}
