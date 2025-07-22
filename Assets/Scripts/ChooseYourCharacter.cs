using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseYourCharacter : MonoBehaviour
{
    public GameObject wizardSelect;
    public GameObject bardSelect;
    public GameObject rogueSelect;

    public Transform mainTarget;
    public TextMeshProUGUI mainTargetName;
    public TextMeshProUGUI mainTargetDescription;

    public Transform leftTarget;
    public Transform rightTarget;
    public int selectedChar = 0; // 0: Wizard, 1: Rogue, 2: Bard

    public float duration = 2f;
    private float startTime;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedChar++;
            startTime = Time.time;
            if (selectedChar > 2)
            {
                selectedChar = 0; // Loop back to the first character
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedChar--;
            startTime = Time.time;
            if (selectedChar < 0)
            {
                selectedChar = 2; // Loop back to the last character
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse is over a character selection area
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (wizardSelect.GetComponent<Collider2D>().OverlapPoint(mousePosition))
            {
                selectedChar = 0;
                startTime = Time.time;
            }
            else if (bardSelect.GetComponent<Collider2D>().OverlapPoint(mousePosition))
            {
                selectedChar = 2;
                startTime = Time.time;
            }
            else if (rogueSelect.GetComponent<Collider2D>().OverlapPoint(mousePosition))
            {
                selectedChar = 1;
                startTime = Time.time;
            }
        }

        // Update the character selection visuals
        float timeProgress = (Time.time - startTime) / duration;
        switch (selectedChar)
        {
            case 0: // Wizard
                mainTargetName.text = "Wizard";
                mainTargetDescription.text = "The wizard is...";

                wizardSelect.transform.position = Vector3.Lerp(wizardSelect.transform.position, mainTarget.position, timeProgress);
                bardSelect.transform.position = Vector3.Lerp(bardSelect.transform.position, leftTarget.position, timeProgress);
                rogueSelect.transform.position = Vector3.Lerp(rogueSelect.transform.position, rightTarget.position, timeProgress);

                wizardSelect.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, timeProgress * 2f);
                bardSelect.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one, timeProgress * 2f);
                rogueSelect.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one, timeProgress * 2f);
                break;

            case 1: // Rogue
                mainTargetName.text = "Rogue";
                mainTargetDescription.text = "The rogue is...";

                wizardSelect.transform.position = Vector3.Lerp(wizardSelect.transform.position, leftTarget.position, timeProgress);
                bardSelect.transform.position = Vector3.Lerp(bardSelect.transform.position, rightTarget.position, timeProgress);
                rogueSelect.transform.position = Vector3.Lerp(rogueSelect.transform.position, mainTarget.position, timeProgress);

                wizardSelect.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one, timeProgress * 2f);
                bardSelect.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one, timeProgress * 2f);
                rogueSelect.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, timeProgress * 2f);
                break;

            case 2: // Bard
                mainTargetName.text = "Bard";
                mainTargetDescription.text = "The bard is...";

                wizardSelect.transform.position = Vector3.Lerp(wizardSelect.transform.position, rightTarget.position, timeProgress);
                bardSelect.transform.position = Vector3.Lerp(bardSelect.transform.position, mainTarget.position, timeProgress);
                rogueSelect.transform.position = Vector3.Lerp(rogueSelect.transform.position, leftTarget.position, timeProgress);

                wizardSelect.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one, timeProgress * 2f);
                bardSelect.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, timeProgress * 2f);
                rogueSelect.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one, timeProgress * 2f);
                break;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            Persist.selectedCharacter = selectedChar;
            SceneManager.LoadScene("TestingTilemap");
        }
    }
}
