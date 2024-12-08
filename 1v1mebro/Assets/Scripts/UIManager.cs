using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject uiCanvas; // Reference to the UI canvas

    // Start is called before the first frame update
    void Start()
    {
        if (uiCanvas == null)
        {
            Debug.LogError("UI Canvas is not assigned in the UIManager.");
        }
        else
        {
            uiCanvas.SetActive(false); // Ensure the canvas is initially hidden
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiCanvas != null)
            {
                uiCanvas.SetActive(!uiCanvas.activeSelf); // Toggle the canvas visibility
            }
        }
    }
}