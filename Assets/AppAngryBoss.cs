using UnityEngine;

public class AppAngryBoss : MonoBehaviour
{
    public float time = 1.0f;
    public OfficeTaskManager otm;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time-=Time.deltaTime;
        if(time < 0.0f)
        {
            otm.finish(0);
            time = 1.0f;
            this.gameObject.active=false;
        }
    }
}
