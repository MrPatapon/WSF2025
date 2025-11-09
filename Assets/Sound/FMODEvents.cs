using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;
public class FMODEvents : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [field: Header("Ambience & Music")]
    public EventReference Ambience;
    public EventReference Music;

    [field: Header("BOSS")]
    [field: SerializeField] public EventReference Kroki { get; private set; }
    [field: SerializeField] public EventReference Wkurwienie { get; private set; }

    [field: Header("Vape")]
    [field: SerializeField] public EventReference Kaszel1 { get; private set; }
    [field: SerializeField] public EventReference Kaszel2 { get; private set; }

    [field: SerializeField] public EventReference suckin { get; private set; }
    [field: SerializeField] public EventReference suckout { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}
