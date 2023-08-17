using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [Range(MIN_MULTIPLY_NUMBER, MAX_MULTIPLY_NUMBER)][SerializeField] private int _randomNumber;

    private const int MIN_MULTIPLY_NUMBER = 2;
    private const int MAX_MULTIPLY_NUMBER = 5;
    private const int SELF = 1;

    private void OnEnable()
    {
        _randomNumber = Random.Range(MIN_MULTIPLY_NUMBER, MAX_MULTIPLY_NUMBER);
        _text.text = $"X{_randomNumber}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.TAG_MOB))
        {
            for (int ii = 0; ii < _randomNumber - SELF; ++ii)
            {
                var replicaMob = Manager.Instance.Object.GetObject(EObjectTypes.Mob);
                replicaMob.transform.localPosition = other.transform.localPosition + other.transform.forward;
                Utils.GetOrAddComponent<AllyMobController>(replicaMob);
                replicaMob.SetActive(true);
            }
        }
    }
}
