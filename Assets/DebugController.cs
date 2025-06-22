using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    public void IncreasePlayerAttack()
    {
        FactionController.Instance.ModifyFactionAttack(1, 5);
    }

    public void DecreasePlayerAttack()
    {
        FactionController.Instance.ModifyFactionAttack(1, -5);
    }

    public void IncreasePlayerDefense()
    {
        FactionController.Instance.ModifyFactionDefense(1, 1);
    }

    public void DecreasePlayerDefense()
    {
        FactionController.Instance.ModifyFactionDefense(1, -1);
    }
    public void IncreasePlayerSpeed()
    {
        FactionController.Instance.ModifyFactionSpeed(1, 0.5f);
    }

    public void DecreasePlayerSpeed()
    {
        FactionController.Instance.ModifyFactionSpeed(1, -.5f);
    }



    public void IncreaseComputerAttack()
    {
        FactionController.Instance.ModifyFactionAttack(-1, 5);
    }

    public void DecreaseComputerAttack()
    {
        FactionController.Instance.ModifyFactionAttack(-1, -5);
    }

    public void IncreaseComputerDefense()
    {
        FactionController.Instance.ModifyFactionDefense(-1, 1);
    }

    public void DecreaseComputerDefense()
    {
        FactionController.Instance.ModifyFactionDefense(-1, -1);
    }
    public void IncreaseComputerSpeed()
    {
        FactionController.Instance.ModifyFactionSpeed(-1, 0.5f);
    }

    public void DecreaseComputerSpeed()
    {
        FactionController.Instance.ModifyFactionSpeed(-1, -.5f);
    }
}
