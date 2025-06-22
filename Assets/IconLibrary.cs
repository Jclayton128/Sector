using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconLibrary : MonoBehaviour
{
    public static IconLibrary Instance { get; private set; }

    //settings
    public Color PlayerColor = Color.white;
    public Color EnemyColor = Color.red;

    public ShipHandler ShipPrefab = null;


    private void Awake()
    {
        Instance = this;
    }

    public Color GetFactionColor(int allegiance)
    {
        if (allegiance == 1) return PlayerColor;
        else if (allegiance == -1) return EnemyColor;
        else return Color.blue;
    }
}
