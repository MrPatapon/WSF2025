using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameType { 
    None,
    Capcha,
    Excel,

};
[Serializable]
public class DayGames
{
    public List<GameType> gto;
    public float maxTime = 30.0f;
    public bool qt = true;
}


public class OfficeTaskManager : MonoBehaviour
{
    public List<DayGames> gto;
    public Excel excel;
    public Captcha captcha;
    public int id=0;
    public int day_id = 0;
    public int dayScore=0;
    public QTManager qtm;
    public keyManager keyManager;

    public GameType gtc;
    void Start()
    {
        excel.otm = this;
        captcha.otm = this;
        captcha.gen();
        excel.new_excel();
    }
    public void turn_off()
    {
        excel.gameObject.active = false;
        captcha.gameObject.active = false;
    }
    public void onSlow()
    {
        keyManager.bossRelation *= 0.5f;
        Debug.Log("YOU!!!!!!!!!!!!!!!!");
    }
    public void StartDay(int day04)
    {
        Debug.Log("STARTING >>>"+day04.ToString());
        day_id = day04;
        gtc = gto[day_id].gto[0];

        excel.MtaskTime = gto[day_id].maxTime;
        excel.taskTime = gto[day_id].maxTime;
        excel.live = true;

        if ( gtc == GameType.Capcha )
        {
            captcha.gameObject.active = true;
            excel.gameObject.active = false;
        }
        if (gtc == GameType.Excel)
        {
            excel.gameObject.active = true;
            captcha.gameObject.active = false;
        }
        if (gtc == GameType.None)
        {
            excel.gameObject.active = false;
            captcha.gameObject.active = false;
        }
        qtm.IsOn= gto[day_id].qt;
    }

    public int EndDay()
    {
        Debug.Log("DAY WIN >>>" + dayScore.ToString());
        int res = dayScore;
        qtm.IsOn = false;
        return dayScore;

    }

    public void finish(int score)
    {
        dayScore += score;
        Debug.Log("you have "+ dayScore.ToString());
        id = (id + 1) % gto[day_id].gto.Count;
        gtc = gto[day_id].gto[id];
        if (gtc == GameType.Capcha)
        {
            captcha.gameObject.active = true;
            excel.gameObject.active = false;
            captcha.MtaskTime = gto[day_id].maxTime;
            captcha.taskTime = gto[day_id].maxTime;
            captcha.live = true;
        }
        if (gtc == GameType.Excel)
        {
            excel.gameObject.active = true;
            captcha.gameObject.active = false;
            excel.MtaskTime = gto[day_id].maxTime;
            excel.taskTime = gto[day_id].maxTime;
            excel.live = true;
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
