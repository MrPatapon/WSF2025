using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameType { 
    None,
    Capcha,
    Excel,

};


public class OfficeTaskManager : MonoBehaviour
{
    public List<GameType> gt;
    public Excel excel;
    public Captcha captcha;
    public int id=0;
    public int dayScore=0;


    public GameType gtc;
    void Start()
    {
        excel.otm = this;
        captcha.otm = this;
        gtc = gt[0];
        captcha.gen();
        excel.new_excel();
        if ( gtc == GameType.Capcha )
        {
            captcha.gameObject.active = true;
            
        }
        if (gtc == GameType.Excel)
        {
            excel.gameObject.active = true;
        }
    }

    public void finish(int score)
    {
        dayScore += score;
        Debug.Log("you have "+ dayScore.ToString());
        id = (id + 1) % gt.Count;
        gtc = gt[id];
        if (gtc == GameType.Capcha)
        {
            captcha.gameObject.active = true;
            excel.gameObject.active = false;
        }
        if (gtc == GameType.Excel)
        {
            excel.gameObject.active = true;
            captcha.gameObject.active = false;
        }
    }

    public List<KeyCode> free_letters()
    {
        String full= captcha.label.text.ToUpper();
        for(int i=0; i<excel.l.Count; i++)
        {
            full += excel.l[i][1].ToUpper();
        }

        //if (gtc == GameType.Capcha)
        //{
        //    return captcha.free_letters;
        /// }
        List<KeyCode>  free_letters = new List<KeyCode>();

        for (int i = 0; i < 26; i++)
        {
            KeyCode c = KeyCode.A + i;
            if (!full.Contains((c.ToString())))
            {
                free_letters.Add(c);
            }
        }
        Debug.Log(free_letters);

        return free_letters;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
