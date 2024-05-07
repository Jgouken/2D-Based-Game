using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardArrows : MonoBehaviour
{
    public string keyPasscode = "";
    [SerializeField] private Cooldown cooldown;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !cooldown.IsCoolingDown)
        {
            player.GetComponent<BardMovement>().enabled = false;
            if (keyPasscode.Length < 10)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    keyPasscode = keyPasscode + "2";
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    keyPasscode = keyPasscode + "1";
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    keyPasscode = keyPasscode + "4";
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    keyPasscode = keyPasscode + "3";
                }
            }
        }
        else
        {
            player.GetComponent<BardMovement>().enabled = true;
            if (keyPasscode != "") { // Such a weird way of doing it, I hate it
                switch (keyPasscode) {
                    case "231": {
                        Debug.Log("Rain");
                        break;
                    }
                    case "234": {
                        Debug.Log("Sun");
                        break;
                    }
                    case "231432": {
                        Debug.Log("Time");
                        break;
                    }
                    case "413": {
                        Debug.Log("Entrance");
                        break;
                    }
                }
                keyPasscode = "";
            }
        }
    }
}
