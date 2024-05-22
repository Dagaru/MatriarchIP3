using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioSource[] sources;
    public static float volume;
    public static float pitch;
    public static string nameOfSound;
    public static bool basementStairs_Loop;
    public static bool hallWayStairs_Loop;
    public static bool NormalWalk_Loop;

    [SerializeField] private AudioSource audioSrc1;//, //audioSrc2, audioSrc3;
    [SerializeField] private AudioSource audioSrc2;
    [SerializeField] private AudioSource audioSrc3;
    [SerializeField] private AudioSource audioSrc4_Screaming;
    [SerializeField] private AudioSource audioSrc5_Ambience;
    [SerializeField] private AudioSource audioSrc6_KeyPickup;
    [SerializeField] private AudioSource audioSrc7_ScribbleNoise;
    [SerializeField] private AudioSource audioSrc8_Door;
    [SerializeField] private AudioSource audioSrc9_MatriarchIdle;
    [SerializeField] private AudioSource audioSrc10_Drawer;
    [SerializeField] private AudioSource audioSrc11_doorSlamNoise;
    [SerializeField] private AudioSource audioSrc12_mazeBoxOpen;
    [SerializeField] private AudioSource audioSrc13_mazeDoorOpen;
    [SerializeField] private AudioSource audioSrc14_mazeSwitch;
    [SerializeField] private AudioSource audioSrc15_BallRoll;
    [SerializeField] private AudioSource audioSrc16_Rain;
    [SerializeField] private AudioSource audioSrc17_ChandlierSquek;


    [Range(0f, 1.1f)]
    [SerializeField] private float SpacialBlend;

    [Range(0f, 360f)]
    [SerializeField] private float spread;

    public static float GlobalVolume;
    public static float Volume;

    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.NormalWalk;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    private void Start()
    {
        sources = this.gameObject.GetComponents<AudioSource>();
        audioSrc1 = sources[0];
        audioSrc2 = sources[1];
        audioSrc3 = sources[2];
        audioSrc4_Screaming = sources[3];
        audioSrc5_Ambience = sources[4];
        audioSrc6_KeyPickup = sources[5];
        audioSrc7_ScribbleNoise = sources[6];
        audioSrc8_Door = sources[7];
        audioSrc9_MatriarchIdle = sources[8];
        audioSrc10_Drawer = sources[9];
        audioSrc11_doorSlamNoise = sources[10];
        audioSrc12_mazeBoxOpen = sources[11];
        audioSrc13_mazeDoorOpen = sources[12];
        audioSrc14_mazeSwitch = sources[13];
        audioSrc15_BallRoll = sources[14];
        audioSrc16_Rain = sources[15];
        audioSrc17_ChandlierSquek = sources[16];
    }

    public void Update()
    {
        audioSrc1.volume = Volume;
        audioSrc1.pitch = pitch;
        audioSrc1.loop = basementStairs_Loop;

        audioSrc2.volume = Volume;
        audioSrc2.pitch = pitch;
        audioSrc2.loop = NormalWalk_Loop;

        audioSrc3.volume = Volume;
        audioSrc3.pitch = pitch;
        audioSrc3.loop = hallWayStairs_Loop;

        
        audioSrc4_Screaming.spatialBlend = SpacialBlend;
        audioSrc4_Screaming.playOnAwake = false;

        audioSrc7_ScribbleNoise.playOnAwake = false;
        audioSrc7_ScribbleNoise.loop = false;
        audioSrc8_Door.playOnAwake = false;
        audioSrc8_Door.loop = false;

        audioSrc9_MatriarchIdle.spatialBlend = SpacialBlend;
        audioSrc9_MatriarchIdle.spread = spread;
        audioSrc9_MatriarchIdle.loop = true;
        audioSrc9_MatriarchIdle.playOnAwake = false;

        audioSrc12_mazeBoxOpen.playOnAwake = false;
        audioSrc12_mazeBoxOpen.loop = false;

        audioSrc13_mazeDoorOpen.playOnAwake = false;
        audioSrc13_mazeDoorOpen.loop = false;

        audioSrc14_mazeSwitch.playOnAwake = false;
        audioSrc14_mazeSwitch.loop = false;

        audioSrc16_Rain.loop = true;
        audioSrc16_Rain.spatialBlend = SpacialBlend;

        audioSrc17_ChandlierSquek.loop = true;
        audioSrc17_ChandlierSquek.spatialBlend = SpacialBlend;

        

        audioSrc5_Ambience.loop = true;

    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
    }

    public void stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }
}
