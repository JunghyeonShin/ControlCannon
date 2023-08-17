using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 150f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {

    }

    private void FixedUpdate()
    {
        var moveVec = new Vector3(0f, 0f, _moveSpeed * transform.forward.z * Time.fixedDeltaTime);
        _rigidbody.velocity = moveVec;
    }

    public void SetTag(string tag)
    {
        gameObject.tag = tag;
    }
}
