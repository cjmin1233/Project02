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
}
