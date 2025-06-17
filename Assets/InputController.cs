using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateCheckForMouseoverPlanet();
    }

    private void UpdateCheckForMouseoverPlanet()
    {

        if (Input.GetMouseButtonDown(0)) // Example: Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                // Handle the hit (e.g., destroy the object, change its color)
                Debug.Log("Hit: " + hit.collider.gameObject.name);
                // Example: Destroy the hit object
                // Destroy(hit.collider.gameObject);
            }
        }
    }
}

