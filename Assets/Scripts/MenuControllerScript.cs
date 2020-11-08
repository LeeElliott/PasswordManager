using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControllerScript : MonoBehaviour {

	public void ViewListClicked()
    {
        // Initialise the list viewer
        SceneManager.LoadScene("ListViewer");
    }

    public void AddEntryClicked()
    {
        // Initialise new entry
        SceneManager.LoadScene("AddEntry");
    }
}
