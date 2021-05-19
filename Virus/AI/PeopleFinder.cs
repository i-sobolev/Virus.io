using System.Collections.Generic;
using UnityEngine;

public class PeopleFinder : MonoBehaviour
{
    public List<Transform> _peopleInArea;
    private Transform _parentTransform;
    private Transform _currentTarget;

    public void SetParent(Transform parent)
    {
        _parentTransform = parent;
    }

    private void FixedUpdate()
    {
        transform.position = _parentTransform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_peopleInArea.Count <= 6)
        {
            if (collision.CompareTag("People"))
                _peopleInArea.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("People"))
            _peopleInArea.Remove(collision.transform);
    }

    public Vector2 FindNearestObject()
    {
        Vector2 objPos = new Vector2(Random.Range(-30f, 30f), Random.Range(-30f, 30f));

        float minDist = Mathf.Infinity;

        for (int i = _peopleInArea.Count - 1; i >= 0; i--)
        {
            if (_peopleInArea[i] == null)
            {
                _peopleInArea.Remove(_peopleInArea[i]);
                i -= 1;
            }

            float dist = Vector2.Distance(_peopleInArea[i].position, transform.position);

            if (dist < minDist)
            {
                objPos = _peopleInArea[i].position;
                _currentTarget = _peopleInArea[i];
                minDist = dist;
            }
        }
    
        return objPos;
    }

    public Vector2 FindNewObject()
    {
        _peopleInArea.Remove(_currentTarget);
        return FindNearestObject();
    }
}
