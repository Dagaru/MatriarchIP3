using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatriarchFollow : MonoBehaviour
{
    //todo::
    // - Make the AI Rotate and look towards the player at all time when following them. (Notes: use trasform.rotation to target the player) - Complete (may try to add steering behaviours in future)
    // - Have the AI then only decect the player if the player is in line of sight (Notes: Use Raycasting for accurate dedection aswell as radius arround the AI for different states.) - Semi Complete (Todo: Raycast infront of the AI to then detect the player)
    // - Have the AI Wander around or patrol certain points near the character. (Notes: have the character with a radius so when the matriarch is too far, the ai will spawn close by and wander)

    [SerializeField] private Transform Player;

    private float Matriarch_Speed_Normal = 0.5f;
    private bool playerInView = false;  


    private Vector3 Velocity;
    private float Distance;

   [SerializeField] private float Aware_Radius = 3f;
   [SerializeField] private float EnemyAttack_Radius = 1.5f;

    Vector3 LeftAngle;
    Vector3 RightAngle;
    Vector3 forward;

   [SerializeField] private float Max_Dist_Raycast = 10f;
   [SerializeField] private float RightAngle_Raycast = 30f;
   [SerializeField] private float LeftAngle_Raycast = -30f;
   [SerializeField] private LayerMask Player_Mask;

    private void CheckingRadius()
    {
        if(Distance < Aware_Radius && Distance > EnemyAttack_Radius)
        {

                Debug.Log("Enemy Detects Player");
                this.transform.position += Velocity * Matriarch_Speed_Normal * (Distance / Aware_Radius) * Time.deltaTime;

                transform.LookAt(Player.position);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        else if(Distance < EnemyAttack_Radius)
        {
            Debug.Log("Enemy Attacks Player");
        }
        else
        {
           Debug.Log("Enemy Wanders");
           playerInView = false;
        }
    }

    private void DrawRayCastLines()
    {
        forward = transform.TransformDirection(this.transform.forward) * Max_Dist_Raycast;
        RightAngle = Quaternion.AngleAxis(RightAngle_Raycast, transform.up) * transform.forward * Max_Dist_Raycast;
        LeftAngle = Quaternion.AngleAxis(LeftAngle_Raycast, transform.up) * transform.forward * Max_Dist_Raycast;

        Debug.DrawRay(this.transform.position, forward, Color.red);
        Debug.DrawRay(this.transform.position, RightAngle, Color.red);
        Debug.DrawRay(this.transform.position, LeftAngle, Color.red);
    }

    private void FrontDetection()
    {
        Vector3 forward = transform.TransformDirection(this.transform.forward) * Max_Dist_Raycast;

        if (Physics.Raycast(this.transform.position, forward, Max_Dist_Raycast, Player_Mask) || Physics.Raycast(this.transform.position, RightAngle, Max_Dist_Raycast, Player_Mask) || Physics.Raycast(this.transform.position, LeftAngle, Max_Dist_Raycast, Player_Mask))
        {
            playerInView = true;
        }
    }

    void Update()
    {
        Distance = (Player.position - this.transform.position).magnitude;

        Velocity = new Vector3(Player.position.x - transform.position.x, Player.position.y - transform.position.y, Player.position.z - transform.position.z);

        FrontDetection();
        DrawRayCastLines();
        CheckingRadius();

    }
}
