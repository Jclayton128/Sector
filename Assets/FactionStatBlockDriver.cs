using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactionStatBlockDriver : MonoBehaviour
{
    //refs
    [SerializeField] TextMeshProUGUI _bonusTMP = null;
    [SerializeField] TextMeshProUGUI _attackTMP = null;
    [SerializeField] TextMeshProUGUI _defenseTMP = null;
    [SerializeField] TextMeshProUGUI _rangeTMP = null;
    [SerializeField] TextMeshProUGUI _speedTMP = null;

    public void PushFactionStatus(FactionStat factionStat)
    {
        _bonusTMP.text = (factionStat.GlobalFarmingBonus).ToString("P0");
        _attackTMP.text = factionStat.Attack.ToString();
        _defenseTMP.text = factionStat.Defense.ToString();
        _rangeTMP.text = factionStat.Range.ToString();
        _speedTMP.text = factionStat.Speed.ToString();  
    }

}
