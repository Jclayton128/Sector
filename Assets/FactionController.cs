using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionController : MonoBehaviour
{
    public static FactionController Instance { get; private set; }

    //settings
    [SerializeField] int _startingAttack = 5;
    [SerializeField] int _startingDefense = 1;
    [SerializeField] float _startingRange = 3f;
    [SerializeField] float _startingSpeed = 1f;
    [SerializeField] float _startingSensor = 5f;

    //state
    public FactionStat PlayerFaction;
    public FactionStat ComputerFaction;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        PlayerFaction = new FactionStat();
        ComputerFaction = new FactionStat();

        PlayerFaction.Attack = _startingAttack;
        ComputerFaction.Attack = _startingAttack;

        PlayerFaction.Defense = _startingDefense;
        ComputerFaction.Defense = _startingDefense;

        PlayerFaction.Range = _startingRange;
        ComputerFaction.Range = _startingRange;

        PlayerFaction.Speed = _startingSpeed;
        ComputerFaction.Speed = _startingSpeed;

        PlayerFaction.Sensor = _startingSensor;
        ComputerFaction.Sensor = _startingSensor;

        PlayerFaction.GlobalFarmingBonus = 1;
        ComputerFaction.GlobalFarmingBonus = 1;

        PlayerFaction.ResearchPointsCollected = 0;
        ComputerFaction.ResearchPointsCollected = 0;
    }

    public float GetFactionFarmingBonus(int allegiance)
    {
        if (allegiance == 1) return PlayerFaction.GlobalFarmingBonus;
        else if (allegiance == -1) return ComputerFaction.GlobalFarmingBonus;
        else return 0;
    }

    public void ModifyFactionFarmingBonus(int allegiance, float amountToAdd)
    {
        if (allegiance == 1)
        {
            PlayerFaction.GlobalFarmingBonus += amountToAdd;
        }
        else if (allegiance == -1)
        {
            ComputerFaction.GlobalFarmingBonus += amountToAdd;
        }
    }

    public float GetFactionResearchCount(int allegiance)
    {
        if (allegiance == 1) return PlayerFaction.ResearchPointsCollected;
        else if (allegiance == -1) return ComputerFaction.ResearchPointsCollected;
        else return 0;
    }

    public void ModifyFactionResearchCount(int allegiance, int amountToAdd)
    {
        if (allegiance == 1)
        {
            PlayerFaction.ResearchPointsCollected += amountToAdd;
        }
        else if (allegiance == -1)
        {
            ComputerFaction.ResearchPointsCollected += amountToAdd;
        }
    }
}
