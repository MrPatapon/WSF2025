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
    public EventInstance footstepEventInstance;

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


    public void InitializeVanity(EventReference vanityEventReference)
    {
        musicVanityEventInstance = CreateInstance(vanityEventReference);
        musicVanityEventInstance.start();
        SetVanityArea(1, 0, 0);
    }
    public void InitializeAnger(EventReference angerEventReference)
    {
        musicAngerEventInstance = CreateInstance(angerEventReference);
        musicAngerEventInstance.start();
        SetAngerArea(1, 0, 0);
    }
    public void InitializeFarewell(EventReference farewellEventReference)
    {
        musicFarewellEventInstance = CreateInstance(farewellEventReference);
        musicFarewellEventInstance.start();
        SetFarewellArea(1, 0, 0);
    }
    public void InitializeMadness(EventReference madnessEventReference)
    {
        musicMadnessEventInstance = CreateInstance(madnessEventReference);
        musicMadnessEventInstance.start();
        SetMadnessArea(1, 0, 0);
    }

    public void InitializeMysterious(EventReference mysteriousEventReference)
    {
        musicMysteriousEventInstance = CreateInstance(mysteriousEventReference);
        musicMysteriousEventInstance.start();
        SetMysteriousArea(1, 0, 0);
    }
    public void InitializeAdventure(EventReference adventureEventReference)
    {
        musicAdventureEventInstance = CreateInstance(adventureEventReference);
        musicAdventureEventInstance.start();
        SetAdventureArea(0, 0, 0);
    }

    public void InitializePeaceful(EventReference peacefulEventReference)
    {
        musicPeacefulEventInstance = CreateInstance(peacefulEventReference);
        musicPeacefulEventInstance.start();
        SetPeacefulArea(1, 0, 0);
    }

    public void InitializeFootsteps(EventReference foostepsEventReference)
    {
        footstepEventInstance = CreateInstance(foostepsEventReference);
        footstepEventInstance.start();
    }

    public void InicjalizacjadlaJu(EventReference muzyka)
    {
        isttrack = CreateInstance(muzyka);
        // jeœli chcesz odpaliæ na starcie: isttrack.start()
    }



    public void SetAdventureArea(float ov, float scene, float lab)
    {

        musicAdventureEventInstance.setParameterByName("Main", (float)ov);
        musicAdventureEventInstance.setParameterByName("Memory", (float)scene);
        musicAdventureEventInstance.setParameterByName("Portalab", (float)lab);
    }
    public void SetMadnessArea(float ov, float scene, float lab)
    {

        musicMadnessEventInstance.setParameterByName("Main", (float)ov);
        musicMadnessEventInstance.setParameterByName("Memory", (float)scene);
        musicMadnessEventInstance.setParameterByName("Portalab", (float)lab);
    }
    public void SetFarewellArea(float ov, float scene, float lab)
    {

        musicFarewellEventInstance.setParameterByName("Main", (float)ov);
        musicFarewellEventInstance.setParameterByName("Memory", (float)scene);
        musicFarewellEventInstance.setParameterByName("Portalab", (float)lab);
    }

    public void SetAngerArea(float ov, float scene, float lab)
    {

        musicAngerEventInstance.setParameterByName("Main", (float)ov);
        musicAngerEventInstance.setParameterByName("Memory", (float)scene);
        musicAngerEventInstance.setParameterByName("Portalab", (float)lab);
    }

    public void SetPeacefulArea(float ov, float scene, float lab)
    {

        musicPeacefulEventInstance.setParameterByName("Main", (float)ov);
        musicPeacefulEventInstance.setParameterByName("Memory", (float)scene);
        musicPeacefulEventInstance.setParameterByName("Portalab", (float)lab);
    }

    public void SetVanityArea(float ov,float scene,float lab)
    {
        musicVanityEventInstance.setParameterByName("Main",(float)ov);
        musicVanityEventInstance.setParameterByName("Memory",(float)scene);
        musicVanityEventInstance.setParameterByName("Portalab",(float)lab);
    }
    public void SetMysteriousArea(float ov, float scene, float lab)
    {
        musicMysteriousEventInstance.setParameterByName("Main", (float)ov);
        musicMysteriousEventInstance.setParameterByName("Memory", (float)scene);
        musicMysteriousEventInstance.setParameterByName("Portalab", (float)lab);
    }



    public void SetFootstepsArea(int area)
    {
        footstepEventInstance.setParameterByName("footsteps", (int)area);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void Stopplay(EventReference sound)
    {

    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
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
