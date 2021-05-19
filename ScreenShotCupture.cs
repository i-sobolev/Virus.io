using UnityEngine;

public class ScreenShotCupture : MonoBehaviour
{
    private Camera _camera;

    private static int _counter = 1;

    private void Start()
    {
        _camera = PlayerCamera.Singletone.gameObject.GetComponent<Camera>();
        InvokeRepeating(nameof(Capture), 5f, 5f);
    }

    private void Capture()
    {
        ScreenCapture.CaptureScreenshot("Assets/Screenshots/Sreenshot" + _counter.ToString("00") + "_" + _camera.pixelWidth + "x" + _camera.pixelHeight + ".png");
        _counter++;
    }
}