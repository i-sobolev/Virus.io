using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodiesAddEffect : MonoBehaviour
{
    public List<Tail> path;
    private Vector2 _moveTarget;
    private int _bodyNumber = 0;
    private float _lerp = 0;

    private void OnEnable()
    {
        ChangeTarget();
    }

    private void Update()
    {
        float distanceToTarget = Vector2.Distance(gameObject.transform.position, _moveTarget);
        gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, _moveTarget, _lerp * Time.deltaTime);
        _lerp += 0.02f;

        if (_lerp > 1)
        {
            ChangeTarget();
            _lerp = 0;
        }

        if (_bodyNumber > path.Count - 2)
            Destroy(gameObject);

    }

    private void ChangeTarget()
    {
        _moveTarget = path[_bodyNumber + 1].transform.position;
        _bodyNumber++;
    }
}
