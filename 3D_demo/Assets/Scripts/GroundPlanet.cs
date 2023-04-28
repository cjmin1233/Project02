using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlanet : MonoBehaviour
{
    private static GroundPlanet _instance;
    public static GroundPlanet Instance { get { return _instance; } }
    public float gravity = -10f;
    private GameObject player_object;
    float distance;
    Vector3 radius_vector;
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        player_object = GameObject.FindGameObjectWithTag("Player");
        distance = transform.lossyScale.x * GetComponent<SphereCollider>().radius +
            player_object.transform.lossyScale.x * player_object.GetComponent<SphereCollider>().radius;

    }
    public void Attract(Transform target_transform)
    {
        Vector3 gravityUp = (target_transform.position - transform.position).normalized;
        Vector3 localUp = target_transform.up;
        Rigidbody target_rigidbody = target_transform.GetComponent<Rigidbody>();


        target_rigidbody.AddForce(gravityUp * gravity * Mathf.Pow(target_rigidbody.velocity.magnitude, 2) / Vector3.Distance(transform.position, target_transform.position), ForceMode.Acceleration);

        Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * target_transform.rotation;
        target_transform.rotation = Quaternion.Slerp(target_transform.rotation, targetRotation, 0.9f);
    }
    public void PlayerOnPlanet()
    {
        radius_vector = player_object.transform.position - transform.position;
        if (radius_vector.magnitude > distance)
        {
            Vector3 targetPosition = transform.position + radius_vector.normalized * distance;
            player_object.transform.position = Vector3.Lerp(player_object.transform.position, targetPosition, 0.9f);
        }
    }
}
