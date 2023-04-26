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
        Vector3 tempPos = Vector3.Lerp(main_camera.transform.position, player.transform.position + player.transform.up * cam_offset.y + player.transform.forward * cam_offset.z, cam_speed);
        // player.transform.rotation �������� x-axis 45�� ��ŭ ȸ���� ���ʹϾ� ����
        Quaternion rotateQuaternion = Quaternion.Lerp(main_camera.transform.rotation, player.transform.rotation * Quaternion.AngleAxis(45f, player.transform.right), cam_speed);

        // MainCamera�� transform.rotation�� ȸ�� ����
        //Camera.main.transform.rotation = player.transform.rotation * rotateQuaternion;

        main_camera.transform.SetPositionAndRotation(tempPos, rotateQuaternion);
        float radius_R = 5.5f;
        float distance_R = Vector3.Distance(player.transform.position, Vector3.zero);
        if (distance_R > radius_R)
        {
            player.transform.position = (player.transform.position - Vector3.zero).normalized * radius_R;
        }
        player.transform.up = (player.transform.position - Vector3.zero).normalized;
        player.GetComponent<Rigidbody>().velocity = player.transform.forward * 3f;
    }
}
