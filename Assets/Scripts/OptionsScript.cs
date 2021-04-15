using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsScript : MonoBehaviour
{
    /* Randomized list of characters
     * [8;!/:G&g ?
     * }-Hm6u31V]Y
     * sX%b{di~wla
     * P4Ftc59BO^R
     * yo(2.STL=n7
     * QKehMz0|f*q
     * C#>EJ+r@D,
     * Zp_UA\vx<N
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
    public InputField inputField;
    public Transform menu;
    public Transform passChange;

    // Use this for initialization
    void Start ()
    {
        sqlite = GetComponent<SQLiteFunctionality>();
        sqlite.OnStart();

        menu.gameObject.SetActive(true);
        passChange.gameObject.SetActive(false);
    }

    // Button press responses
    public void ChangeClicked()
    {
        menu.gameObject.SetActive(false);
        passChange.gameObject.SetActive(true);
    }
    public void DestroyClicked()
    {
        sqlite.LastResort();
        SceneManager.LoadScene("Decoy");
    }
    public void SubmitClicked()
    {
        string input = inputField.text;
        string site = sqlite.LookupSingle(0, 0);
        string user = sqlite.LookupSingle(0, 2);
        List<int> key = DecodeKey(sqlite.LookupSingle(0, 5));

        sqlite.UpdateEntry(site, user, EncryptData(input, key));

    }
    public void CancelClicked()
    {
        menu.gameObject.SetActive(true);
        passChange.gameObject.SetActive(false);
    }
    public void MenuClicked()
    {
        SceneManager.LoadScene("Menu");
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
}
