using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ListViewerControllerScript : MonoBehaviour {

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
        // Initialise the list viewer
        SceneManager.LoadScene("Menu");
    }
}
