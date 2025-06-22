using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RingDriver : MonoBehaviour
{
    public enum RingTypes { City, Ship, Base}

    //settings
    [SerializeField] float _radius = 0.4f;
    [SerializeField] Vector2[] _unitPoints = new Vector2[10];
    [SerializeField] float _spinRate = 10f;
    [SerializeField] Sprite _filledSprite = null;
    [SerializeField] Sprite _emptySprite = null;
    [SerializeField] RingTypes _ringTypes = RingTypes.City;

    //state
    [SerializeField] List<SpriteRenderer> _shipSpots;


    private void Start()
    {
        InitializeSpots();
    }
    private void InitializeSpots()
    {
        for (int i = 0; i < _shipSpots.Count; i++)
        {            
            _shipSpots[i].transform.position = transform.position + (Vector3)(_unitPoints[i] * _radius);
            _shipSpots[i].transform.rotation = Quaternion.Euler(0, 0, i * (Mathf.PI / 5f * Mathf.Rad2Deg));
        }
    }
   
    public Vector2 GetRandomPositionInOrbit()
    {
        float randRadius = UnityEngine.Random.Range(_radius * 0.9f, _radius * 1.1f);
        Vector2 pos = transform.position + (Vector3)UnityEngine.Random.insideUnitCircle.normalized * randRadius;
        return pos;
    }

    public void SetSpots(int fillCount, Color color)
    {
        for (int i = 0; i < _shipSpots.Count; i++)
        {
            if (i < fillCount)
            {

                _shipSpots[i].sprite = _filledSprite;
                _shipSpots[i].color = color;
            }
            else
            {
                _shipSpots[i].sprite = _emptySprite;
            }

            //    _shipSpots[i].transform.position = transform.position + (Vector3)(_unitPoints[i] * _radius);
            //_shipSpots[i].transform.rotation = Quaternion.Euler(0,0, i * (Mathf.PI / 5f * Mathf.Rad2Deg));
        }

    }

    public void HighlightSpots(int spotsToHighlight, Color highlightColor)
    {
        for (int i = 0; i < _shipSpots.Count; i++)
        {
            if (i < spotsToHighlight)
            {
                _shipSpots[i].color = highlightColor;
            }
            else
            {
                _shipSpots[i].color = Color.white;
            }

            //    _shipSpots[i].transform.position = transform.position + (Vector3)(_unitPoints[i] * _radius);
            //_shipSpots[i].transform.rotation = Quaternion.Euler(0,0, i * (Mathf.PI / 5f * Mathf.Rad2Deg));
        }

    }



    private void Update()
    {
        transform.Rotate(Vector3.forward, _spinRate * Time.deltaTime);
    }


}
