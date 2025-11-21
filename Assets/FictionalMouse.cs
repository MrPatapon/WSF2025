using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.UI;

public class FictionalMouse : MonoBehaviour
{
    public Camera camera3d;
    public MeshRenderer screen;
    public RenderTexture rt;
    //debug monitor
    public Vector2 sp;
    public bool on;
    //2d
    public Camera camer2d;
    public Canvas canvas;
    public Image img;

    public float mscale=1.0f;
    // Update is called once per frame
    void Update()
    {
        Ray r = camera3d.ScreenPointToRay(Input.mousePosition);
        float time = (screen.transform.position.z - r.origin.z) / r.direction.z;
        Vector3 globalPosition = r.origin + time * r.direction - screen.transform.position;
        Vector2 localPosition = new Vector2(globalPosition.x / screen.transform.lossyScale.x,
                                            globalPosition.y / screen.transform.lossyScale.y);
        Vector2 scacePos = new Vector2(rt.width * (localPosition.x + 0.5f), rt.height * (localPosition.y + 0.5f));
        sp = scacePos;
        on = (Mathf.Abs(localPosition.x) < 0.5)&& (Mathf.Abs(localPosition.y) < 0.5);
        Vector2 fp = new Vector2(rt.width , rt.height);
        UpdateMouse(on,sp,fp);
    }
    void UpdateMouse(bool on,Vector2 sp, Vector2 fp)
    {
        //img.gameObject.active = on;
        float isf = 1.0f / canvas.scaleFactor;
        img.GetComponent<RectTransform>().anchoredPosition = (sp - fp * 0.5f) * mscale;
        //img.GetComponent<RectTransform>().
    }
}
