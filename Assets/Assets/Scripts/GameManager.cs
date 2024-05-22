using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    public static bool SpawnMatriarch = false;
    private bool hasEnemySpawned = false;
    private bool hasParticleSpawned = false;

    private void Update()
    {
        if (SpawnMatriarch == true && PickupItem.puzzleDone == true)
        {
            if(hasParticleSpawned == false)
            {
                hasParticleSpawned = true;
            }

            Invoke("SpawnMatriarchDelay", 0.01f);
        }
        else
        {
            if(hasEnemySpawned == false)
            {
               enemy.SetActive(false);
            }
        }
    }

    private void SpawnMatriarchDelay()
    {
        if (hasEnemySpawned == false)
        {
          enemy.SetActive(true);
          enemy.GetComponent<Enemy>().enabled = false;    
          FindObjectOfType<SoundManager>().Play("Scream");
          Invoke("DespawnParticleEffect", 0.001f);
          hasEnemySpawned = true;
        }

    }

    private void DespawnParticleEffect()
    {
        enemy.GetComponent<Enemy>().enabled = true;
    }
}
