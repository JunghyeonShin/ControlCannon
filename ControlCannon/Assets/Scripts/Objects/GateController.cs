using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GateController : MonoBehaviour
{
    private TextMeshProUGUI _multiplierText;
    private int _multiplier;

    public int LoadIndex { get; set; }

    private const string MULTIPLIER_OBJECT_NAME = "Multiplier";
    private const int SELF = 1;

    private void Awake()
    {
        _multiplierText = Utils.FindChild<TextMeshProUGUI>(gameObject, MULTIPLIER_OBJECT_NAME);
    }

    private void OnEnable()
    {
        var currentStageInfo = Manager.Instance.Stage.CurrentStageInfo;
        if (null == currentStageInfo)
            return;

        transform.localPosition = currentStageInfo.gates[LoadIndex].gatePosition;
        _multiplier = currentStageInfo.gates[LoadIndex].multiplier;
        _multiplierText.text = $"X{_multiplier}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.TAG_MOB))
        {
            for (int ii = 0; ii < _multiplier - SELF; ++ii)
            {
                var replicaMob = Manager.Instance.Object.GetObject(EObjectTypes.Mob);
                replicaMob.transform.localPosition = other.transform.localPosition + other.transform.forward;
                Utils.GetOrAddComponent<AllyMobController>(replicaMob);
                replicaMob.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        Manager.Instance.Object.ReturnObject(EObjectTypes.Gate, gameObject);
    }
}
