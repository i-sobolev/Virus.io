using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeaderIndicator : MonoBehaviour
{
    [Header("UI")]
    public Text leaderNicname;
    public Image icon;
    public GameObject arrow;

    private float _width, _height;
    private float _borderOffset;
    private Vector2 indicatorOffset = new Vector2(0, 100f);
    private RectTransform _transform;
    private Camera _camera;
    private GameObject _playerGameObject;
    private UIRateTable _rateTable;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
        _width = Screen.width;
        _height = Screen.height;

        _borderOffset = _height < _width ? _height * 0.15f : _width * 0.15f;
    }

    private void Start()
    {
        _camera = PlayerCamera.Singletone.gameObject.GetComponent<Camera>();
        _rateTable = UIRateTable.Singletone;
        _playerGameObject = PlayerVirusHead.Singletone.gameObject;

        _playerGameObject = GameObject.Find(TemporaryData.PlayerNickName);
        StartCoroutine(EnableDelay());
    }

    private void Update()
    {
        GameObject leader = _rateTable.playersList[0].gameObject;
        Vector2 projectedVector = _camera.WorldToScreenPoint(leader.transform.position);

        float x = projectedVector.x;
        float y = projectedVector.y;

        if (x > _width - _borderOffset)
            x = _width - _borderOffset;

        if (x < _borderOffset)
            x = _borderOffset;

        if (y > _height - _borderOffset)
            y = _height - _borderOffset;

        if (y < _borderOffset)
            y = _borderOffset;

        _transform.anchoredPosition = new Vector2(x, y);

        if (y < _height - _borderOffset && y > _borderOffset && x < _width - _borderOffset && x > _borderOffset)
        {
            _transform.anchoredPosition += indicatorOffset;
            arrow.SetActive(true);
        }

        if (leader.name == TemporaryData.PlayerNickName)
            arrow.SetActive(false);

        leaderNicname.text = leader.name;

        if (_playerGameObject != null)
            arrow.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, (transform.position - _playerGameObject.transform.position).normalized));
        
        else
            Destroy(gameObject);
    }

    private IEnumerator EnableDelay()
    {
        leaderNicname.enabled = false;
        icon.enabled = false;
        arrow.SetActive(false);

        yield return new WaitForSeconds(2f);

        leaderNicname.enabled = true;
        icon.enabled = true;
        arrow.SetActive(true);
    }
}
