using System;
using UnityEngine;

public class PlanetHandler : MonoBehaviour
{
    public enum PlanetTypes { Rocky, Gas, Terran, Artifact}

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
    [SerializeField] float _timeBetweenBattles = 0.5f;

    //stub in faction 
    int _maxAttack_def = 5;
    int _maxAttack_atk = 25;
    int _defense_def = 5;
    int _defense_atk = 3;

    //state
    [SerializeField] PlanetTypes _planetType = PlanetTypes.Rocky;
    public PlanetTypes PlanetType => _planetType;

    [SerializeField] int _citiesOnPlanet = 0;
    public int CitiesOnPlanet => _citiesOnPlanet;
    [SerializeField] int _basesInOrbit = 0;
    [SerializeField] int _defendersInOrbit = 0;
    [SerializeField] int _attackersInOrbit = 0;

    bool _isCommanding = false;
    float _fleetCommandFactor = 0;
    int _shipsCommanded;

    [SerializeField] int _allegiance = 1;
    public int Allegiance => _allegiance;

    float _countdownToNextCombatRound;
    int _currentDefense_def;
    int _currentDefense_atk;

    int _attackerAllegiance;


    #region Flow
    private void Start()
    {
        _countdownToNextCombatRound = 0;
        _commandHandler.StopLine();
        _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Dehighlight);
        RenderPlanet();
    }

    private void Update()
    {
        if (_attackersInOrbit > 0 && _defendersInOrbit == 0)
        {
            //invert allegiance
            _allegiance *= -1;

            _defendersInOrbit = _attackersInOrbit;
            _attackersInOrbit = 0;
            RenderPlanet();
        }

        else if (_attackersInOrbit > 0 && _defendersInOrbit > 0)
        {
            _countdownToNextCombatRound -= Time.deltaTime;
            if (_countdownToNextCombatRound <= 0)
            {
                ResolveCombatRound();
                _countdownToNextCombatRound = _timeBetweenBattles;
            }
        }        
    }

    private void ResolveCombatRound()
    {
        int attackRoll_def = UnityEngine.Random.Range(0, FactionController.Instance.GetFactionAttack(_allegiance));
        int attackRoll_atk = UnityEngine.Random.Range(0, FactionController.Instance.GetFactionAttack(_attackerAllegiance));

        if (attackRoll_atk > attackRoll_def)
        {
            _currentDefense_def--;
        }
        else if (attackRoll_atk < attackRoll_def)
        {
            _currentDefense_atk--;
        }
        else
        {
            //nothing happens in a tie
        }

        if (_currentDefense_def <= 0)
        {
            _defendersInOrbit--;
            RenderPlanet();
            Debug.Log("Defender destroyed");

            _currentDefense_def = _defense_def;

        }
        else if (_currentDefense_atk <= 0)
        {
            _attackersInOrbit--;
            RenderPlanet();
            Debug.Log("Attacker destroyed");

            _currentDefense_atk = _defense_atk;
        }


    }
    


    #endregion

    #region Rendering

    public void RenderPlanet()
    {
        Color col = Color.white;
        Color anticol = Color.red;
        if (_allegiance == 1)
        {
            col = Color.white;
            anticol = Color.red;
        }
        else if (_allegiance == -1)
        {
            col = Color.red;
            anticol = Color.white;
        }

        _ringCities.SetSpots(_citiesOnPlanet, col);
        _ringBases.SetSpots(_basesInOrbit, col);
        _ringShips.SetSpots(_defendersInOrbit, col);
        _ringEnemy.SetSpots(_attackersInOrbit, anticol);
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
            _shipsCommanded = Mathf.RoundToInt(_defendersInOrbit * _fleetCommandFactor);
            _shipsCommanded = Mathf.Clamp(_shipsCommanded, 0, _defendersInOrbit);

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

        _defendersInOrbit -= _shipsCommanded;
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

    #region Additions

    public void ReceiveCity()
    {
        _citiesOnPlanet++;
        RenderPlanet();
    }

    public void ReceiveProducedShips(int shipsToAdd)
    {
        _defendersInOrbit += shipsToAdd;
        RenderPlanet();
    }

    public void ReceiveProducedBases(int basesToAdd)
    {
        _basesInOrbit += basesToAdd;
        RenderPlanet();
    }

    public void ReceiveFleet(FleetHandler arrivingFleet)
    {
        if (_allegiance == 0)
        {
            _defendersInOrbit += arrivingFleet.FleetSize;
            _allegiance = arrivingFleet.Allegiance;
        }
        else if (arrivingFleet.Allegiance == _allegiance)
        {
            _defendersInOrbit += arrivingFleet.FleetSize;
        }
        else if (arrivingFleet.Allegiance != _allegiance)
        {
            _attackersInOrbit += arrivingFleet.FleetSize;
            _attackerAllegiance = arrivingFleet.Allegiance;
            _currentDefense_atk = _defense_atk;
            _currentDefense_def = _defense_atk;
        }

        RenderPlanet();
        arrivingFleet.RemoveFleet();
    }

    #endregion

}
