using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionHandler : MonoBehaviour
{
    public enum ProductionModes { Ships, Bases, Farming, Science}


    //refs
    [SerializeField] PlanetHandler _planetHandler;

    //settings
    [SerializeField] float _rockyProductionMultiplier = 1.2f;
    [SerializeField] float _terranFarmingMultiplier = 1.2f;
    [SerializeField] float _artifactResearchMultiplier = 1.2f;


    [SerializeField] float _shipProductionRate = 0.1f;
    [SerializeField] float _baseProductionRate = 0.05f;
    [SerializeField] float _farmingBonus = 0.05f;
    [SerializeField] float _researchRate = 1f;

    //state
    [SerializeField] ProductionModes _productionMode = ProductionModes.Ships;
    ProductionModes _previousProductionMode;
    [SerializeField] float _production;


    void Start()
    {
        _planetHandler = GetComponent<PlanetHandler>();
        if (_planetHandler.PlanetType == PlanetHandler.PlanetTypes.Rocky)
        {
            _shipProductionRate *= _rockyProductionMultiplier;
            _baseProductionRate *= _rockyProductionMultiplier;
        }

        if (_planetHandler.PlanetType == PlanetHandler.PlanetTypes.Terran)
        {
            _farmingBonus *= _terranFarmingMultiplier;
        }

        if (_planetHandler.PlanetType == PlanetHandler.PlanetTypes.Artifact)
        {
            _researchRate *= _artifactResearchMultiplier;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch ( _productionMode )
        {
            case ProductionModes.Ships:
                UpdateShipProduction();
                break;

            case ProductionModes.Bases:
                UpdateBaseProduction();
                break;

            case ProductionModes.Farming:
                UpdateFarmingProduction();
                break;

            case ProductionModes.Science:
                UpdateScienceProduction();
                break;

        }
    }


    private void UpdateShipProduction()
    {
        //_production += Time.deltaTime * (_shipProductionRate);

        _production += Time.deltaTime * (_shipProductionRate * FactionController.Instance.GetFactionFarmingBonus(_planetHandler.Allegiance));
        if (_production > 1)
        {
            _production = 0;
            _planetHandler.ReceiveProducedShip();
        }
    }

    private void UpdateBaseProduction()
    {
        _production += Time.deltaTime * (_baseProductionRate * FactionController.Instance.GetFactionFarmingBonus(_planetHandler.Allegiance));
        if (_production > 1)
        {
            _production = 0;
            _planetHandler.ReceiveProducedBase();
        }
    }

    private void UpdateFarmingProduction()
    {
       //farming doesn't actually produce anything.
    }

    private void UpdateScienceProduction()
    {
        _production += Time.deltaTime * (_researchRate *
            FactionController.Instance.GetFactionFarmingBonus(_planetHandler.Allegiance));
        if (_production > 1)
        {
            _production = 0;
            FactionController.Instance.ModifyFactionResearchCount(_planetHandler.Allegiance,  1);
        }
    }

    public void SetProductionMode(ProductionModes newProductionMode)
    {
        _previousProductionMode = _productionMode;

        _productionMode = newProductionMode;
        _production = 0;

        if (_previousProductionMode == ProductionModes.Farming)
        {
            FactionController.Instance.ModifyFactionFarmingBonus(
                _planetHandler.Allegiance, -_farmingBonus);
        }

        if (_productionMode == ProductionModes.Farming)
        {
            FactionController.Instance.ModifyFactionFarmingBonus(
                _planetHandler.Allegiance, _farmingBonus);
        }       

    }
}
