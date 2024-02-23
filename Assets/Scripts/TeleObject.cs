using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class TeleObject : MonoBehaviour
{
    public CompositeCollider2D boundary;
    private Vector2 positionOffset;
    private Vector3 lastMousePos;
    public Vector2 mouseSpeed;
    public bool grabbed = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!boundary) boundary = transform.parent.gameObject.GetComponent<CompositeCollider2D>();
        lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            positionOffset = new Vector2(transform.position.x - mousePos.x, transform.position.y - mousePos.y);
            grabbed = true;
        }
    }

    void OnMouseExit()
    {
        if (!Input.GetMouseButton(0))
        {
            positionOffset = new Vector2(0, 0);
        }
    }

    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var objectRigidbody = gameObject.GetComponent<Rigidbody2D>();
        if (grabbed)
        {
            mouseSpeed = (mousePos - lastMousePos) / Time.deltaTime;
            objectRigidbody.velocity = new Vector3(0, 0);
            lastMousePos = mousePos;
        }
        if (!Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftShift)) { grabbed = false; objectRigidbody.velocity = mouseSpeed; }
        if (Input.GetMouseButton(0) && grabbed && !Input.GetKey(KeyCode.LeftShift)) transform.position = new Vector3(mousePos.x + positionOffset.x, mousePos.y + positionOffset.y);
        if (!grabbed) objectRigidbody.velocity = new Vector2(objectRigidbody.velocity.x, objectRigidbody.velocity.y * 0.5f);
    }

    void LateUpdate()
    {
        var rightEdge = 0.5000f - (gameObject.GetComponent<BoxCollider2D>().bounds.size.x / (boundary.bounds.size.x * 2));
        var leftEdge = (gameObject.GetComponent<BoxCollider2D>().bounds.size.x / (boundary.bounds.size.x * 2)) - 0.5000f;
        var topEdge = 0.5000f - (gameObject.GetComponent<BoxCollider2D>().bounds.size.y / (boundary.bounds.size.y * 2));
        var bottomEdge = (gameObject.GetComponent<BoxCollider2D>().bounds.size.y / (boundary.bounds.size.y * 2)) - 0.5000f;

        transform.localPosition = new Vector3(transform.localPosition.x > rightEdge ? rightEdge : transform.localPosition.x < leftEdge ? leftEdge : transform.localPosition.x, transform.localPosition.y > topEdge ? topEdge : transform.localPosition.y < bottomEdge ? bottomEdge : transform.localPosition.y);
    }
}
