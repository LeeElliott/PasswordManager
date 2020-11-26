using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListButtonScript : MonoBehaviour
{
    private int myID;
    

	// Use this for initialization
	void Start ()
    {
        // Get controller
        GameObject control = GameObject.Find("Controller");

        // Add listener
        UnityEngine.UI.Button buttonRef = GetComponent<UnityEngine.UI.Button>();
        buttonRef.onClick.AddListener(delegate { control.GetComponent<ViewerScript>().SelectionClicked(myID); });
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void setID(int i) { myID = i; }
    public int getID() { return myID; }
}
