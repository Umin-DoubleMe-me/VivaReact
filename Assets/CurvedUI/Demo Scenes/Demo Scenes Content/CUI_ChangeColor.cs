using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace CurvedUI
{
	public class CUI_ChangeColor : MonoBehaviour
	{
	
		public void ChangeColorToBlue()
		{
			Debug.Log("Umin Down");
			this.GetComponent<Renderer>().material.color = Color.blue;
		}
	
		public void ChangeColorToCyan()
		{
			Debug.Log("Umin Hover");
			this.GetComponent<Renderer>().material.color = Color.cyan;
		}
	
		public void ChangeColorToWhite()
		{
			Debug.Log("Umin Exit");
			this.GetComponent<Renderer>().material.color = Color.white;
		}
	

	}
}

