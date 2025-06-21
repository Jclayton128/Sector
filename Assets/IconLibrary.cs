using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconLibrary : MonoBehaviour
{
    public static IconLibrary Instance { get; private set; }

    //settings
    public Sprite Ship_1 = null;
    public Sprite Ship_10 = null;
    public Sprite Ship_100 = null;

    public Sprite Base_1 = null;
    public Sprite Base_10 = null;
    public Sprite Base_100 = null;

    private void Awake()
    {
        Instance = this;
    }
}
