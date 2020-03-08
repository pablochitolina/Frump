using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {

	PersonagemScript ps;
	public GameObject personagem;

	// Use this for initialization
	void Start () {
		ps = personagem.GetComponent<PersonagemScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ButtonClick (float theValue) {
		Debug.Log ("PrintFloat is called with a value of " + theValue);
		if (theValue == 1) {
			ps.Again();
		} else if (theValue == 2) {
			ps.Ranking();
		} else if (theValue == 3) {
			ps.Exit();
		}else if (theValue == 4) {
			ps.Leader();
		} else if (theValue == 5) {
			ps.Achiev();
		}else if (theValue == 6) {
			ps.YesConnect();
		} else if (theValue == 7) {
			ps.NoConnect();
		}
		//ranking == 2
		//again == 1
		//exit == 3
		//leader == 4
		//achiev == 5
		// yes == 6
		//no == 7
	}

}
