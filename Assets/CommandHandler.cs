using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
    //refs
    [SerializeField] LineRenderer _commandLine = null;

    //settings
    [SerializeField] Color _greyColor = Color.grey;
    [SerializeField] Color _inRangeColor = Color.green;
    [SerializeField] Color _stretchColor = Color.yellow;
    [SerializeField] Color _outRangeColor = Color.red;

    [SerializeField] float _rangeMax = 5f;

    //state
    [SerializeField] float _rangeCurrent;
    bool _isLining = false;


    public void StartLine()
    {
        _isLining = true;
        _commandLine.positionCount = 2;
        _commandLine.SetPosition(0, transform.position);
        _commandLine.SetPosition(1, transform.position);
    }

    public void StopLine()
    {
        _isLining = false;
        ClearLine();
    }

    private void Update()
    {
        if (_isLining)
        {
            _commandLine.SetPosition(1, InputController.Instance.MousePosition);
        }
    }

    private void ClearLine()
    {
        _commandLine.positionCount = 0;
    }

}
