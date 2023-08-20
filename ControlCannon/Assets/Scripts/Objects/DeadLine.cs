using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Define.TAG_ENEMY_MOB))
        {
            other.gameObject.SetActive(false);
            Manager.Instance.Stage.DefeatStage();
        }
    }
}
