using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    [SerializeField] private GameObject main_camera;
    [SerializeField] private Vector3 cam_offset;
    [SerializeField] private float cam_speed;
    private GameObject player;
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        Vector3 tempPos = Vector3.Lerp(main_camera.transform.position, player.transform.position + cam_offset, cam_speed);

        main_camera.transform.position = tempPos;

    }
}
