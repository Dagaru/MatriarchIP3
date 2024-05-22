using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip NormalWalk;


    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 2f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}
