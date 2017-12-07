using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidHUD : MonoBehaviour {

	[SerializeField]
	UnityEngine.UI.Image degrees90;
	[SerializeField]
	UnityEngine.UI.Text degrees90Text;
	[SerializeField]
	UnityEngine.UI.Image degrees360;
	[SerializeField]
	UnityEngine.UI.Text degrees360Text;

	// Use this for initialization
	void Start () {
		
	}

	void setEnable( UnityEngine.UI.Image image, UnityEngine.UI.Text text, bool enable ){
		if( enable ){
			image.color = Color.green;
			text.color = Color.white;
		}
		else{
			image.color = Color.gray;
			text.color = Color.white;
		}
	}
	
	// Update is called once per frame
	void Update () {
		setEnable( degrees90, degrees90Text, BoidBehaviour.considerAngleOfView );
		setEnable( degrees360, degrees360Text, !BoidBehaviour.considerAngleOfView );
	}
}
