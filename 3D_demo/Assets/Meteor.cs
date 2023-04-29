using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private LineRenderer lr;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    private void FixedUpdate()
    {
        GroundPlanet.Instance.Gravity(transform);
        DrawLine();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ObjectPool.Instance.AddToPool(0, gameObject);
        }
    }
    private void DrawLine()
    {
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, GroundPlanet.Instance.transform.position);
    }
}
