using UnityEngine;

public class PlanetHandler : MonoBehaviour
{
    //refs
    [SerializeField] PlanetSelectionDriver _ringSelection = null;
    [SerializeField] RingDriver _ringCities = null;
    [SerializeField] RingDriver _ringBases = null;
    [SerializeField] RingDriver _ringShips = null;
    [SerializeField] RingDriver _ringEnemy = null;

    //settings
    [SerializeField] float _commandRate = 0.5f;
    [SerializeField] FleetHandler _fleetPrefab = null;


    //state
    [SerializeField] int _citiesOnPlanet = 0;
    [SerializeField] int _basesInOrbit = 0;
    [SerializeField] int _shipsInOrbit = 0;
    [SerializeField] int _enemiesInOrbit = 0;

    bool _isCommanding = false;
    [SerializeField] float _fleetCommandFactor = 0;


    private void Start()
    {
        _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Dehighlight);
        RenderPlanet();
    }

    #region Rendering

    public void RenderPlanet()
    {
        _ringCities.SetSpots(_citiesOnPlanet);
        _ringBases.SetSpots(_basesInOrbit);
        _ringShips.SetSpots(_shipsInOrbit);
        _ringEnemy.SetSpots(_enemiesInOrbit);
    }

    #endregion



    #region Mouse Response

    private void OnMouseEnter()
    {
        if (_fleetCommandFactor > 0)
        {
            _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Commanded);
        }
        else
        {
            _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Highlight);
        }
    }

    private void OnMouseExit()
    {
        if (_fleetCommandFactor > 0)
        {
            //stay in command-select
            _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Commanded);
        }
        else
        {
            _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Dehighlight);
        }

    }

    private void OnMouseOver()
    {
        if (_isCommanding)
        {
            _fleetCommandFactor += _commandRate * Time.deltaTime;
        }
    }

    private void OnMouseDown()
    {
        _isCommanding = true;
        _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Commanding);
    }

    private void OnMouseUp()
    {
        //if releasing while over a valid planet, then create a fleet, fill it with appropriate amount of commanded ships, and send it. Then update this planet's remaining fleet size.


        _isCommanding = false;
        _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Dehighlight);
        _fleetCommandFactor = 0;
    }

    public void HighlightPlanet()
    {
        _ringSelection.enabled = true;
    }

    public void DehighlightPlanet()
    {
        _ringSelection.enabled = false;
    }

    #endregion

}
