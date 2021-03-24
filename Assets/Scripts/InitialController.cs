using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialController : MonoBehaviour
{
    public Button button;

    // Animated sprites
    public Sprite spriteOne;

    private int setNumber;

    // Use this for initialization
    void Start ()
    {
        // Choose a set at random
        setNumber = Random.Range(0, 1);

        switch (setNumber)
        {
            default:
                button.GetComponent<Image>().sprite = spriteOne;
                break;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void ButtonClicked()
    {
        SceneManager.LoadScene("Login");
    }
}
