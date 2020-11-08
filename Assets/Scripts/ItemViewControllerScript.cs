using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemViewControllerScript : MonoBehaviour {

	// Use this for initialization
	void Start()
    {
		
	}
	
	// Update is called once per frame
	void Update()
    {
		
	}

    public void MenuClicked()
    {
        SceneManager.LoadScene("Menu");
    }

    public void BackClicked()
    {
        SceneManager.LoadScene("ListViewer");
    }

    public void SubmitClicked()
    {
        // Store amended password 

    }

    public void DeleteClicked()
    {
        // Delete entire record from stored data

    }
}
