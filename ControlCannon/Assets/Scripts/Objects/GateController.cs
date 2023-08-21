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
    private const float MIN_RANDOM_POSITION_X = -1f;
    private const float MAX_RANDOM_POSITION_X = 1f;
    private const float MIN_RANDOM_POSITION_Z = 0f;
    private const float MAX_RANDOM_POSITION_Z = 1f;
    private const int SELF = 1;

    private void Awake()
    {
        var gateRenderer = GetComponentInChildren<Renderer>();
        var allyMaterial = Manager.Instance.Resource.Load<Material>(Define.RESOURCE_MATERIAL_ALLY);
        gateRenderer.sharedMaterial = allyMaterial;

        _multiplierText = Utils.FindChild<TextMeshProUGUI>(gameObject, MULTIPLIER_OBJECT_NAME);
    }

    private void OnEnable()
    {
        var currentStageInfo = Manager.Instance.Stage.CurrentStageInfo;
        if (null == currentStageInfo)
            return;

        transform.localPosition = currentStageInfo.Gates[LoadIndex].GatePosition;
        _multiplier = currentStageInfo.Gates[LoadIndex].Multiplier;
        _multiplierText.text = $"X{_multiplier}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.TAG_ALLY_MOB))
        {
            var allyMobController = other.gameObject.GetComponent<AllyMobController>();
            if (allyMobController.IsPassedGate(LoadIndex))
                return;

            allyMobController.PassGate(LoadIndex);
            for (int ii = 0; ii < _multiplier - SELF; ++ii)
            {
                var replicaAllyMob = Manager.Instance.Object.GetObject(EObjectTypes.AllyMob);
                var randomPosition = new Vector3(Random.Range(MIN_RANDOM_POSITION_X, MAX_RANDOM_POSITION_X), 0f, Random.Range(MIN_RANDOM_POSITION_Z, MAX_RANDOM_POSITION_Z));
                replicaAllyMob.transform.localPosition = other.transform.localPosition + randomPosition;
                var replicaAllyMobController = Utils.GetOrAddComponent<AllyMobController>(replicaAllyMob);
                replicaAllyMob.SetActive(true);
                replicaAllyMobController.PassGate(LoadIndex);
            }
        }
    }

    private void OnDisable()
    {
        Manager.Instance.Object.ReturnObject(EObjectTypes.Gate, gameObject);
    }
}
