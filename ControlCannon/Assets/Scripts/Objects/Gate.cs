using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [Range(MIN_MULTIPLY_NUMBER, MAX_MULTIPLY_NUMBER)][SerializeField] private int _randomNumber;

    private const string TAG_MOB = "Mob";
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
        if (other.CompareTag(TAG_MOB))
        {
            for (int ii = 0; ii < _randomNumber - SELF; ++ii)
            {
                var replica = Manager.Instance.Object.GetObject(EObjectTypes.Mob);
                replica.transform.localPosition = other.transform.localPosition + other.transform.forward;
                replica.SetActive(true);
            }
        }
    }
}
