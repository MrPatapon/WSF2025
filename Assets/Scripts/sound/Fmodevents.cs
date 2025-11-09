using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;

public class FmodEvents : MonoBehaviour
{
    //ambience

    [field: Header("Ambience & Music")]
     public EventReference Ambience;
     public EventReference Music;



    [field: Header("Vaping & Coughing")]
    [field: SerializeField] public EventReference Inhale { get; private set; }
    [field: SerializeField] public EventReference Exhale { get; private set; }
    [field: SerializeField] public EventReference SmallCough { get; private set; }
    [field: SerializeField] public EventReference MediumCough { get; private set; }
    [field: SerializeField] public EventReference LargeCough { get; private set; }

     


    [field: Header("MiniGames")]
    [field: SerializeField] public EventReference KeyboardTyping { get; private set; }
    [field: SerializeField] public EventReference KeyboardButton { get; private set; }
    [field: SerializeField] public EventReference SceneSound { get; private set; }



    [field: Header("Boss")]
    [field: SerializeField] public EventReference BossAlert { get; private set; }
    [field: SerializeField] public EventReference BossTriggered { get; private set; }
    [field: SerializeField] public EventReference BossRun { get; private set; }


    [field: Header("Sound Cues")]
    [field: SerializeField] public EventReference Success { get; private set; }
    [field: SerializeField] public EventReference Fail { get; private set; }
    [field: SerializeField] public EventReference PlayerScream { get; private set; }
    [field: SerializeField] public EventReference EndDayPhone { get; private set; }

    [field: SerializeField] public EventReference CityAmbientBreak { get; private set; }


    [field: Header("UI")]
    [field: SerializeField] public EventReference Click { get; private set; }
    [field: SerializeField] public EventReference Hover { get; private set; }

    public static FmodEvents instance { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}

