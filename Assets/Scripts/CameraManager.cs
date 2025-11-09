using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraManager: MonoBehaviour
{
    [Header("3D Objects")]
    [SerializeField] private GameObject leftObject;
    [SerializeField] private GameObject rightObject;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationDuration = 1f;   // Time to rotate

    [Header("Default Rotation")]
    [SerializeField] private Vector3 defaultRotation = new Vector3(15f, 0f, 0f);

    [Header("Hover Rotations")]
    [SerializeField] private Vector3 leftRotation = new Vector3(0f, -45f, 0f);
    [SerializeField] private Vector3 rightRotation = new Vector3(0f, 45f, 0f);

    [Header("QuadTextureSetup")]
    [SerializeField] private Camera mainCamera;   // your main scene camera
    [SerializeField] private Camera uiCamera;     // the camera rendering UI to RenderTexture
    [SerializeField] private MeshRenderer targetQuad; // the quad that displays the RenderTexture
    [SerializeField] private EventSystem eventSystem; // the EventSystem in your UI scene
    [SerializeField] private GraphicRaycaster uiRaycaster; // from your Canvas

    private Coroutine rotationRoutine;

    private bool isOverLeft = false;
    private bool isOverRight = false;

    void Start()
    {
        
        transform.rotation = Quaternion.Euler(defaultRotation); // Set starting rotation
    }

    void Update()
    {
        isOverLeft = IsMouseOverObject(leftObject);
        isOverRight = IsMouseOverObject(rightObject);
        QuadCamFunc();

        UpdateRotation();
    }

    private bool IsMouseOverObject(GameObject obj)
    {
        if (obj == null) return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == obj)
                return true;
        }

        return false;
    }

    private void UpdateRotation()
    {
        Vector3 targetRot;

        if (isOverLeft && !isOverRight) targetRot = leftRotation;
        else if (isOverRight && !isOverLeft) targetRot = rightRotation;
        else targetRot = defaultRotation;

        if (rotationRoutine != null) StopCoroutine(rotationRoutine);
        rotationRoutine = StartCoroutine(RotateToRotation(targetRot));
    }

    private IEnumerator RotateToRotation(Vector3 targetEuler)
    {
        float elapsed = 0f;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(targetEuler);

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            transform.rotation = Quaternion.Lerp(startRot, targetRot, smoothT);
            yield return null;
        }

        transform.rotation = targetRot;
        rotationRoutine = null;
    }
    private void QuadCamFunc()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");

                if (hit.collider.gameObject == targetQuad.gameObject)
                {
                    Debug.Log("Quad clicked!");

                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.y = 1f - pixelUV.y; // flip if needed for RenderTexture orientation

                    // Check UV values
                    Debug.Log($"UV coords: {pixelUV}");

                    // Convert to RenderTexture pixel space
                    RenderTexture rt = uiCamera.targetTexture;
                    Vector2 renderTexPos = new Vector2(
                        pixelUV.x * rt.width,
                        pixelUV.y * rt.height
                    );

                    // Convert to screen position relative to the UI Camera
                    Vector2 screenPos = new Vector2(
                        renderTexPos.x / rt.width * uiCamera.pixelWidth,
                        renderTexPos.y / rt.height * uiCamera.pixelHeight
                    );

                    Debug.Log($"Converted screenPos: {screenPos}");

                    // Create pointer event for UI raycast
                    PointerEventData pointerData = new PointerEventData(eventSystem)
                    {
                        position = screenPos
                    };

                    var results = new System.Collections.Generic.List<RaycastResult>();
                    uiRaycaster.Raycast(pointerData, results);

                    Debug.Log($"UI Raycast results: {results.Count}");

                    foreach (var result in results)
                    {
                        Debug.Log($"Clicked on UI element: {result.gameObject.name}");
                        ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
                    }

                    if (results.Count == 0)
                    {
                        Debug.LogWarning("No UI elements hit — check Canvas render mode, raycaster setup, or UV flip.");
                    }
                }
                else
                {
                    Debug.Log("Raycast hit something else, not the target quad.");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }
    }
}