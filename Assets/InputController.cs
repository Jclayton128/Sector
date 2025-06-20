using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }


    //state
    [SerializeField] Vector3 _mousePosition;
    public Vector3 MousePosition => _mousePosition;


    [SerializeField] PlanetHandler _commandedPlanet;

    [SerializeField] PlanetHandler _planetUnderCursor;
    public PlanetHandler PlanetUnderCursor => _planetUnderCursor;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        UpdateMousePosition();
        UpdateMouseClick();
    }

    private void UpdateMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_planetUnderCursor != null)
            {
                _commandedPlanet = _planetUnderCursor;
                _planetUnderCursor.HandleLMBDown(); 
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_commandedPlanet != null)
            {
                _commandedPlanet.HandleLMBUp();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (_planetUnderCursor != null)
            {
                _planetUnderCursor.HandleRMBDown();
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (_planetUnderCursor != null)
            {
                _planetUnderCursor.HandleRMBUp();
            }
        }
    }

    private void UpdateMousePosition()
    {
        Plane plane = new Plane(Vector3.back, Vector3.zero); // Define a plane
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
        {
            _mousePosition = ray.GetPoint(enter);
        }               
    }
    
    public void SetPlanetUnderCursor(PlanetHandler planetUnderCursor)
    {
        _planetUnderCursor = planetUnderCursor;
    }
}

