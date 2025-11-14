using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 0.8f;

    [Header("FootstepsVolume")]
    [Range(0, 1)]
    public float footstepsVolume = 0.5f;


    private Bus masterBus;

    private List<FMOD.Studio.EventInstance> musicInstances = new List<FMOD.Studio.EventInstance>();
    private FMOD.Studio.EventInstance activeEvent;

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    private EventInstance ambienceEventInstance;
    public EventInstance musicEventInstance;

    public EventInstance isttrack;

    public EventInstance musicVanityEventInstance;
    private EventInstance musicAdventureEventInstance;
    private EventInstance musicMysteriousEventInstance;
    private EventInstance musicPeacefulEventInstance;
    private EventInstance musicMadnessEventInstance;
    private EventInstance musicAngerEventInstance;
    private EventInstance musicFarewellEventInstance;


    public static AudioManager instance { get; private set; }

    private void Awake()
    {
       

        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene. " + instance + " will be replaced by " + this);
        }
        instance = this;

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");

    }

    private void Start()
    {
        InitializeAmbience(FmodEvents.instance.Ambience);
        //InitializeMusic(FmodEvents.instance.Music);
        InitializeAmbience(FmodEvents.instance.Ambience);
        //InitializeMusic(FmodEvents.instance.Music);


        musicInstances.Add(musicMysteriousEventInstance);
        musicInstances.Add(musicAdventureEventInstance);
        musicInstances.Add(musicVanityEventInstance);
        musicInstances.Add(musicPeacefulEventInstance);
        musicInstances.Add(musicMadnessEventInstance);
        musicInstances.Add(musicAngerEventInstance);
        musicInstances.Add(musicFarewellEventInstance);
        // InicjalizacjadlaJu(FmodEvents.instance./*pierwszy track*/);

    }
    public void UpdateActiveTrack()
    {
        foreach (var eventInstance in eventInstances)
        {
            float value;
            eventInstance.getParameterByName("Main", out value);
            if (value == 1)
            {
                activeEvent = eventInstance;
                break;
            }
        }
    }
    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }
    private void InitializeMusic(EventReference ambienceEventReference)
    {
        musicEventInstance = CreateInstance(ambienceEventReference);
        musicEventInstance.start();
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
    public void PlayOneShotWithVolume(EventReference sound, Vector3 position, float volume)
    {
        // Create instance manually (instead of PlayOneShot)
        EventInstance instance = RuntimeManager.CreateInstance(sound);

        // Position
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));

        // Clamp volume 0..1 for safety
        volume = Mathf.Clamp01(volume);
        instance.setVolume(volume);

        // Play and release
        instance.start();
        instance.release(); // safe cleanup
    }

    public void Stopplay(EventReference sound)
    {

    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        //eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        masterBus.setVolume(volume);
    }

    private void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        // stop all of the event emitters, because if we don't they may hang around in other scenes
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
    public void ChangeActiveTrackParameters(float mainValue, float portalabValue, float memoryValue)
    {
        activeEvent.setParameterByName("Main", mainValue);
        activeEvent.setParameterByName("Portalab", portalabValue);
        activeEvent.setParameterByName("Memory", memoryValue);
    }
}
