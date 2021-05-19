using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    private Vector2 _touchPoint1;
    private Vector2 _touchPoint2;
    private bool _firstTouched = false;

    public Vector2 MoveDirection { get; private set; }
    public float TouchDistance { get; private set; }

    public UnityEvent doubleTapStart;
    public UnityEvent doubleTapEnd;

    public static PlayerInput Singletone { get; private set; }

    private void Awake() => Singletone = this;

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && touch.fingerId == 0)
            {
                _touchPoint1 = new Vector2(touch.position.x, touch.position.y);

                if (!_firstTouched)
                    StartCoroutine(DoubleTapCooldown());

                else
                    doubleTapStart?.Invoke();
            }

            if (touch.phase == TouchPhase.Ended && touch.fingerId == 0)
            {
                doubleTapEnd.Invoke();
            }
                

            if (touch.phase == TouchPhase.Moved && touch.fingerId == 0)
            {
                _touchPoint2 = touch.position;
                MoveDirection = _touchPoint2 - _touchPoint1;
                MoveDirection.Normalize();

                TouchDistance = Vector2.Distance(_touchPoint1, _touchPoint2) / 250;

                if (TouchDistance > 1)
                    TouchDistance = 1;
            }
        }
    }

    private IEnumerator DoubleTapCooldown()
    {
        _firstTouched = true;
        yield return new WaitForSeconds(0.4f);
        _firstTouched = false;
    }
}