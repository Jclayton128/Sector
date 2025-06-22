using System;
using System.Collections.Generic;
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
    ProductionHandler _productionHandler;

    //settings
    [SerializeField] float _commandRate = 0.75f;
    [SerializeField] FleetHandler _fleetPrefab = null;

    [Header("Combat Values")]
    //[SerializeField] float _timeBetweenBattles = 0.5f;
    [SerializeField] float _kVal = 0.1f;



    //stub in faction 


    //state
    [SerializeField] PlanetTypes _planetType = PlanetTypes.Rocky;
    public PlanetTypes PlanetType => _planetType;

    [SerializeField] int _citiesOnPlanet = 0;
    public int CitiesOnPlanet => _citiesOnPlanet;
    [SerializeField] int _basesInOrbit = 0;

    bool _isCommanding = false;
    float _fleetCommandFactor = 0;
    int _shipsCommanded;

    [SerializeField] int _allegiance = 1;
    public int Allegiance => _allegiance;


    [Header("Combat State")]
    float _countdownToNextCombatRound;
    [SerializeField] int _currentDefense_owner;
    [SerializeField] int _currentDefense_invader;
    int _invaderAllegiance;
    [SerializeField] List<ShipHandler> _shipsInOrbit_Owner = new List<ShipHandler>();
    [SerializeField] List<ShipHandler> _shipsInOrbit_Invader = new List<ShipHandler>();
    float _push;



    #region Flow
    private void Start()
    {
        _productionHandler = GetComponent<ProductionHandler>();

        _countdownToNextCombatRound = 0;
        _commandHandler.StopLine();
        _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Dehighlight);
        RenderPlanet();
    }

    private void Update()
    {
        if (_shipsInOrbit_Invader.Count > 0 && _shipsInOrbit_Owner.Count == 0)
        {
            //invert allegiance
            ExecuteAllegianceTurnover(_invaderAllegiance);
        }

        //include bases in the defender count
        else if (_shipsInOrbit_Invader.Count > 0 && (_shipsInOrbit_Owner.Count) > 0)
        {
            _countdownToNextCombatRound -= Time.deltaTime;
            if (_countdownToNextCombatRound <= 0)
            {
                ResolveCombatRound();

                _countdownToNextCombatRound = CalculateTimeUntilNextBattle();
            }
        }        
    }

    private float CalculateTimeUntilNextBattle()
    {
        float timeUntilNextRound;
        //with matched forces, this should be _timeBetweenBattles.
        //Number of units * unit speed vs enemy count * speed.

        _push = (_shipsInOrbit_Owner.Count * FactionController.Instance.GetFactionSpeed(_allegiance)) -
            (_shipsInOrbit_Invader.Count * FactionController.Instance.GetFactionSpeed(_invaderAllegiance));

        timeUntilNextRound = 1 / (1 + Mathf.Exp(-1 * _kVal * _push));
        timeUntilNextRound = Mathf.Clamp(timeUntilNextRound, 0.1f, 1f);
        //Debug.Log("combat time: " +  timeUntilNextRound);
        return timeUntilNextRound;
    }

    private void ExecuteAllegianceTurnover(int newAllegiance)
    {
        Debug.Log("Allegiance Turnover");
        _allegiance = newAllegiance;
        _invaderAllegiance = 0;

        _shipsInOrbit_Owner = new List<ShipHandler>(_shipsInOrbit_Invader);

        _citiesOnPlanet = 0;
        _productionHandler.SetProductionMode(ProductionHandler.ProductionModes.Farming);
        RenderPlanet();
        

        foreach (var ship in _shipsInOrbit_Owner)
        {
            ship.SetShipDestinationInOrbit(_ringShips.GetRandomPositionInOrbit(), _ringShips.transform);
        }

        _shipsInOrbit_Invader.Clear();
    }

    private void ResolveCombatRound()
    {
        int attackRoll_def = UnityEngine.Random.Range(0, FactionController.Instance.GetFactionAttack(_allegiance));
        int attackRoll_atk = UnityEngine.Random.Range(0, FactionController.Instance.GetFactionAttack(_invaderAllegiance));

        if (attackRoll_atk > attackRoll_def)
        {
            _currentDefense_owner--;
        }
        else if (attackRoll_atk < attackRoll_def)
        {
            _currentDefense_invader--;
        }
        else
        {
            //nothing happens in a tie
        }

        if (_currentDefense_owner <= 0)
        {
            if (_basesInOrbit > 0)
            {
                _basesInOrbit--;
                //RenderPlanet();
                Debug.Log("Defender Base destroyed");
            }
            else
            {
                int last = _shipsInOrbit_Owner.Count - 1;

                _shipsInOrbit_Owner[last].DestroySelf();
                _shipsInOrbit_Owner.RemoveAt(last);

                //RenderPlanet();
                Debug.Log("Defender Ship destroyed");
            }

            if (_basesInOrbit > 0)
            {
                _currentDefense_owner = 3 * FactionController.Instance.GetFactionDefense(_allegiance);
            }
            else
            {
                _currentDefense_owner = FactionController.Instance.GetFactionDefense(_allegiance);
            }
        }
        else if (_currentDefense_invader <= 0)
        {
            int last = _shipsInOrbit_Invader.Count - 1;

            _shipsInOrbit_Invader[last].DestroySelf();
            _shipsInOrbit_Invader.RemoveAt(last);

            //RenderPlanet();
            Debug.Log("Attacker destroyed");

            _currentDefense_invader = FactionController.Instance.GetFactionDefense(_invaderAllegiance);
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
        //_ringShips.SetSpots(_defendersInOrbit, col);
        //_ringEnemy.SetSpots(_attackersInOrbit, anticol);
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
            _shipsCommanded = Mathf.RoundToInt(_shipsInOrbit_Owner.Count * _fleetCommandFactor);
            _shipsCommanded = Mathf.Clamp(_shipsCommanded, 0, _shipsInOrbit_Owner.Count);

            for (int i = 0; i < _shipsCommanded; i++)
            {
                _shipsInOrbit_Owner[i].RecolorShip(Color.green);
            }

            _ringShips.HighlightSpots(_shipsCommanded, Color.green);

        }
    }

    public void HandleLMBDown()
    {
        _isCommanding = true;
        _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Commanding);
        _commandHandler.StartLine();
    }

    public void HandleLMBUp()
    {
        //if releasing while over a valid planet, then create a fleet, fill it with appropriate amount of commanded ships, and send it. Then update this planet's remaining fleet size.
        _commandHandler.StopLine();

        //should eventually check range and check planet for validity
        if (InputController.Instance.PlanetUnderCursor != null && InputController.Instance.PlanetUnderCursor != this)
        {
            SendFleet(_shipsCommanded);
        }

        foreach (var ship in _shipsInOrbit_Owner)
        {
            ship.RecolorShip(IconLibrary.Instance.GetFactionColor(_allegiance));
        }

        _shipsCommanded = 0;
        _isCommanding = false;
        _ringSelection.SetSelectionState(PlanetSelectionDriver.SelectionStates.Dehighlight);
        _fleetCommandFactor = 0;
    }

    public void HandleRMBDown()
    {
        _productionHandler.IncrementProductionMode();
    }

    public void HandleRMBUp()
    {

    }

    private void SendFleet(int numberOfShipsToSend)
    {
        List<ShipHandler> shipsToSend = new List<ShipHandler>(_shipsInOrbit_Owner.GetRange(0, numberOfShipsToSend));
        foreach (var ship in shipsToSend)
        {
            ship.SetShipDestinationAsPlanet(InputController.Instance.PlanetUnderCursor);
            _shipsInOrbit_Owner.Remove(ship);
            ship.RecolorShip(IconLibrary.Instance.GetFactionColor(_allegiance));
        }

    }


    //private void SendFleet()
    //{
    //    PlanetHandler destination = InputController.Instance.PlanetUnderCursor;

    //    FleetHandler newFleet = Instantiate(_fleetPrefab, transform.position, Quaternion.identity);
    //    newFleet.SetFleet(_shipsCommanded, FactionController.Instance.GetFactionSpeed(_allegiance), destination, _allegiance);

    //    _defendersInOrbit -= _shipsCommanded;
    //    _ringShips.HighlightSpots(0, Color.white);

    //    RenderPlanet();
    //}

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

    public void ReceiveShipHandler(ShipHandler newShip)
    {
        if (newShip.Allegiance == _allegiance)
        {
            _shipsInOrbit_Owner.Add(newShip);
            
            newShip.SetShipDestinationInOrbit(_ringShips.GetRandomPositionInOrbit(), _ringShips.transform);
        }
        else if (newShip.Allegiance != _allegiance)
        {
            _shipsInOrbit_Invader.Add(newShip);
            _invaderAllegiance = newShip.Allegiance;
            newShip.SetShipDestinationInOrbit(_ringEnemy.GetRandomPositionInOrbit(), _ringEnemy.transform);
        }
        else if (_shipsInOrbit_Owner.Count == 0)
        {
            _invaderAllegiance = newShip.Allegiance;
            ExecuteAllegianceTurnover(_invaderAllegiance);
            //_allegiance = newShip.Allegiance;
        }

        
        
    }

    public Vector2 GetRandomOrbit(int allegiance)
    {
        if (_allegiance == allegiance)
        {
            return _ringShips.GetRandomPositionInOrbit();

        }
        else if (_allegiance != allegiance)
        {
            return _ringEnemy.GetRandomPositionInOrbit();
        }
        else return _ringShips.GetRandomPositionInOrbit();
    }


    //public void ReceiveProducedShips(int shipsToAdd)
    //{
    //    _defendersInOrbit += shipsToAdd;
    //    RenderPlanet();
    //}

    public void ReceiveProducedBases(int basesToAdd)
    {
        _basesInOrbit += basesToAdd;
        RenderPlanet();
    }



    //public void ReceiveFleet(FleetHandler arrivingFleet)
    //{
    //    if (_allegiance == 0)
    //    {
    //        _defendersInOrbit += arrivingFleet.FleetSize;
    //        _allegiance = arrivingFleet.Allegiance;
    //    }
    //    else if (arrivingFleet.Allegiance == _allegiance)
    //    {
    //        _defendersInOrbit += arrivingFleet.FleetSize;
    //    }
    //    else if (arrivingFleet.Allegiance != _allegiance)
    //    {
    //        _attackersInOrbit += arrivingFleet.FleetSize;
    //        _attackerAllegiance = arrivingFleet.Allegiance;
    //        _currentDefense_invader = FactionController.Instance.GetFactionDefense(_attackerAllegiance);
    //        if (_basesInOrbit > 0)
    //        {
    //            _currentDefense_owner = 3 * FactionController.Instance.GetFactionDefense(_allegiance);
    //        }
    //        else
    //        {
    //            _currentDefense_owner = FactionController.Instance.GetFactionDefense(_allegiance);
    //        }
                
    //    }

    //    RenderPlanet();
    //    arrivingFleet.RemoveFleet();
    //}

    #endregion

}
