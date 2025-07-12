using System;
using UnityEngine;

public class BallDrag : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float drag = 2.0f;
    Vector3 velocity;

    private void Awake()
    {
        velocity = rb.linearVelocity;
    }

    void FixedUpdate()
    {
        SlowVelocityOverTime();
    }

    private void SlowVelocityOverTime()
    {
        velocity *= (1 / (1 + drag * Time.fixedDeltaTime));
        rb.linearVelocity = velocity;
    }
}
