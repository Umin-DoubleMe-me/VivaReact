using Oculus.Interaction;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CurvedUIEventSetting : MonoBehaviour
{
	[SerializeField] public GraphicRaycaster _graphicRaycaster;
	[SerializeField] public PointableCanvasModule _pointableCanvasModule;

	// Start is called before the first frame update
	void Start()
    {
		ActiveSetting();
		StartCoroutine(ActiveCor());
	}

	private void ActiveSetting()
	{
		_graphicRaycaster.enabled = true;
		_pointableCanvasModule.enabled = true;
	}

	private IEnumerator ActiveCor()
	{
		int num = 0;
		while(num < 3)
		{
			yield return new WaitForSeconds(1.5f);
			ActiveSetting();
		}
	}
}
