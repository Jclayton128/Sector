using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetHandler : MonoBehaviour
{
    //refs
    [SerializeField] List<SpriteRenderer> _srs = new List<SpriteRenderer>();

    //settings
    float _closeEnough = 0.3f;

    //state
    int _allegiance = 0;
    public int Allegiance => _allegiance;

     int _fleetSize = 0;
    public int FleetSize => _fleetSize;

    float _speed;
    PlanetHandler _destination;


    //public void SetFleet(int size, float speed, PlanetHandler destination, int allegiance)
    //{
    //    _fleetSize = size;
    //    _speed = speed;
    //    transform.up = destination.transform.position - transform.position;
    //    _allegiance = allegiance;
    //    _destination = destination;

    //    RenderFleet(size, allegiance);

    //}

    //private void RenderFleet(int size, int allegiance)
    //{
    //    for (int i = 0; i < _srs.Count; i++)
    //    {
    //        if (i < size)
    //        {
    //            _srs[i].enabled = true;
    //        }
    //        else
    //        {
    //            _srs[i].enabled = false;
    //        }
    //    }

    //    //TODO update the color of the fleet to match allegiance
    //}

    //private void Update()
    //{
    //    transform.position += transform.up * _speed * Time.deltaTime;
    //    //check for distance to destination planet
    //    UpdateCheckDistance();
    //}

    //private void UpdateCheckDistance()
    //{
    //    if ((_destination.transform.position - transform.position).magnitude <= _closeEnough)
    //    {
    //        _destination.ReceiveFleet(this);
    //    }
    //}

    public void RemoveFleet()
    {
        Destroy(this.gameObject);
    }
    
}
