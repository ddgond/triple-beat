using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    public Vector3 bob = Vector3.up;
    public float frequency = 1;

    private Vector3 startPos;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + bob);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position - bob);
    }

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + bob * Mathf.Sin(Time.time * frequency);
    }
}
