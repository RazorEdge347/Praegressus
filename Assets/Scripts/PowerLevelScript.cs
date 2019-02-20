using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLevelScript : MonoBehaviour {

	TextMesh PowerLvl;
	public int PWLVL = 0;
    

	// Use this for initialization
	void Start () {
		PowerLvl = GetComponent<TextMesh> ();
	}

	public bool setPower(int val){
        if (val > 0)
        {
            if (PWLVL < 5)
            {
                
                PWLVL += val;
                return true;
            }
        }
        else
        {
            if (PWLVL > 0)
            {
                PWLVL += val;
                return true;
            }

        }
        return false;

	}



	// Update is called once per frame
	void Update () {
		PWLVL = Mathf.Clamp (PWLVL, 0, 10);
		PowerLvl.text = PWLVL.ToString ();
	}
}
