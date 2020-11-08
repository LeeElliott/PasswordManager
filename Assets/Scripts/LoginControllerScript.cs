using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginControllerScript : MonoBehaviour
{
    public InputField inputField;

    public void LoginClicked()
    {
        string enterredPassword = inputField.text;

        if (PasswordCorrect(enterredPassword))
        {
            // Login passed initialise menu scene
            SceneManager.LoadScene("Menu");
        }
    }

    bool PasswordCorrect(string a)
    {
        // Look up stored password
        string storedPassword = "NotSet";

        // Input matches stored password
        if (storedPassword == a)
        {
            // Check passed
            return true;
        }
        else if ( storedPassword == "NotSet")
        {
            // Set stored password


            // Check passed
            return true;
        }

        // No match, check failed
        return false;
    }
}
