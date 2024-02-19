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
    public float boundLeftSide;
    public bool selected = false;
    public bool grabbed = false;

    // Start is called before the first frame update
    void Start() {
        if (!boundary) boundary = transform.parent.gameObject.GetComponent<CompositeCollider2D>();
        boundLeftSide = boundary.bounds.size.y;
    }
    void OnMouseOver()
    {
        selected = true;
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
            selected = false;
        }
    }

    void Update()
    {
        if (grabbed) gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
        if (!Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftShift)) { grabbed = false; selected = false; new Vector2(0, 0); }
        if (Input.GetMouseButton(0) && grabbed && !Input.GetKey(KeyCode.LeftShift))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x < boundary.GetComponent<Transform>().localPosition.x + (boundary.bounds.size.x / 2) - (boundary.GetComponent<BoxCollider2D>().bounds.size.x / 2)) transform.position = new Vector3(mousePos.x + positionOffset.x, mousePos.y + positionOffset.y);
        }
    }
}
