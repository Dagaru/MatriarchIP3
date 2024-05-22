using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] protected List<Transform> patrolTargetPoints;
    [SerializeField] private Animator anim;
    //[SerializeField] private GameObject DetectionCube;
    [SerializeField] private GameObject DetectionCube2;
    [SerializeField] private GameObject DetectionCubeLower;
    [SerializeField] private GameObject DetectionCubeHigher;

    //StealthDetection
    [Range(1f, 30f)]
    [SerializeField] private float maxDist;
    [Range(1f, 90f)]
    [SerializeField] private float RightAngle_Raycast;
    [Range(1f, -90f)]
    [SerializeField] private float LeftAngle_Raycast;
    [SerializeField] private LayerMask Player_Mask;

    //Patrolling
    [SerializeField] protected int patrolPointIndex;

    //Attacking
    [SerializeField] float timeBetweenAttacks = 0;
    [SerializeField] bool alreadyAttacked = false;

    //states
    [SerializeField] float sightRange_Slow, SightRange_Fast, attackRange;
    [SerializeField] bool playerInSightRange_Far, playerInSightRange_Close, playerInAttackRange, PlayerInFOV, AttackOnce, LeapOnce, UpperDetection, NormalDetection, LowerDetection;

    [SerializeField] float stage1Speed;
    [SerializeField] float stage2Speed;
    [SerializeField] float stage3Speed;

    private float stage1SpeedDefault;
    private float stage2SpeedDefault;
    private float stage3SpeedDefault;

    private float attackTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        patrolPointIndex = 0;
        attackTimer = 0; //set timer to 0
        stage1SpeedDefault = stage1Speed;
        stage2SpeedDefault = stage2Speed;
        stage3SpeedDefault = stage3Speed;
    }

    private void Update()
    {

        attackTimer = attackTimer + Time.deltaTime;
        print(attackTimer);

        SightDetectionStanding();
        SightDetectionCrouch();
        SightDetectionUpper();

        playerInSightRange_Far = Physics.CheckSphere(this.transform.position, sightRange_Slow, whatIsPlayer);
        playerInSightRange_Close = Physics.CheckSphere(this.transform.position, SightRange_Fast, whatIsPlayer);

        playerInAttackRange = Physics.CheckSphere(this.transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange_Close && !playerInAttackRange && !NormalDetection && !UpperDetection && !LowerDetection)
        {
            print("patrol");
            Patrolling();
        }
        if(NormalDetection && !playerInAttackRange && !playerInSightRange_Close || LowerDetection && !playerInAttackRange && !playerInSightRange_Close || UpperDetection && !playerInAttackRange && !playerInSightRange_Close)
        {
            Debug.Log("I CAN SEE THE PLAYER");

            if (playerInSightRange_Far == true)
            {
               Debug.Log("Goto Player");
               ChasePlayerSlowSpeed();
            }
        }
        if (NormalDetection && playerInSightRange_Close && playerInSightRange_Far && !playerInAttackRange || LowerDetection && !playerInAttackRange && !playerInSightRange_Close || UpperDetection && !playerInAttackRange && !playerInSightRange_Close)
        {
            Debug.Log("Goto Player fast");

            if (LeapOnce == false)
            {
                FindObjectOfType<SoundManager>().Play("Scream");
                anim.SetBool("attackPlayer", true);

                Debug.Log("Play Once");

                LeapOnce = true;
            }


            ChasePlayerFastSpeed();
        }
        if (NormalDetection && playerInAttackRange && playerInSightRange_Close || UpperDetection && playerInAttackRange && playerInSightRange_Close || LowerDetection && playerInAttackRange && playerInSightRange_Close)
        {
            Debug.Log("Hurt Player");
            AttackPlayer();
        }

        if (PickupItem.counter >=1 && attackTimer > 200) // 200 seconds have passed and the player has done the maze script
        {
            AttackPlayer();
        }
       
    }

    private void Patrolling()
    {
        FindObjectOfType<SoundManager>().Play("Idle");

        anim.SetBool("attackPlayer", false);

        Vector3 targetPosition = patrolTargetPoints[patrolPointIndex].position;

        agent.SetDestination(targetPosition);
        agent.speed = stage1Speed;

        if ((transform.position - targetPosition).magnitude < 2)
        {
            ++patrolPointIndex;
            if (patrolPointIndex >= patrolTargetPoints.Count)
            {
                patrolPointIndex = 0;
            }
        }
    }

    private void ChasePlayerSlowSpeed()
    {
        agent.SetDestination(player.position);
        agent.speed = stage2Speed;
    }

    private void ChasePlayerFastSpeed()
    {
        agent.SetDestination(player.position);
        agent.speed = stage3Speed;
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            if(AttackOnce == false)
            {
                PlayerController.health -= 10;


                AttackOnce = true;
            }

            anim.SetBool("attackPlayer", false);

            alreadyAttacked = true;

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        stage1Speed = 0;
        stage2Speed = 0;
        stage3Speed = 0;

        AttackOnce = false;

        Invoke(nameof(BackToNormal), 4f);
    }
    
    private void BackToNormal()
    {
        alreadyAttacked = false;

        stage1Speed = stage1SpeedDefault;
        stage2Speed = stage2SpeedDefault;
        stage3Speed = stage3SpeedDefault;

        LeapOnce = false;  

        NormalDetection = false;
        UpperDetection = false;
        LowerDetection = false;
    }

    private void SightDetectionStanding()
    {
        RaycastHit hit;

        Vector3 LeftAngle;
        Vector3 RightAngle;
        Vector3 LeftAngleDiv;
        Vector3 RightAngleDiv;
        Vector3 LeftAngleMulti;
        Vector3 RightAngleMulti;

        RightAngle = Quaternion.AngleAxis(RightAngle_Raycast, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        LeftAngle = Quaternion.AngleAxis(LeftAngle_Raycast, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        RightAngleDiv = Quaternion.AngleAxis(RightAngle_Raycast / 2, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        LeftAngleDiv = Quaternion.AngleAxis(LeftAngle_Raycast / 2, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        RightAngleMulti = Quaternion.AngleAxis(RightAngle_Raycast * 2, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        LeftAngleMulti = Quaternion.AngleAxis(LeftAngle_Raycast * 2, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;

        //Forward
        if (Physics.Raycast(DetectionCube2.transform.position, DetectionCube2.transform.forward, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCube2.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                NormalDetection = true;

                //make timer before the player is fully detected.
            }
        }
        if (Physics.Raycast(DetectionCube2.transform.position, LeftAngle, out hit, maxDist))
        {
            //Left
            Debug.DrawLine(DetectionCube2.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                NormalDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCube2.transform.position, RightAngle, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCube2.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                NormalDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCube2.transform.position, LeftAngleDiv, out hit, maxDist))
        {
            //Left
            Debug.DrawLine(DetectionCube2.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                NormalDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCube2.transform.position, RightAngleDiv, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCube2.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                NormalDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCube2.transform.position, LeftAngleMulti, out hit, maxDist))
        {
            //Left
            Debug.DrawLine(DetectionCube2.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                NormalDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCube2.transform.position, RightAngleMulti, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCube2.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                NormalDetection = true;
            }
        }

        if (hit.collider.tag != "Player")
        {
            Invoke(nameof(DelaySight), 3f);
        }

            Debug.Log(NormalDetection + " Is player in sight Normal");
    }

    private void DelaySight()
    {
        NormalDetection = false;
    }


    private void SightDetectionCrouch()
    {
        RaycastHit hit;

        Vector3 LeftAngle;
        Vector3 RightAngle;
        Vector3 LeftAngleDiv;
        Vector3 RightAngleDiv;
        Vector3 LeftAngleMulti;
        Vector3 RightAngleMulti;

        RightAngle = Quaternion.AngleAxis(RightAngle_Raycast, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        LeftAngle = Quaternion.AngleAxis(LeftAngle_Raycast, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        RightAngleDiv = Quaternion.AngleAxis(RightAngle_Raycast / 2, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        LeftAngleDiv = Quaternion.AngleAxis(LeftAngle_Raycast / 2, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        RightAngleMulti = Quaternion.AngleAxis(RightAngle_Raycast * 2, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        LeftAngleMulti = Quaternion.AngleAxis(LeftAngle_Raycast * 2, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
       
        if (Physics.Raycast(DetectionCubeLower.transform.position, DetectionCubeLower.transform.forward, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCubeLower.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                LowerDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCubeLower.transform.position, LeftAngle, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCube2.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                LowerDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCubeLower.transform.position, RightAngle, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCubeLower.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                LowerDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCubeLower.transform.position, LeftAngleDiv, out hit, maxDist))
        {

            Debug.DrawLine(DetectionCube2.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                LowerDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCubeLower.transform.position, RightAngleDiv, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCubeLower.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                LowerDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCubeLower.transform.position, LeftAngleMulti, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCube2.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                LowerDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCubeLower.transform.position, RightAngleMulti, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCubeLower.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                LowerDetection = true;
            }
        }

        if (hit.collider.tag != "Player")
        {
            Invoke(nameof(DelaySightCrouch), 3f);
        }

        Debug.Log(LowerDetection + " Is player in sight Lower");
    }

    private void DelaySightCrouch()
    {
        LowerDetection = false;
    }

    private void SightDetectionUpper()
    {
        RaycastHit hit;

        Vector3 LeftAngle;
        Vector3 RightAngle;
        Vector3 LeftAngleDiv;
        Vector3 RightAngleDiv;
        Vector3 LeftAngleMulti;
        Vector3 RightAngleMulti;

        RightAngle = Quaternion.AngleAxis(RightAngle_Raycast, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        LeftAngle = Quaternion.AngleAxis(LeftAngle_Raycast, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        RightAngleDiv = Quaternion.AngleAxis(RightAngle_Raycast / 2, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        LeftAngleDiv = Quaternion.AngleAxis(LeftAngle_Raycast / 2, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        RightAngleMulti = Quaternion.AngleAxis(RightAngle_Raycast * 2, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;
        LeftAngleMulti = Quaternion.AngleAxis(LeftAngle_Raycast * 2, DetectionCubeLower.transform.up) * DetectionCubeLower.transform.forward * maxDist;

        if (Physics.Raycast(DetectionCubeHigher.transform.position, DetectionCubeHigher.transform.forward, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCubeHigher.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                UpperDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCubeHigher.transform.position, LeftAngle, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCubeHigher.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                UpperDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCubeHigher.transform.position, RightAngle, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCubeHigher.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                UpperDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCubeHigher.transform.position, LeftAngleDiv, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCubeHigher.transform.position, hit.point, Color.red);
            
            if (hit.collider.tag == "Player")
            {
                UpperDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCubeHigher.transform.position, RightAngleDiv, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCubeHigher.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                UpperDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCubeHigher.transform.position, LeftAngleMulti, out hit, maxDist))
        {
            //Left
            Debug.DrawLine(DetectionCubeHigher.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                UpperDetection = true;
            }
        }
        if (Physics.Raycast(DetectionCubeHigher.transform.position, RightAngleMulti, out hit, maxDist))
        {
            Debug.DrawLine(DetectionCubeHigher.transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player")
            {
                UpperDetection = true;
            }
        }

        if (hit.collider.tag != "Player")
        {
            Invoke(nameof(DelaySightCrouch), 3f);
        }

        Debug.Log(UpperDetection + " Is player in sight Higher");
    }

    private void UpperDelay()
    {
        UpperDetection = false;
    }
}
