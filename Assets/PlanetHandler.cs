using UnityEngine;

public class PlanetHandler : MonoBehaviour
{
    //refs
    [SerializeField] PlanetSelectionDriver _ringSelection = null;
    [SerializeField] RingDriver _ringCities = null;
    [SerializeField] RingDriver _ringBases = null;
    [SerializeField] RingDriver _ringShips = null;
    [SerializeField] RingDriver _ringEnemy = null;
    [SerializeField] CommandHandler _commandHandler = null;

    //settings
    [SerializeField] float _commandRate = 0.75f;
    [SerializeField] FleetHandler _fleetPrefab = null;


    //state
    [SerializeField] int _citiesOnPlanet = 0;
    [SerializeField] int _basesInOrbit = 0;
    [SerializeField] int _shipsInOrbit = 0;
    [SerializeField] int _enemiesInOrbit = 0;

    bool _isCommanding = false;
    float _fleetCommandFactor = 0;
    int _shipsCommanded;

    [SerializeField] int _allegiance = 1;


    private void Start()
    {
        _commandHandler.StopLine();
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
        InputController.Instance.SetPlanetUnderCursor(this);

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
        InputController.Instance.SetPlanetUnderCursor(null);

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
            _shipsCommanded = Mathf.RoundToInt(_shipsInOrbit * _fleetCommandFactor);
            _shipsCommanded = Mathf.Clamp(_shipsCommanded, 0, _shipsInOrbit);

            _ringShips.HighlightSpots(_shipsCommanded, Color.green);

        }
    }

    private void OnMouseDown()
    {
        _isCommanding = true;
        _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Commanding);
        _commandHandler.StartLine();
    }

    private void OnMouseUp()
    {
        //if releasing while over a valid planet, then create a fleet, fill it with appropriate amount of commanded ships, and send it. Then update this planet's remaining fleet size.
        _commandHandler.StopLine();

        //should eventually check range and check planet for validity
        if (InputController.Instance.PlanetUnderCursor != null && InputController.Instance.PlanetUnderCursor != this)
        {
            SendFleet();
        }

        _isCommanding = false;
        _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Dehighlight);
        _fleetCommandFactor = 0;
    }

    private void SendFleet()
    {
        PlanetHandler destination = InputController.Instance.PlanetUnderCursor;

        FleetHandler newFleet = Instantiate(_fleetPrefab, transform.position, Quaternion.identity);
        newFleet.SetFleet(_shipsCommanded, 1f, destination, _allegiance);

        _shipsInOrbit -= _shipsCommanded;
        _ringShips.HighlightSpots(0, Color.white);

        RenderPlanet();
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

    #region Fleet Arrival

    public void ReceiveFleet(FleetHandler arrivingFleet)
    {
        if (arrivingFleet.Allegiance == _allegiance)
        {
            _shipsInOrbit += arrivingFleet.FleetSize;
            RenderPlanet();

            arrivingFleet.RemoveFleet();

        }
    }

    #endregion

}
