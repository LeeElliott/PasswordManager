using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddItemControllerScript : MonoBehaviour
{
    public InputField purposeField;
    public InputField usernameField;
    public InputField passwordField;

    public void AddEntryClicked()
    {
        // Take string data from three input fields
        
    }

    public void MenuClicked()
    {
        // Initialise the list viewer
        SceneManager.LoadScene("Menu");
    }
}
