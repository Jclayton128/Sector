using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSelectionDriver : MonoBehaviour
{
    public enum SelectionStates { Dehighlight, Highlight, Commanding, Commanded}


    //refs
    [SerializeField] SpriteRenderer _sr = null;

    //settings
    [SerializeField] Sprite _highlightSprite = null;
    [SerializeField] Sprite _commandedSprite = null;
    [SerializeField] Sprite _commandingSprite = null;


    public void SetSelectionState(SelectionStates selectionState)
    {
        switch (selectionState)
        {
            case SelectionStates.Dehighlight:
                _sr.enabled = false;
                break;

            case SelectionStates.Highlight:
                _sr.enabled = true;
                _sr.sprite = _highlightSprite;
                break;

            case SelectionStates.Commanded:
                _sr.enabled = true;
                _sr.sprite = _commandedSprite;
                break;

            case SelectionStates.Commanding:
                _sr.enabled = true;
                _sr.sprite = _commandingSprite;
                break;

        }
    }
}
