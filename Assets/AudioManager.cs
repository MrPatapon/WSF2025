using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;
    public EventInstance music;
    public EventInstance ambient;


    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeAmbience(FMODEvents.instance.Ambience);
        InitializeMusic(FMODEvents.instance.Music);

        void InitializeAmbience(EventReference ambienceEventReference)
        {
            ambient = CreateInstance(ambienceEventReference);
            ambient.start();
        }
        void InitializeMusic(EventReference ambienceEventReference)
        {
            music = CreateInstance(ambienceEventReference);
            music.start();
        }

        EventInstance CreateInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstances.Add(eventInstance);
            return eventInstance;
        }
    }
}
