using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    private float _rotateSpeed = 30.0f;

    private void FixedUpdate()
    {
        var rotateAngle = Quaternion.Euler(0f, _rotateSpeed * Time.fixedDeltaTime, 0f);
        transform.rotation *= rotateAngle;
    }
}
