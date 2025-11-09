using System;
using System.Collections.Generic;
using UnityEngine; 

public class Excel : MonoBehaviour
{
    public List<excell_line> data;
    public List<KeyCode> free_letters;
    public QTManager qtm;
    public OfficeTaskManager otm;

    public List<String[]> l;
    public int line = 0;
    public float t = 0.0f;
    public String s = "";
    public int exc_id=0;
    public GameObject deco_obj;
    public TMPro.TMP_Text info;

    void gen_q()
    {
        List<String[]> namesAll = new List<String[]>{
            new String[2]{"Jan","Kowalski"},
            new String[2]{"Janusz","Kowalski"},
            new String[2]{ "Krzysztof", "Kopa" },
            new String[2]{ "Konrad", "Kopa" },
            new String[2]{ "Dominik", "Rudy" },
            new String[2]{ "Damian", "Rudy" },
            new String[2]{ "Ola", "Nowak" },
            new String[2]{ "Urszula", "Nowak" },
            new String[2]{ "Ignacy", "Opolski" },
            new String[2]{ "Konrad", "Opolski" },
            new String[2]{ "Dobromir", "Zasada" },
            new String[2]{ "Urszula", "Zasada" },
            new String[2]{ "Magda", "Mazur" },
            new String[2]{ "Krzysztof", "Mazur" }
        };

        List<String[]> namesOk = new();
        foreach (String[] name in namesAll)
        {
            bool ok = true;
            if (qtm != null)
            {
                foreach (KeyCode l in qtm.letters_in_use)
                {
                    if (name[1].Contains(l.ToString()))
                    {
                        ok = false;
                    }
                }
            } 
            if (ok)
            {
                namesOk.Add(name);
            }
        }
        l = new();
        for(int i = 0;i < data.Count; i++)
        {
            l.Add(namesOk[UnityEngine.Random.Range(0, namesOk.Count)]);
        }

    }

    public void new_excel()
    {
        exc_id += 1;
        gen_q();
        String deb = "";
        foreach(String[] p in l){
            //Debug.Log(p[0] + " " + p[1]);
            deb += p[0] + " " + p[1] + "\n";
        }
        info.text = deb;
        line = 0;
        for (int i = 0; i < data.Count; i++)
        {
            data[i].cellA.text = l[i][0];
            data[i].cellB.text = "";//l[i][1];
        }
        if (exc_id % 2 == 0)
        {
            deco_obj.active = true;
        }
        else
        {
            deco_obj.active = false;
        }
    }

    void Start()
    {
        new_excel();
    }

    void Update()
    {
        t += Time.deltaTime;
        bool need_up = false;
        for (int i = 0; i < 26; i++)
        {
            KeyCode key = KeyCode.A + i;
            if (qtm != null)
            {
                if (qtm.letters_in_use.Contains(key))
                {
                    continue;
                }
            }
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

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            s = "";
            need_up = true;
        }

        

        if (need_up)
        {
            data[line].cellB.text = s;
            if ((data[line].cellB.text == l[line][1])||(Input.GetKey(KeyCode.Minus)))
            {
                line += 1;
                s = "";
            }
            if (line == data.Count)
            {
                

                int score = 0;
                foreach (String[] p in l)
                {
                    score += p[1].Length;
                }
                new_excel();
                otm.finish(score);
            }
        }


        if (((int)t) % 2 == 1)
        {
            data[line].cellB.text = s;
        }
        else
        {
            data[line].cellB.text = s + "|";
        }
    }
}
