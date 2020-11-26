using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ViewerScript : MonoBehaviour
{
    SQLiteFunctionality sqlite;

    // Containers for sections
    public Transform listView;
    public Transform itemView;
    public Transform changePass;
    public Transform newItem;

    // List view objects
    public Transform scrollView;
    public GameObject itemButton;

    // Item view variables
    int currentID;
    public Text siteText;
    public Text userText;
    public Text passText;

    // Password creation objects
    public Transform messageBox;
    public Toggle specialsToggle;
    public InputField suggestionInputField;
    public Slider minSlider;
    public Slider maxSlider;
    public Text minText;
    public Text maxText;
    public Text messageText;
    public Text proposedText;
    public Slider strengthSlider;
    public int score;

    // New item objects
    public InputField siteInputField;
    public InputField usernameInputField;
    public InputField passwordInputField;

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
    public char[] alphabet = new char[95]{
        (char)0x5B, (char)0x38, (char)0x3B, (char)0x21, (char)0x2F, (char)0x3A, (char)0x47, (char)0x26, (char)0x67, (char)0x20, (char)0x3F,
        (char)0x7D, (char)0x2D, (char)0x48, (char)0x6D, (char)0x36, (char)0x75, (char)0x33, (char)0x31, (char)0x56, (char)0x5D, (char)0x59,
        (char)0x73, (char)0x58, (char)0x25, (char)0x62, (char)0x7B, (char)0x64, (char)0x69, (char)0x7E, (char)0x77, (char)0x6C, (char)0x61,
        (char)0x50, (char)0x34, (char)0x46, (char)0x74, (char)0x63, (char)0x35, (char)0x39, (char)0x42, (char)0x4F, (char)0x5E, (char)0x52,
        (char)0x79, (char)0x6F, (char)0x28, (char)0x32, (char)0x2E, (char)0x53, (char)0x54, (char)0x4C, (char)0x3D, (char)0x6E, (char)0x37,
        (char)0x51, (char)0x4B, (char)0x65, (char)0x68, (char)0x4D, (char)0x7A, (char)0x30, (char)0x7C, (char)0x66, (char)0x2A, (char)0x71,
        (char)0x43, (char)0x23, (char)0x3E, (char)0x45, (char)0x4A, (char)0x27, (char)0x2B, (char)0x72, (char)0x40, (char)0x44, (char)0x2C,
        (char)0x5A, (char)0x70, (char)0x5F, (char)0x55, (char)0x41, (char)0x22, (char)0x5C, (char)0x76, (char)0x78, (char)0x3C, (char)0x4E,
        (char)0x57, (char)0x6B, (char)0xA3, (char)0x49, (char)0x29, (char)0x6A, (char)0x36
    };

    /* Ordered list of special characters
     * 
     * 
     * 
     */
    public char[] specials = new char[] {
        (char)0x20, (char)0x21, (char)0x22, (char)0x23, (char)0x24, (char)0x25, (char)0x26, (char)0x27, (char)0x28, (char)0x29, (char)0x2A,
        (char)0x2B, (char)0x2C, (char)0x2D, (char)0x2E, (char)0x2F, (char)0x3A, (char)0x3B, (char)0x3C, (char)0x3D, (char)0x3E, (char)0x3F,
        (char)0x40, (char)0x5B, (char)0x5C, (char)0x5D, (char)0x5E, (char)0x5F, (char)0x7B, (char)0x7C, (char)0x7D, (char)0x7E, (char)0xA3
    };

    // Use this for initialization
    void Start()
    {
        sqlite = GetComponent<SQLiteFunctionality>();
        sqlite.OnStart();

        listView.gameObject.SetActive(true);
        itemView.gameObject.SetActive(false);
        changePass.gameObject.SetActive(false);
        newItem.gameObject.SetActive(false);
        messageBox.gameObject.SetActive(false);

        FillList();
    }
	
	// Update is called once per frame
	void Update()
    {
        minText.text = minSlider.value.ToString();
        maxText.text = maxSlider.value.ToString();
    }

    private void FillList()
    {
        // Fill list
        for (int i = 0; i < sqlite.GetRowCount(); i++)
        {
            GameObject button;

            // Create new instances of our prefab button
            button = (GameObject)Instantiate(itemButton, scrollView.transform);
            button.transform.SetParent(scrollView);

            // Change text of button
            button.transform.GetChild(0).GetComponent<Text>().text = DecryptData(sqlite.LoadList(i)[0], DecodeKey(sqlite.LoadList(i)[1]));
            button.transform.GetChild(1).GetComponent<Text>().text = DecryptData(sqlite.LoadList(i)[2], DecodeKey(sqlite.LoadList(i)[3]));

            button.GetComponent<ListButtonScript>().setID(i);
        }
    }

    // Strength check
    private int StrengthCheck(string pass, bool spec)
    {
        score = 0;
        float complexity = 0;

        // Length and complexity (calculate number of permutations)
        if (spec)
        {
            complexity = Mathf.Pow(96, pass.Length);
        }
        else
        {
            complexity = Mathf.Pow(62, pass.Length);
        }

        // Score the complexity
        float hoursTaken = ((complexity / 2000000000) / 60) / 60;
        
        if (hoursTaken >= 336)
        {
            score += 100;
        }
        else if (hoursTaken <= 236)
        {
            score += 0;
        }
        else
        {
            score += (int)hoursTaken - 236;
        }

        // Dictionary
        // L33t check
        // Close key
        // Repetition
        // Sequence
        // Date/time

        return score;
    }

    // Button Events
    public void MenuClicked()
    {
        // Return to menu
        SceneManager.LoadScene("Menu");
    }

    public void NewEntryClicked()
    {
        listView.gameObject.SetActive(false);
        newItem.gameObject.SetActive(true);
    }

    public void SelectionClicked(int i)
    {
        listView.gameObject.SetActive(false);
        itemView.gameObject.SetActive(true);

        // Store ID
        currentID = i;

        // Fill boxes
        siteText.text = sqlite.LookupSingle(currentID, 0);
        userText.text = sqlite.LookupSingle(currentID, 2);
        passText.text = sqlite.LookupSingle(currentID, 4);
    }

    public void BackClicked()
    {
        // Return to list view
        SceneManager.LoadScene("ListViewer");
        FillList();
    }

    public void ChangeClicked()
    {
        // Open floating password generation panel 
        itemView.gameObject.SetActive(false);
        changePass.gameObject.SetActive(true);
    }

    public void GenerateClicked()
    {
        // Extract text from input field
        string enterredString = suggestionInputField.text;
        string newPassword = "";

        if (enterredString != "")
        {
            for (int i = 0; i < enterredString.Length; i++)
            {
                // Only accept the first char from every word
                if (i == 0 || enterredString[i - 1] == ' ')
                {
                    // Create string to check for numeral words
                    string partial = "";
                    
                    for (int j = 0; j < enterredString.Length - i; j++)
                    {
                        if (enterredString[i + j] != ' ')
                        {
                            partial += char.ToLower(enterredString[i + j]);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (partial == "one" || partial == "won")
                    {
                        newPassword += '1';
                    }
                    else if (partial == "to" || partial == "too" || partial == "two")
                    {
                        newPassword += '2';
                    }
                    else if (partial == "three")
                    {
                        newPassword += '3';
                    }
                    else if (partial == "for" || partial == "fore" || partial == "four")
                    {
                        newPassword += '4';
                    }
                    else if (partial == "five")
                    {
                        newPassword += '5';
                    }
                    else if (partial == "six" || partial == "sex")
                    {
                        newPassword += '6';
                    }
                    else if (partial == "seven")
                    {
                        newPassword += '7';
                    }
                    else if (partial == "ate" || partial == "eight")
                    {
                        newPassword += '8';
                    }
                    else if (partial == "nein" || partial == "nine")
                    {
                        newPassword += '9';
                    }
                    else if (partial == "zero" || partial == "nought" || partial == "not" || partial == "knot")
                    {
                        newPassword += '0';
                    }
                    else
                    {
                        int switchChance = Random.Range(0, 4);

                        switch (switchChance)
                        {
                            // Special character swap
                            case 3:
                                if (enterredString[i] == 'a' || enterredString[i] == 'A')
                                {
                                    newPassword += '@';
                                }
                                else if (enterredString[i] == 'e' || enterredString[i] == 'E')
                                {
                                    newPassword += '£';
                                }
                                else if (enterredString[i] == 'i' || enterredString[i] == 'I')
                                {
                                    newPassword += '!';
                                }
                                else if (enterredString[i] == 's' || enterredString[i] == 'S')
                                {
                                    newPassword += '$';
                                }
                                else if (enterredString[i] == 'v' || enterredString[i] == 'V')
                                {
                                    newPassword += '^';
                                }
                                else if (enterredString[i] == 'x' || enterredString[i] == 'X')
                                {
                                    newPassword += '*';
                                }
                                else
                                {
                                    newPassword += enterredString[i];
                                }
                                break;
                            // Numerical char swap
                            case 2:
                                if (enterredString[i] == 'i' || enterredString[i] == 'I')
                                {
                                    newPassword += '1';
                                }
                                else if (enterredString[i] == 'e' || enterredString[i] == 'E')
                                {
                                    newPassword += '3';
                                }
                                else if (enterredString[i] == 'a' || enterredString[i] == 'A')
                                {
                                    newPassword += '4';
                                }
                                else if (enterredString[i] == 's' || enterredString[i] == 'S')
                                {
                                    newPassword += '5';
                                }
                                else if (enterredString[i] == 'o' || enterredString[i] == 'O')
                                {
                                    newPassword += '0';
                                }
                                else
                                {
                                    newPassword += enterredString[i];
                                }
                                break;
                            // Change case
                            case 1:
                                if (char.IsUpper(enterredString[i]))
                                {
                                    newPassword += char.ToLower(enterredString[i]);
                                }
                                else
                                {
                                    newPassword += char.ToUpper(enterredString[i]);
                                }
                                break;
                            // Default is no modifier
                            default:
                                newPassword += enterredString[i];
                                break;
                        }
                    }
                }
            }

            if (newPassword.Length < (int)minSlider.value)
            {
                // Too short display warning
                newPassword = "ERROR";
                messageBox.gameObject.SetActive(true);
                messageText.text = "Created password does not meet minimum length requirement.";
            }
            else if (newPassword.Length > (int)maxSlider.value)
            {
                // Too long display warning
                messageBox.gameObject.SetActive(true);
                messageText.text = "Created password exceeds length as set above.";
            }
            else
            {
                // Hide the message box
                messageBox.gameObject.SetActive(false);

                // Check to ensure symbols and numbers
                while (!ContainsNumbers(newPassword))
                {
                    AddNumbers(newPassword);
                }
                while (!ContainsSpecials(newPassword))
                {
                    AddSpecials(newPassword);
                }
            }
        }
        else
        {
            int passLength = Random.Range((int)minSlider.value, (int)maxSlider.value + 1);

            for (int i = 0; i < passLength; i++)
            {
                char nextChar = new char();
                if (!specialsToggle.isOn)
                {
                    bool accepted = false;

                    while (!accepted)
                    {
                        nextChar = alphabet[Random.Range(0, 95)];
                        accepted = true;

                        for (int j = 0; j < specials.Length; j++)
                        {
                            if (nextChar == specials[j])
                            {
                                accepted = false;
                            }
                        }
                    }
                }
                else
                {
                    nextChar = alphabet[Random.Range(0, 95)];
                }

                newPassword += nextChar;
            }
        }

        proposedText.text = newPassword;

        // Calculate password strength
        int strength = StrengthCheck(newPassword, specialsToggle.isOn);

        // Update strength meter
        strengthSlider.value = strength;
    }

    public void AcceptClicked()
    {
        // Store the generated password
        string site = sqlite.LookupSingle(currentID, 0);
        string user = sqlite.LookupSingle(currentID, 2);
        sqlite.UpdateEntry(site, user, proposedText.text);

        // Switch back to item view
        itemView.gameObject.SetActive(true);
        changePass.gameObject.SetActive(false);
    }

    public void DeleteClicked()
    {
        // Delete entire record from stored data
        
        string currentSite = DecryptData(sqlite.LookupSingle(currentID, 0), DecodeKey(sqlite.LookupSingle(currentID, 1)));
        string currentAccount = DecryptData(sqlite.LookupSingle(currentID, 2), DecodeKey(sqlite.LookupSingle(currentID, 3)));
        sqlite.RemoveEntry(currentSite, currentAccount);
    }

    public void AddEntryClicked()
    {
        // Collect input data from input fields
        char[] site = siteInputField.text.ToCharArray();
        char[] username = usernameInputField.text.ToCharArray();
        char[] password = passwordInputField.text.ToCharArray();

        // Encryption
        List<int> siteKey = new List<int>();
        List<int> usernameKey = new List<int>();
        List<int> passwordKey = new List<int>();

        for (int i = 0; i < 10; i++)
        {
            siteKey.Add(Random.Range(0, 95));
            usernameKey.Add(Random.Range(0, 95));
            passwordKey.Add(Random.Range(0, 95));
        }

        for (int k = 0; k < passwordKey.Count; k++)
        {
            for (int j = 0; j < site.Length; j++)
            {
                // Site encryption
                for (int i = 0; i < alphabet.Length; i++)
                {
                    if (site[j] == alphabet[i])
                    {
                        // Shift along alphabet array
                        if (i + siteKey[k] < alphabet.Length)
                        {
                            // Store char from new position
                            site[j] = (alphabet[i + siteKey[k]]);
                        }
                        else
                        {
                            int modifier = (i + siteKey[k]) - alphabet.Length;

                            site[j] = (alphabet[modifier]);
                        }
                    }
                }                
            }
            for (int j = 0; j < username.Length; j++)
            {
                // username encryption
                for (int i = 0; i < alphabet.Length; i++)
                {
                    if (username[j] == alphabet[i])
                    {
                        // Shift along alphabet array
                        if (i + usernameKey[k] < alphabet.Length)
                        {
                            // Store char from new position
                            username[j] = (alphabet[i + usernameKey[k]]);
                        }
                        else
                        {
                            int modifier = (i + usernameKey[k]) - alphabet.Length;

                            username[j] = (alphabet[modifier]);
                        }
                    }
                }
            }
            for (int j = 0; j < password.Length; j++)
            {
                // Password encryption
                for (int i = 0; i < alphabet.Length; i++)
                {
                    if (password[j] == alphabet[i])
                    {
                        // Shift along alphabet array
                        if (i + passwordKey[k] < alphabet.Length)
                        {
                            // Store char from new position
                            password[j] = (alphabet[i + passwordKey[k]]);
                        }
                        else
                        {
                            int modifier = (i + passwordKey[k]) - alphabet.Length;

                            password[j] = (alphabet[modifier]);
                        }
                    }
                }
            }
        }

        // Conversion of key data
        string sKey = "";   string uKey = "";   string pKey = "";
        for (int i = 0; i < siteKey.Count; i++)
        {
            sKey += alphabet[siteKey[i]];
            uKey += alphabet[usernameKey[i]];
            pKey += alphabet[passwordKey[i]];
        }

        // Storage of encrypted data
        // Delete entire record from stored data
        string debugString = "Site: " + site + " Name: " + username + " Pass: " + password;
        Debug.Log (debugString);
        debugString = "Site: " + new string(site)+ " Name: " + new string(username) + " Pass: " + new string(password);
        Debug.Log(debugString);
        //sqlite.AddNewEntry(new string(site), sKey, new string(username), uKey, new string(password), pKey);
    }



    private void AddNumbers(string s)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == 'i' || s[i] == 'I')
            {
                s[i].Equals('1');
            }
            else if (s[i] == 'e' || s[i] == 'E')
            {
                s[i].Equals('3');
            }
            else if (s[i] == 'a' || s[i] == 'A')
            {
                s[i].Equals('4');
            }
            else if (s[i] == 's' || s[i] == 'S')
            {
                s[i].Equals('5');
            }
            else if (s[i] == 'o' || s[i] == 'O')
            {
                s[i].Equals('0');
            }
        }
    }

    private void AddSpecials(string s)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == 'a' || s[i] == 'A')
            {
                s[i].Equals('@');
                break;
            }
            else if (s[i] == 'e' || s[i] == 'E')
            {
                s[i].Equals('£');
                break;
            }
            else if (s[i] == 'i' || s[i] == 'I')
            {
                s[i].Equals('!');
                break;
            }
            else if (s[i] == 's' || s[i] == 'S')
            {
                s[i].Equals('$');
                break;
            }
            else if (s[i] == 'v' || s[i] == 'V')
            {
                s[i].Equals('^');
                break;
            }
            else if (s[i] == 'x' || s[i] == 'X')
            {
                s[i].Equals('*');
                break;
            }
        }
    }

    private bool ContainsNumbers(string s)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if (char.IsDigit(s[i]))
            {
                return true;
            }
        }

        return false;
    }

    private bool ContainsSpecials(string s)
    {
        for (int i = 0; i < s.Length; i++)
        {
            for (int j = 0; j < specials.Length; j++)
            {
                if (s[i] == specials[j])
                {
                    return true;
                }
            }
        }

        return false;
    }

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
            dKey.Add((int)key[i]);
        }

        return dKey;
    }
    private string EncryptData(string data, List<int> key)
    {
        int dataLength = data.Length;
        int iterations = key.Count;
        string eData = "";

        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < dataLength; j++)
            {
                for (int k = 0; k < alphabet.Length; k++)
                {
                    if (alphabet[k] == data[j])
                    {
                        if (k + key[i] < (alphabet.Length - 1))
                        {
                            eData += alphabet[k + key[i]];
                        }
                        else
                        {
                            eData += alphabet[k + key[i] - alphabet.Length];
                        }
                    }
                }
            }
        }

        return eData;
    }
    private string DecryptData(string data, List<int> key)
    {
        List<int> dKey = key;
        dKey.Reverse();

        int dataLength = data.Length;
        int iterations = key.Count;
        string dData = "";

        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < dataLength; j++)
            {
                for (int k = 0; k < alphabet.Length; k++)
                {
                    if (alphabet[k] == data[j])
                    {
                        if (k - key[i] > 0)
                        {
                            dData += alphabet[k - key[i]];
                        }
                        else
                        {
                            dData += alphabet[alphabet.Length + (k - key[i])];
                        }
                    }
                }
            }
        }

        return dData;
    }
}
