using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Captcha : MonoBehaviour
{
    public TMPro.TMP_Text label;
    public TMPro.TMP_Text user_label;
    string s="";
    public float t = 0.0f;
    int id = 0;
    void Start()
    {
        gen();
    }
    void gen()
    {
        List<String> a = new List<String> { "Ala ma kota", "Pies i kot", "Szef jest super" };
        label.text = a[id];
        id = (id + 1) % a.Count;
    }

    void Update()
    {
        t += Time.deltaTime;
        bool need_up = false;
        for (int i = 0; i < 26; i++)
        {
            KeyCode key = KeyCode.A + i;
            if (Input.GetKeyDown(key))
            {
                if (s.Length == 0)
                {
                    s += key.ToString();
                }
                else
                {
                    s += key.ToString().ToLower();
                }
                
                
                need_up = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            s += " ";
            need_up = true; 
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            s = "";
            need_up = true;
        }
        if (need_up)
        {
            if (s == label.text)
            {
                s = "";
                gen();
            }
        }
        if (((int)t) % 2 == 1)
        {
            user_label.text = s;
        }
        else
        {
            user_label.text = s+"|";
        }
    }
}
