using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FirstPersonZoom : MonoBehaviour
{
    [SerializeField] private Camera cameraToZoom;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;
    [SerializeField] private float timeToZoom = 0.1f;
    [SerializeField] private float zoomedFOV = 30f;
    private float defaultFOV;
    private bool isZooming = false;
    private bool isZoomIn = false;
    private CancellationTokenSource cancellationTokenSource;

    public void Initialize(Camera camera = null)
    {
        if (camera != null)
        {
            cameraToZoom = camera;
        }
        else if (cameraToZoom == null)
        {
            cameraToZoom = GetComponent<Camera>();
        }
        defaultFOV = cameraToZoom.fieldOfView;
    }
    /*
    private void Update()
    {
        if (Input.GetKeyDown(zoomKey))
        {
            if (isZooming)
            {
                cancellationTokenSource.Cancel();
            }
            isZoomIn = !isZoomIn;
            ZoomTask(isZoomIn);
        }
    }
    */

    public bool ZoomRequest()
    {
        return Input.GetKeyDown(zoomKey);
    }

    public bool ToggleZoom()
    {
        if (isZooming)
        {
            cancellationTokenSource.Cancel();
        }
        isZoomIn = !isZoomIn;
        ZoomTask(isZoomIn);
        return isZoomIn;
    }

    private async void ZoomTask(bool isZoomIn)
    {
        isZooming = true;
        cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        float targetFOV = isZoomIn ? zoomedFOV : defaultFOV;
        float startFOV = cameraToZoom.fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < timeToZoom)
        {
            if (token.IsCancellationRequested)
            {
                break;
            }

            elapsedTime += Time.deltaTime;
            cameraToZoom.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime / timeToZoom);
            await Task.Yield();
        }

        cameraToZoom.fieldOfView = targetFOV;
        isZooming = false;
    }
}
