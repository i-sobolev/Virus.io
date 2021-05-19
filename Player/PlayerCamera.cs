using System.Collections;
using UnityEngine;
using VirusAnimator;

public class PlayerCamera : MonoBehaviour
{
    private GameObject _moveTarget;
    private Camera _camera;

    public static PlayerCamera Singletone { get; private set; }

    private void Awake()
    {
        Singletone = this;
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        _moveTarget = PlayerVirusHead.Singletone.gameObject;
        PlayerVirusHead.Singletone.OnVirusGrow += AddSize;
    }

    void Update()
    {
        if (_moveTarget != null)
            transform.position = new Vector3(_moveTarget.transform.position.x, _moveTarget.transform.position.y, transform.position.z);
    }

    public void AddSize()
    {
        if (_camera.orthographicSize < 11)
            StartCoroutine(SmoothAddSize());
    }

    public void SetMoveTarget(GameObject gameObject) => _moveTarget = gameObject;
    
    private IEnumerator SmoothAddSize()
    {
        float targetSize = _camera.orthographicSize + 0.5f;
        float step = 0;

        do
        {
            _camera.orthographicSize += Twiner.SmoothSquarer(step);
            step += Time.deltaTime;
            yield return null;
        } 
        while (_camera.orthographicSize < targetSize);

        _camera.orthographicSize = targetSize;
    }
}