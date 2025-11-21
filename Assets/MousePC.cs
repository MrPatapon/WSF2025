using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePC : MonoBehaviour
{
    public List<Button> bs;

    // Update is called once per frame
    void Update()
    {
        foreach(Button b in bs)
        {
            
            if (Mathf.Abs((b.GetComponent<RectTransform>().anchoredPosition - GetComponent<RectTransform>().anchoredPosition).magnitude) < 50.0)
            {
                if ((Input.GetMouseButtonDown(0))||(Input.GetMouseButtonDown(1)))
                {
                    b.onClick.Invoke();

                }
                else
                {
                    ColorBlock cb = b.colors;
                    cb.normalColor = Color.white;
                    b.colors = cb;
                }
                
            }
            else
            {
                ColorBlock cb = b.colors;
                cb.normalColor = Color.gray;
                b.colors = cb;
            }

            
        }
    }
}
