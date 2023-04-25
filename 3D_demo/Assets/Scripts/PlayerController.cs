using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private TextMeshProUGUI test_text;
    Vector2 diff_vec;
    [SerializeField] private float speed;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) diff_vec = touch.deltaPosition;
            else if (touch.phase == TouchPhase.Ended) diff_vec = touch.position - diff_vec;
            test_text.text = diff_vec.ToString();
            rb.velocity = new Vector3(diff_vec.x, 0f, diff_vec.y) * speed;
        }
    }
}
