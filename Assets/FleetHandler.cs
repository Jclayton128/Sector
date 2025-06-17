using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetHandler : MonoBehaviour
{
    //refs
    [SerializeField] List<SpriteRenderer> _srs = new List<SpriteRenderer>();


    //state
    [SerializeField] int _fleetSize = 0;
    public int FleetSize => _fleetSize;
    float _speed;

    public void SetFleet(int size, float speed, PlanetHandler destination)
    {
        _fleetSize = size;
        speed = _speed;
        transform.up = destination.transform.position - transform.position; 
    }

    private void Update()
    {
        transform.position += transform.up * _speed;
        //check for distance to destination planet
    }

}
