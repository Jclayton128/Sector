using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RingDriver : MonoBehaviour
{


    //settings
    [SerializeField] float _radius = 0.4f;
    [SerializeField] Vector2[] _unitPoints = new Vector2[10];
    [SerializeField] float _spinRate = 10f;
    [SerializeField] Sprite _filledSprite = null;
    [SerializeField] Sprite _emptySprite = null;

    //state
    [SerializeField] List<SpriteRenderer> _shipSpots;

    [ContextMenu("Set Ship Spots")]
    public void SetSpots(int fillCount)
    {
        for (int i = 0; i < _shipSpots.Count; i++)
        {
            if (i < fillCount)
            {
                _shipSpots[i].sprite = _filledSprite;
            }
            else
            {
                _shipSpots[i].sprite = _emptySprite;
            }

                _shipSpots[i].transform.position = _unitPoints[i] * _radius;
            _shipSpots[i].transform.rotation = Quaternion.Euler(0,0, i * (Mathf.PI / 5f * Mathf.Rad2Deg));
        }

 /*       Debug.Log("setting ship spots");
*//*        if (_shipSpots != null && _shipSpots.Count > 0)
        {
            for (int i = _shipSpots.Count - 1; i >= 0; i--)
            {
                Destroy(_shipSpots[i].gameObject);
            }
        }
        _shipSpots.Clear();*//*

        _shipSpots = new List<SpriteRenderer>();
        float angDelta = (2*Mathf.PI / _numberOfShips);

        for (int i = 0; i < _numberOfShips; i++)
        {
            Vector2 pos = Vector2.zero;
 *//*           Debug.Log("ang:" + angDelta)*//*

            pos.x = Mathf.Cos((i * angDelta) * Mathf.Rad2Deg) * _radius;
            pos.y = Mathf.Sin((i * angDelta) * Mathf.Rad2Deg) * _radius;

            var ship = Instantiate(_shipSpotPrefab, pos, Quaternion.identity);
            _shipSpots.Add(ship);
            ship.transform.parent = transform;
        }*/
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, _spinRate * Time.deltaTime);
    }


}
