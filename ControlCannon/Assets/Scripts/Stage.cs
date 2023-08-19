using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stage
{
    public Vector3 castlePosition;
    public Vector3 castleRotation;
    public Gate[] gates;

    [Serializable]
    public class Gate
    {
        public int multiplier;
        public Vector3 gatePosition;
    }
}
