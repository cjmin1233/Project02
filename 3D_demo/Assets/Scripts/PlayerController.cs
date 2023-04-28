using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private LineRenderer lr;

    [SerializeField] private float maxDrag = 100f;

    Vector3 dragStartPos;
    Vector3 draggingPos;
    Vector3 dragReleasePos;
    Touch touch;

    Vector2 last_joystick_input;
    [SerializeField] private float dir_length = 10f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        rb.useGravity = false;
    }
    /*private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                DragStart();
            }
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Dragging();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                DragRelease();
            }
        }
    }*/
    private void FixedUpdate()
    {
        GroundPlanet.Instance.Attract(transform);
        GroundPlanet.Instance.PlayerOnPlanet();
    }

    private void DragStart()
    {
        dragStartPos = touch.position;
        dragStartPos.z = dragStartPos.y;
        dragStartPos.y = 0f;
        lr.positionCount = 1;
        lr.SetPosition(0, transform.position);
    }

    private void Dragging()
    {
        draggingPos = touch.position;
        draggingPos.z = draggingPos.y;
        draggingPos.y = 0f;
        Vector3 diff_vec = draggingPos - dragStartPos;
        diff_vec.x /= Screen.width / 2;
        diff_vec.z /= Screen.height / 2;
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + diff_vec.z * transform.forward + diff_vec.x * transform.right);
    }
    private void DragRelease()
    {
        lr.positionCount = 0;

        dragReleasePos = touch.position;
        dragReleasePos.z = dragReleasePos.y;
        dragReleasePos.y = 0f;
        Vector3 force = dragReleasePos - dragStartPos;
        force.x /= Screen.width;
        force.z /= Screen.height;
        //Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;
        Vector3 clampedForce = maxDrag * force;
        rb.AddRelativeForce(Vector3.forward * clampedForce.z + Vector3.right * clampedForce.x, ForceMode.Impulse);
    }
    public void DragControl(Vector2 joystick_input)
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lr.positionCount = 1;
                lr.SetPosition(0, Vector3.zero);
            }
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (joystick_input.y < 0f) joystick_input.y = 0f;
                lr.positionCount = 2;
                lr.SetPosition(0, Vector3.zero);
                lr.SetPosition(1, dir_length * (joystick_input.y * Vector3.forward + joystick_input.x * Vector3.right));

                last_joystick_input = joystick_input;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                lr.positionCount = 0;
                Vector2 clampedForce = maxDrag * last_joystick_input;
                print(clampedForce.magnitude);
                rb.AddRelativeForce(Vector3.forward * clampedForce.y + Vector3.right * clampedForce.x, ForceMode.Impulse);
            }
        }
    }
}
