using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class QTManager : MonoBehaviour
{
    public List<KeyCode> letters_in_use = new();
    public List<QT_element> qts;
    public Captcha cap;
    public float liveTime = 6.0f;

    public UnityEvent onPressed;
    public UnityEvent onFinish;
    public UnityEvent onFail;
    public bool autoT=false;

    public float Stime = 3.0f;
    KeyCode new_letter()
    {
        KeyCode l = KeyCode.A;
        if (cap != null)
        {
            l = cap.free_letters[Random.Range(0, cap.free_letters.Count)];
        }
        else
        {
            l = l + Random.Range(0, 5);
        }

        return l;

    }
    int id = 0;
    void Start()
    {
        onPressed.AddListener(() =>
        {
            Debug.Log("PRESSED");
            updateLetter();
        });
        onFinish.AddListener(() =>
        {
            Debug.Log("FIN");
            updateLetter();
        });
        onFail.AddListener(() =>
        {
            Debug.Log("OOO");
            updateLetter();
        });
    }
    void updateLetter()
    {
        letters_in_use = new();
        foreach(QT_element qt in qts)
        {
            if (qt.keyImage.gameObject.active)
            {
                letters_in_use.Add(qt.my_key);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        Stime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Tab) || (autoT && Stime<0.0))
        {
            Stime = 3.0f;
            if (!qts[id].keyImage.gameObject.active)
            {
                Debug.Log("DDDDDDDDDDDdd");
                KeyCode key = new_letter();
                //letters_in_use.Add(key);
                updateLetter();
                qts[id].run(key, liveTime);
                id = (id + 1) % qts.Count;
            }
        }
    }

    


}
