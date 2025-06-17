using UnityEngine;

public class PlanetHandler : MonoBehaviour
{
    //refs
    [SerializeField] SpriteRenderer _ringSelection = null;
    [SerializeField] RingArranger _ringCities = null;
    [SerializeField] RingArranger _ringBases = null;
    [SerializeField] RingArranger _ringShips = null;
    [SerializeField] RingArranger _ringEnemy = null;

    //state
    [SerializeField] int _citiesOnPlanet = 0;
    [SerializeField] int _basesInOrbit = 0;
    [SerializeField] int _shipsInOrbit = 0;
    [SerializeField] int _enemiesInOrbit = 0;


    private void Start()
    {
        DehighlightPlanet();
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



    #region Highlighting

    private void OnMouseEnter()
    {
        Debug.Log("mouse enter");
        HighlightPlanet();
    }

    private void OnMouseExit()
    {
        Debug.Log("mouse exit");
        DehighlightPlanet();
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
