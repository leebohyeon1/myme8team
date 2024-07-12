using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.3f;
    public GameObject background;

    private Vector3 velocity = Vector3.zero;
    private Vector3 minCameraPos, maxCameraPos;

    void Start()
    {
        CalculateBounds();
    }

    void LateUpdate()
    {
        Vector3 targetPosition = player.position;
        targetPosition.z = transform.position.z; // 카메라의 z 위치 유지

        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // 카메라 위치 제한
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minCameraPos.x, maxCameraPos.x);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minCameraPos.y, maxCameraPos.y);

        transform.position = smoothedPosition;
    }

    void CalculateBounds()
    {
        Bounds bounds = background.GetComponent<Renderer>().bounds;
        Camera cam = Camera.main;

        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        minCameraPos.x = bounds.min.x + horzExtent;
        maxCameraPos.x = bounds.max.x - horzExtent;
        minCameraPos.y = bounds.min.y + vertExtent;
        maxCameraPos.y = bounds.max.y - vertExtent;
    }
}