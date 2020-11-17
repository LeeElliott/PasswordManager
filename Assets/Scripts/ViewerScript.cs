using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ViewerScript : MonoBehaviour
{
    // Containers for sections
    public Transform listView;
    public Transform itemView;
    public Transform changePass;
    public Transform newItem;

    // Password creation objects
    public Toggle specialsToggle;
    public InputField suggestionInputField;
    public Slider minSlider;
    public Slider maxSlider;
    public Text minText;
    public Text maxText;
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
        listView.gameObject.SetActive(true);
        itemView.gameObject.SetActive(false);
        changePass.gameObject.SetActive(false);
        newItem.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update()
    {
        minText.text = minSlider.value.ToString();
        maxText.text = maxSlider.value.ToString();
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

    public void SelectionClicked()
    {
        listView.gameObject.SetActive(false);
        itemView.gameObject.SetActive(true);
    }

    public void BackClicked()
    {
        // Return to list view
        SceneManager.LoadScene("ListViewer");
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
                if (enterredString[i] == ' ')
                {
                    newPassword += char.ToUpper(enterredString[i + 1]);
                    i++;
                }
                else if (enterredString[i] == 'e' && Random.Range(0, 3) == 1)       // Switch a portion of the E's
                {
                    newPassword += '3';
                }
                else if (enterredString[i] == 'o' && Random.Range(0, 3) == 1)       // Switch a portion of the O's
                {
                    newPassword += '0';
                }
                else if (enterredString[i] == 'i' && Random.Range(0, 3) == 1)       // Switch a portion of the I's
                {
                    newPassword += '1';
                }
                else if (enterredString[i] == 's' && Random.Range(0, 3) == 1)       // Switch a portion of the S's
                {
                    newPassword += '5';
                }
                else if (enterredString[i] == 'a' && Random.Range(0, 3) == 1)       // Switch a portion of the A's
                {
                    newPassword += '4';
                }
                else
                {
                    newPassword += enterredString[i];
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


        // Switch back to item view
        itemView.gameObject.SetActive(true);
        changePass.gameObject.SetActive(false);
    }

    public void DeleteClicked()
    {
        // Delete entire record from stored data

    }

    public void AddEntryClicked()
    {
        // Collect input data from input fields
        string site = siteInputField.text;
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        // Encryption
        List<int> siteKey = new List<int>();
        List<int> usernameKey = new List<int>();
        List<int> passwordKey = new List<int>();

        for (int i = 0; i < 10; i++)
        {
            siteKey.Add(Random.Range(0, 96));
            usernameKey.Add(Random.Range(0, 96));
            passwordKey.Add(Random.Range(0, 96));
        }

        for (int k = 0; k < passwordKey.Count; k++)
        {
            for (int j = 0; j < password.Length; j++)
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
                            site[j].Equals(alphabet[i + siteKey[k]]);
                        }
                        else
                        {
                            int modifier = (i + siteKey[k]) - alphabet.Length;

                            site[j].Equals(alphabet[modifier]);
                        }
                    }
                }
                // username encryption
                for (int i = 0; i < alphabet.Length; i++)
                {
                    if (username[j] == alphabet[i])
                    {
                        // Shift along alphabet array
                        if (i + usernameKey[k] < alphabet.Length)
                        {
                            // Store char from new position
                            username[j].Equals(alphabet[i + usernameKey[k]]);
                        }
                        else
                        {
                            int modifier = (i + usernameKey[k]) - alphabet.Length;

                            username[j].Equals(alphabet[modifier]);
                        }
                    }
                }
                // Password encryption
                for (int i = 0; i < alphabet.Length; i++)
                {
                    if (password[j] == alphabet[i])
                    {
                        // Shift along alphabet array
                        if (i + passwordKey[k] < alphabet.Length)
                        {
                            // Store char from new position
                            password[j].Equals(alphabet[i + passwordKey[k]]);
                        }
                        else
                        {
                            int modifier = (i + passwordKey[k]) - alphabet.Length;

                            password[j].Equals(alphabet[modifier]);
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

    }
}
