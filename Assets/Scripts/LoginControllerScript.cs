using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginControllerScript : MonoBehaviour
{
    /* Randomized list of characters
     * [8;!/:G&g ?
     * }-Hm6u31V]Y
     * sX%b{di~wla
     * P4Ftc59BO^R
     * yo(2.STL=n7
     * QKehMz0|f*q
     * C#>EJ'+r@D,
     * Zp_UA"\vx<N
     * Wk£I)j$
     */
    public char[] alphabet = new char[93]{
        (char)0x5B, (char)0x38, (char)0x3B, (char)0x21, (char)0x2F, (char)0x3A, (char)0x47, (char)0x26, (char)0x67, (char)0x20, (char)0x3F,
        (char)0x7D, (char)0x2D, (char)0x48, (char)0x6D, (char)0x36, (char)0x75, (char)0x33, (char)0x31, (char)0x56, (char)0x5D, (char)0x59,
        (char)0x73, (char)0x58, (char)0x25, (char)0x62, (char)0x7B, (char)0x64, (char)0x69, (char)0x7E, (char)0x77, (char)0x6C, (char)0x61,
        (char)0x50, (char)0x34, (char)0x46, (char)0x74, (char)0x63, (char)0x35, (char)0x39, (char)0x42, (char)0x4F, (char)0x5E, (char)0x52,
        (char)0x79, (char)0x6F, (char)0x28, (char)0x32, (char)0x2E, (char)0x53, (char)0x54, (char)0x4C, (char)0x3D, (char)0x6E, (char)0x37,
        (char)0x51, (char)0x4B, (char)0x65, (char)0x68, (char)0x4D, (char)0x7A, (char)0x30, (char)0x7C, (char)0x66, (char)0x2A, (char)0x71,
        (char)0x43, (char)0x23, (char)0x3E, (char)0x45, (char)0x4A, (char)0x2B, (char)0x72, (char)0x40, (char)0x44, (char)0x2C,
        (char)0x5A, (char)0x70, (char)0x5F, (char)0x55, (char)0x41, (char)0x5C, (char)0x76, (char)0x78, (char)0x3C, (char)0x4E,
        (char)0x57, (char)0x6B, (char)0xA3, (char)0x49, (char)0x29, (char)0x6A, (char)0x36
    };

    SQLiteFunctionality sqlite;

    public InputField loginUserField;
    public InputField loginPasswordField;
    public InputField signupUserField;
    public InputField signupPasswordField;
    public Transform login;
    public Transform signup;
    public Transform warning;
    public Text warningText;
    public Toggle coverToggle;

    private float timeStart = -99;
    private int attemptCount = 3;
    private bool loginShow = false;
    private bool signupShow = false;

    void Start()
    {
        sqlite = GetComponent<SQLiteFunctionality>();
        sqlite.OnStart();
        warning.gameObject.SetActive(false);

        // Check if account exists
        if (sqlite.GetRowCount() > 0)
        {
            // Database has content
            login.gameObject.SetActive(true);
            signup.gameObject.SetActive(false);
        }
        else
        {
            // Database has no content
            login.gameObject.SetActive(false);
            signup.gameObject.SetActive(true);
        }

        coverToggle.onValueChanged.AddListener(delegate { ToggleClicked(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < timeStart + 5)
        {
            warning.gameObject.SetActive(true);
        }
        else
        {
            warning.gameObject.SetActive(false);
        }
    }

    public void LoginClicked()
    {
        string enterredUsername = loginUserField.text;
        string enterredPassword = loginPasswordField.text;

        if (PasswordCorrect(enterredUsername, enterredPassword))
        {
            // Login passed initialise menu scene
            SceneManager.LoadScene("Menu");
        }
        else
        {
            timeStart = Time.time;

            string warningString = "The entered username or password is incorrect. Please try again.";
            warningString += "\nAttempts remaining " + attemptCount + " / 3.";
            warningText.text = warningString;
            attemptCount--;
        }
    }

    public void SignupClicked()
    {
        List<int> accKey = new List<int>{ 1, 50, 7, 20, 11 };
        List<int> userKey = new List<int> { 0, 7, 1, 21, 7 };
        List<int> passKey = new List<int> { 0, 10, 3, 1, 9 };
        string accountName = EncryptData("MemorySaver", accKey);
        string enterredUsername = EncryptData(signupUserField.text, userKey);
        string enterredPassword = EncryptData(signupPasswordField.text, passKey);

        // Set stored password
        sqlite.AddNewEntry(accountName, EncodeKey(accKey), enterredUsername, EncodeKey(userKey), enterredPassword, EncodeKey(passKey));
        SceneManager.LoadScene("Menu");
    }

    bool PasswordCorrect(string user, string pass)
    {
        string storedUser = DecryptData(sqlite.LookupSingle(0, 2), DecodeKey(sqlite.LookupSingle(0, 3)));
        string storedPass = DecryptData(sqlite.LookupSingle(0, 4), DecodeKey(sqlite.LookupSingle(0, 5)));

        if (storedUser == user && storedPass == pass)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
     * Encryption of data
     */
    private string EncodeKey(List<int> key)
    {
        string eKey = "";

        for (int i = 0; i < key.Count; i++)
        {
            eKey += alphabet[key[i]];
        }

        return eKey;
    }
    private List<int> DecodeKey(string key)
    {
        List<int> dKey = new List<int>();

        for (int i = 0; i < key.Length; i++)
        {
            for (int j = 0; j < alphabet.Length; j++)
            {
                if (alphabet[j] == key[i])
                {
                    dKey.Add(j);
                }
            }
        }

        return dKey;
    }
    private string EncryptData(string data, List<int> key)
    {
        int dataLength = data.Length;
        int iterations = key.Count;
        char[] eData = data.ToCharArray();
        int alphabetLength = alphabet.Length - 1;

        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < dataLength; j++)
            {
                for (int k = 0; k < alphabet.Length; k++)
                {
                    if (alphabet[k] == eData[j])
                    {
                        eData[j] = alphabet[((k + key[i]) + alphabetLength) % alphabetLength];
                        break;
                    }
                }
            }
        }

        return new string(eData);
    }
    private string DecryptData(string data, List<int> key)
    {
        List<int> dKey = key;
        dKey.Reverse();

        int dataLength = data.Length;
        int iterations = key.Count;
        char[] dData = data.ToCharArray();
        int alphabetLength = alphabet.Length - 1;

        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < dataLength; j++)
            {
                for (int k = 0; k < alphabet.Length; k++)
                {
                    if (alphabet[k] == dData[j])
                    {
                        dData[j] = alphabet[((k - dKey[i]) + alphabetLength) % alphabetLength];
                        break;
                    }
                }
            }
        }

        return new string(dData);
    }    

    private void ToggleClicked()
    {
        // Cover or show password
        if (coverToggle.isOn)
        {
            loginPasswordField.GetComponent<InputField>().contentType = InputField.ContentType.Standard;
            signupPasswordField.GetComponent<InputField>().contentType = InputField.ContentType.Standard;
            loginPasswordField.ActivateInputField();
            loginPasswordField.ForceLabelUpdate();
            signupPasswordField.ActivateInputField();
            signupPasswordField.ForceLabelUpdate();
        }
        else
        {
            loginPasswordField.GetComponent<InputField>().contentType = InputField.ContentType.Password;
            signupPasswordField.GetComponent<InputField>().contentType = InputField.ContentType.Password;
            loginPasswordField.ActivateInputField();
            loginPasswordField.ForceLabelUpdate();
            signupPasswordField.ActivateInputField();
            signupPasswordField.ForceLabelUpdate();
        }
    }
}
