using UnityEngine;

public class camerazoom : MonoBehaviour
{
    [SerializeField] GameObject plr1;
    [SerializeField] GameObject plr2;

    [Header("Camera Settings")]
    [SerializeField] Camera cam;
    [SerializeField] float minZoom = 5f;
    [SerializeField] float maxZoom = 15f;
    [SerializeField] float zoomPadding = 3f;
    [SerializeField] float smoothSpeed = 5f;

    [Header("Camera Offset")]
    [SerializeField] Vector3 cameraOffset = new Vector3(0f, 2f, -10f);

    void Start()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (plr1 == null || plr2 == null || cam == null)
            return;

        // Find the midpoint between players
        // Find midpoint between players on X and Y
        Vector3 midpoint = (plr1.transform.position + plr2.transform.position) / 2f;

        // Keep camera at its original Z position
        Vector3 targetPosition = new Vector3(
            midpoint.x + cameraOffset.x,
            midpoint.y + cameraOffset.y,
            cameraOffset.z
        );

        // Smooth camera movement
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );


        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        // Calculate distance between players
        Vector3 difference = plr1.transform.position - plr2.transform.position;

        float distanceX = Mathf.Abs(difference.x);
        float distanceY = Mathf.Abs(difference.y);

        // Calculate required zoom based on distance and aspect ratio
        float requiredZoomX = distanceX / (2f * cam.aspect);
        float requiredZoomY = distanceY / 2f;

        float targetZoom = Mathf.Max(requiredZoomX, requiredZoomY) + zoomPadding;

        // Clamp zoom
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        // Smooth zoom
        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            targetZoom,
            smoothSpeed * Time.deltaTime
        );
    }
}
