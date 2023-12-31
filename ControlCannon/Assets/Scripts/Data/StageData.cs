using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageData
{
    public Vector3 CastlePosition;
    public Vector3 CastleRotation;
    public Gate[] Gates;
    public Obstacle[] Obstacles;

    [Serializable]
    public class Gate
    {
        public int Multiplier;
        public string GateType;
        public Vector3 GatePosition;
        public Vector3 GateScale;
    }

    [Serializable]
    public class Obstacle
    {
        public string ObstacleName;
        public Vector3 ObstaclePosition;
        public Vector3 ObstacleRotation;
        public Vector3 ObstacleScale;
    }
}
