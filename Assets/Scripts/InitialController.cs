using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialController : MonoBehaviour
{
    public GameObject adBanner;
    public Transform popup;

    // Switching controllers
    public RuntimeAnimatorController banner1, banner2, banner3;

    private int setNumber;
    private float timeStart = -99;

    // Use this for initialization
    void Start ()
    {
        popup.gameObject.SetActive(false);

        // Choose a set at random
        setNumber = Random.Range(0, 3);

        switch (setNumber)
        {
            default:
                adBanner.GetComponent<Animator>().runtimeAnimatorController = banner1;
                break;
            case 1:
                adBanner.GetComponent<Animator>().runtimeAnimatorController = banner2;
                break;
            case 2:
                adBanner.GetComponent<Animator>().runtimeAnimatorController = banner3;
                break;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time < timeStart + 5)
        {
            popup.gameObject.SetActive(true);
        }
        else
        {
            popup.gameObject.SetActive(false);
        }
    }

    public void ButtonClicked()
    {
        SceneManager.LoadScene("Login");
    }

    public void TileClicked()
    {
        // Display popup for 5s
        timeStart = Time.time;
    }
}
