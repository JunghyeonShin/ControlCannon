using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCastleLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.TAG_ALLY_MOB))
        {
            var allyMobController = other.GetComponent<IAllyMobController>();
            allyMobController.AllyMoveTypes = EAllyMobMoves.Tracking;
        }
    }
}
