using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private LineRenderer lr;

    [SerializeField] private float power = 5f;
    [SerializeField] private float maxDrag = 5f;

    Vector3 dragStartPos;
    Vector3 draggingPos;
    Vector3 dragReleasePos;
    Touch touch;
    GameObject player;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                DragStart();
            }
            if (touch.phase == TouchPhase.Moved || touch.phase==TouchPhase.Stationary)
            {
                Dragging();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                DragRelease();
            }
        }
    }

    private void DragStart()
    {
        dragStartPos = touch.position;
        dragStartPos.z = dragStartPos.y;
        dragStartPos.y = 0f;
        lr.positionCount = 1;
        lr.SetPosition(0, player.transform.position);
    }

    private void Dragging()
    {
        draggingPos = touch.position;
        draggingPos.z = draggingPos.y;
        draggingPos.y = 0f;
        Vector3 diff_vec = draggingPos - dragStartPos;
        print(diff_vec);
        diff_vec.x /= Screen.width / 2;
        diff_vec.z /= Screen.height / 2;
        lr.positionCount = 2;
        lr.SetPosition(0, player.transform.position);
        lr.SetPosition(1, player.transform.position + diff_vec);
    }
    private void DragRelease()
    {
        lr.positionCount = 0;

        Vector3 dragReleasePos = touch.position;
        dragReleasePos.z = dragReleasePos.y;
        dragReleasePos.y = 0f;
        Vector3 force = dragReleasePos - dragStartPos;
        Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;
        rb.AddForce(clampedForce, ForceMode.Impulse);
    }

}
