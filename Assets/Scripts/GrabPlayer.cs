using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GrabPlayer : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        if (player) gameObject.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        else Debug.LogError("GET RID OF ME. PLEASE.");
    }
}
