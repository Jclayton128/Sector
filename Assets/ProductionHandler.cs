using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionHandler : MonoBehaviour
{
    public enum ProductionModes { Farming, Ships, Bases, Research, Count}


    //refs
    PlanetHandler _planetHandler;
    [SerializeField] SpriteRenderer _modeSprite = null;
    [SerializeField] Image _cityGrowthRing = null;
    [SerializeField] Image _productionGrowthRing = null;


    //settings
    [Header("Production Icons")]
    [Tooltip("0: Farming, 1: Ships, 2: Bases, 3: Research")]
    [SerializeField] Sprite[] _icons = null;

    [Header("Planet Type Multipliers")]
    [SerializeField] float _rockyProductionMultiplier = 1.2f;
    [SerializeField] float _terranFarmingMultiplier = 1.2f;
    [SerializeField] float _artifactResearchMultiplier = 1.2f;

    [Header("Base Rates")]
    [SerializeField] float _cityGrowthRate = 0.05f;
    [SerializeField] float _productionRate = 0.1f;
    [SerializeField] float _farmingBonus = 0.05f;
    [SerializeField] float _researchRate = 1f;

    //state
    ProductionModes _productionMode = ProductionModes.Count;
    //ProductionModes _previousProductionMode = ProductionModes.Count;
    [SerializeField] float _production;
    [SerializeField] float _cityGrowth;


    void Start()
    {
        _planetHandler = GetComponent<PlanetHandler>();
        _productionGrowthRing.fillAmount = 0;
        _cityGrowthRing.fillAmount = 0;

        SetProductionMode(ProductionModes.Farming);
        if (_planetHandler.PlanetType == PlanetHandler.PlanetTypes.Rocky)
        {
            _productionRate *= _rockyProductionMultiplier;
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

    #region Flow

    // Update is called once per frame
    void Update()
    {
        if (_planetHandler.CitiesOnPlanet < 1 && _productionMode != ProductionModes.Farming)
        {
            SetProductionMode(ProductionModes.Farming);
        }

        UpdateCityGrowth();



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

            case ProductionModes.Research:
                UpdateScienceProduction();
                break;

        }
    }

    private void UpdateCityGrowth()
    {
        _cityGrowth += Time.deltaTime * (_cityGrowthRate * FactionController.Instance.GetFactionFarmingBonus(_planetHandler.Allegiance) /
            (0.5f + (_planetHandler.CitiesOnPlanet * _planetHandler.CitiesOnPlanet / 2f)));

        _cityGrowthRing.fillAmount = _cityGrowth / 1;

        if (_cityGrowth > 1)
        {
            _cityGrowth = 0;
            _planetHandler.ReceiveCity();
        }
    }

    private void UpdateShipProduction()
    {
        //_production += Time.deltaTime * (_shipProductionRate);

        _production += Time.deltaTime * (_productionRate * FactionController.Instance.GetFactionFarmingBonus(_planetHandler.Allegiance));
        _productionGrowthRing.fillAmount = _production / 1;

        if (_production > 1)
        {
            _production = 0;
            for (int i = 0; i < _planetHandler.CitiesOnPlanet; i++)
            {
                var newShip = Instantiate(IconLibrary.Instance.ShipPrefab, transform.position, Quaternion.identity);
                newShip.SetAllegiance(_planetHandler.Allegiance);
                _planetHandler.ReceiveShipHandler(newShip);
            }
        }
    }

    private void UpdateBaseProduction()
    {
        _production += Time.deltaTime * (_productionRate * FactionController.Instance.GetFactionFarmingBonus(_planetHandler.Allegiance)) / 2;
        _productionGrowthRing.fillAmount = _production / 1;

        if (_production > 1)
        {
            _production = 0;
            _planetHandler.ReceiveProducedBases(1 * _planetHandler.CitiesOnPlanet);
        }
    }

    private void UpdateFarmingProduction()
    {
        UpdateCityGrowth();
    }

    private void UpdateScienceProduction()
    {
        _production += Time.deltaTime * (_researchRate *
            FactionController.Instance.GetFactionFarmingBonus(_planetHandler.Allegiance));
        if (_production > 1)
        {
            _production = 0;
            FactionController.Instance.ModifyFactionResearchCount(_planetHandler.Allegiance,  1 * _planetHandler.CitiesOnPlanet);
        }
    }

    #endregion

    #region Mode Changing
    public void IncrementProductionMode()
    {
        int index = (int)_productionMode;
        index++;
        if (index >= (int)ProductionModes.Count)
        {
            index = 0;
        }

        SetProductionMode((ProductionModes)index);
    }

    public void SetProductionMode(ProductionModes newProductionMode)
    {
        if (_productionMode == ProductionModes.Farming)
        {
            FactionController.Instance.ModifyFactionFarmingBonus(
                _planetHandler.Allegiance, -1 * _farmingBonus);
        }

        _productionMode = newProductionMode;
        //_production = 0;

        if (_productionMode == ProductionModes.Farming)
        {
            FactionController.Instance.ModifyFactionFarmingBonus(
                _planetHandler.Allegiance, _farmingBonus);
        }

        _modeSprite.sprite = _icons[(int)_productionMode];
    }

    #endregion
}
