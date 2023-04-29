using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    [SerializeField] private GameObject main_camera;
    [SerializeField] private Vector3 cam_pos_offset;
    [SerializeField] private Quaternion cam_rot_offset;
    [SerializeField] private float cam_speed;

    private GameObject ground_planet;
    private GameObject player;
    private Transform cam_target_transform;

    Vector3 cam_target_pos;
    Quaternion cam_target_rot;

    public Joystick joystick;
    Vector2 joystick_input;

    [SerializeField] private float[] meteor_spawn_range;
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
        cam_target_transform = player.transform.Find("cam_target");
    }
    private void Update()
    {
        joystick_input.x = joystick.Horizontal;
        joystick_input.y = joystick.Vertical;
        player.GetComponent<PlayerController>().DragControl(joystick_input);
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) SpawnMeteor();
    }
    private void FixedUpdate()
    {
        PlayerCamera();
    }
    private void PlayerCamera()
    {
        cam_target_transform.SetLocalPositionAndRotation(cam_pos_offset, cam_rot_offset);

        cam_target_pos = Vector3.Lerp(main_camera.transform.position, cam_target_transform.position, cam_speed);
        cam_target_rot = Quaternion.Slerp(main_camera.transform.rotation, cam_target_transform.rotation, cam_speed);

        main_camera.transform.SetPositionAndRotation(cam_target_pos, cam_target_rot);
    }
    private void SpawnMeteor()
    {
        float ground_radius = ground_planet.transform.lossyScale.x * ground_planet.GetComponent<SphereCollider>().radius;
        float radius = Random.Range(meteor_spawn_range[0] * ground_radius, meteor_spawn_range[1] * ground_radius);
        float pi = Random.Range(0, 180f);
        float theta = Random.Range(0, 360f);
        Vector3 spawn_pos = ground_planet.transform.position + radius * new Vector3(Mathf.Cos(pi) * Mathf.Cos(theta), Mathf.Sin(pi), Mathf.Cos(pi) * Mathf.Sin(theta));

        var obj = ObjectPool.Instance.GetFromPool(0);
        obj.transform.position = spawn_pos;
        obj.SetActive(true);
    }
}
