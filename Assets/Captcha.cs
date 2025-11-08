using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Captcha : MonoBehaviour
{
    public TMPro.TMP_Text label;
    public TMPro.TMP_Text user_label;
    string s="";
    public float t = 0.0f;
    public List<KeyCode> free_letters;
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

        String upp_tex = label.text.ToUpper();
        free_letters = new List<KeyCode>();

        for (int i = 0; i < 26; i++)
        {
            KeyCode c = KeyCode.A + i;
            if (!upp_tex.Contains((c.ToString()))){
                free_letters.Add(c);
            }
        }
        Debug.Log(free_letters);
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape) ){
            SceneManager.LoadScene(0);
        }





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
            if (s.Length > 0)
            {
                if (s[s.Length-1]!=' ')
                {
                    s += " ";
                    need_up = true;
                }
            }
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
