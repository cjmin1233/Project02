using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    [SerializeField] private GameObject main_camera;
    [SerializeField] private Vector3 cam_pos_offset;
    [SerializeField] private Vector3 cam_rot_offset;
    [SerializeField] private float cam_speed;

    private GameObject ground_planet;
    private GameObject player;

    float distance;
    Vector3 radius_R;
    Vector3 cam_pos;
    Quaternion cam_rot;
    public float speed;
    Vector3 moveDirection;
    Vector3 sphereNormal;

    public float gravity = -120f;
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        ground_planet = GameObject.FindGameObjectWithTag("Ground");
        player = GameObject.FindGameObjectWithTag("Player");
        distance = (player.transform.lossyScale.x + ground_planet.transform.lossyScale.x) * 0.5f;
    }
    private void FixedUpdate()
    {
        PlayerOnPlanet();
    }
    public void Attract()
    {
        Vector3 gravityUp = (player.transform.position - ground_planet.transform.position).normalized;
        Vector3 localUp = player.transform.up;

        //float player_sp = player.GetComponent<Rigidbody>().velocity.magnitude;
        player.transform.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * player.transform.rotation;
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 50f * Time.deltaTime);
    }
    private void PlayerOnPlanet()
    {
        Vector3 radius_vec = player.transform.position - ground_planet.transform.position;
        if (radius_vec.magnitude > distance)
        {
            Vector3 targetPosition = ground_planet.transform.position + radius_vec.normalized * distance;
            player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, 50f * Time.deltaTime);
        }
    }
}
