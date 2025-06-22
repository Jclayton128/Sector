using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandler : MonoBehaviour
{
    //settings
    float _closeEnough_Planet = 1.2f;
    float _closeEnough_Position = 0.1f;


    //state
    int _allegiance;
    public int Allegiance => _allegiance;
    [SerializeField] bool _hasDestination = false;
    [SerializeField] float _speed;
    [SerializeField] PlanetHandler _destinationPlanet;
    [SerializeField] Vector3 _destinationPosition;
    [SerializeField] Transform _destinationTransform;

    public void SetAllegiance(int allegiance)
    {
        _allegiance = allegiance;
    }

    public void SetShipDestinationAsPlanet(PlanetHandler destinationPlanet)
    {
        _hasDestination = true;
        _speed = FactionController.Instance.GetFactionSpeed(_allegiance);
        _destinationPlanet = destinationPlanet;
        transform.parent = null;
    }

    public void SetShipDestinationInOrbit(Vector2 destinationPos, Transform destinationTransform)
    {
        _destinationPlanet = null;
        _hasDestination = true;
        _speed = FactionController.Instance.GetFactionSpeed(_allegiance);
        _destinationPosition = destinationPos;
        _destinationTransform = destinationTransform;
    }

    private void Update()
    {
        if (!_hasDestination) return;
        transform.position += transform.up * _speed * Time.deltaTime;
        //check for distance to destination planet
        UpdateCheckDistance();
    }

    private void UpdateCheckDistance()
    {
        if (_destinationPlanet != null)
        {
            transform.up = _destinationPlanet.transform.position - transform.position;
            if ((_destinationPlanet.transform.position - transform.position).magnitude <= _closeEnough_Planet)
            {
                _destinationPlanet.ReceiveShipHandler(this);
                _destinationPlanet = null;
            }
        }
        else
        {
            transform.up = _destinationPosition - transform.position;
            if ((_destinationPosition - transform.position).magnitude <= _closeEnough_Position)
            {
                transform.parent = _destinationTransform;
                _hasDestination = false;
            }
        }

    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
