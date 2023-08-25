using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMaker : MonoBehaviour
{
    [Serializable]
    public class GateInfo
    {
        public GameObject Gate;
        public int Multiplier;
        public EGateTypes GateType;
    }

    [SerializeField] private GameObject _castle;
    [SerializeField] private GameObject _gateRoot;
    [SerializeField] private GameObject _obstacleRoot;

    private GateInfo[] _gatesInfo = new GateInfo[0];

    public GameObject Castle { get { return _castle; } }
    public GameObject GateRoot { get { return _gateRoot; } }
    public GameObject ObstacleRoot { get { return _obstacleRoot; } }
    public GateInfo[] GatesInfo { get { return _gatesInfo; } }


    private void Update()
    {
        var gates = _gateRoot.GetComponentsInChildren<Collider>();
        if (gates.Length != _gatesInfo.Length)
            _RenewGatesInfo(gates);
    }

    private void _RenewGatesInfo(Collider[] gates)
    {
        _gatesInfo = new GateInfo[gates.Length];
        for (int ii = 0; ii < _gatesInfo.Length; ++ii)
        {
            _gatesInfo[ii] = new GateInfo();
            _gatesInfo[ii].Gate = gates[ii].gameObject;
        }
    }
}
