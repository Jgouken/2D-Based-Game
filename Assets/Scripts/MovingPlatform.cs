using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private float seconds;
    [SerializeField] private Cooldown cooldown;

    private bool toEnd = true;
    public float xspeed;
    public float yspeed;
    
    void Start()
    {
        if (start.position.x > end.position.y)
        {
            Debug.LogError("Start must be lower or equal to the x position of End.");
        }
        platform.tag = "Moving Platform";
        platform.transform.position = start.position;
        xspeed = Math.Abs(start.position.x - end.position.x) / ((seconds / 2) * 100);
        yspeed = Math.Abs(start.position.y - end.position.y) / ((seconds / 2) * 100);
        if (seconds <= 0) seconds = 0.1f;
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        if (!cooldown.IsCoolingDown)
        {
            if (toEnd)
            {
                platform.transform.position = new Vector3(platform.transform.position.x + xspeed, platform.transform.position.y - yspeed);
                if (platform.transform.position.x >= end.position.x)
                {
                    toEnd = false;
                    cooldown.StartCooldown();
                }
            }
            else
            {
                platform.transform.position = new Vector3(platform.transform.position.x - xspeed, platform.transform.position.y + yspeed);
                if (platform.transform.position.x <= start.position.x)
                {
                    toEnd = true;
                    cooldown.StartCooldown();
                }
            }
        }
    }
}
