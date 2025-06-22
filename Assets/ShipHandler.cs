using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandler : MonoBehaviour
{
    //refs
    ParticleSystem _ps;
    ParticleSystem.MainModule _psmm;
    SpriteRenderer _sr;

    //settings
    float _closeEnough_Planet = 0.3f;
    float _closeEnough_Position = 0.1f;


    //state
    [SerializeField] int _allegiance;
    public int Allegiance => _allegiance;
    [SerializeField] bool _hasDestination = false;
    [SerializeField] float _speed;
    [SerializeField] PlanetHandler _destinationPlanet;
    [SerializeField] Vector3 _destinationPosition;
    [SerializeField] Transform _destinationTransform;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _ps = GetComponent<ParticleSystem>();
        _psmm = _ps.main;
    }

    public void SetAllegiance(int allegiance)
    {
        _allegiance = allegiance;
        if (_allegiance == 1)
        {
            RecolorShip(Color.white);
        }
        else if (_allegiance == -1)
        {
            RecolorShip(Color.red);
        }
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
        Debug.Log($"new orbit destination under this transform: {destinationTransform}");
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

    public void RecolorShip(Color color)
    {
        _sr.color = color;
        _psmm.startColor = color;
    }


    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
