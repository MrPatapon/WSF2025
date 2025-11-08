using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QT_element : MonoBehaviour
{
    public Image keyImage;
    public Image trailImage;
    public TMPro.TMP_Text letter;
    float timeHP = 0.5f;
    float mtimeHP = 0.5f;
    public KeyCode my_key;
    public bool moving = false;
    public QTManager qtManager;
    void Start()
    {
        keyImage.gameObject.active = false;
        trailImage.gameObject.active = false;
    }

    public void run(KeyCode key, float lifeTime)
    {
        keyImage.gameObject.active = true;
        trailImage.gameObject.active = true;
        my_key = key;
        letter.text = key.ToString();
        timeHP = lifeTime;
        mtimeHP = lifeTime;
        moving = false;
        keyImage.color = Color.white;
        keyImage.rectTransform.anchoredPosition = new Vector2(keyImage.rectTransform.anchoredPosition.x,0.0f);
    }

    void Update()
    {

        if (keyImage.gameObject.active == true)
        {
            
            if (Input.GetKey(my_key))
            {
                if (!moving)
                {
                    qtManager.onPressed.Invoke();
                }
                moving = true;
                keyImage.rectTransform.anchoredPosition = keyImage.rectTransform.anchoredPosition +
                    Vector2.down * 10.0f * Time.deltaTime;
                if (keyImage.rectTransform.anchoredPosition.y < -200.0f)
                {
                    keyImage.gameObject.active = false;
                    trailImage.gameObject.active = false;
                    qtManager.onFinish.Invoke();
                }
            }


            else
            {
                keyImage.color = keyImage.color + (Color.red - keyImage.color) * (Time.deltaTime/ mtimeHP);
                timeHP -= Time.deltaTime;
                if ((timeHP < 0.0) || (moving))
                {
                    keyImage.gameObject.active = false;
                    trailImage.gameObject.active = false;
                    qtManager.onFail.Invoke();
                }
            }
        }
    }
}
