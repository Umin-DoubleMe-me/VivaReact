using UnityEngine;

public class CurvedUIData : MonoBehaviour
{
	[HideInInspector] public RectTransform CanvasRectTrans;

	[SerializeField] public Transform BackImageGenTransform;
	[SerializeField] public Transform RightButtonGenTransform;
	[SerializeField] public Transform SideButtonGenTransform;

	[SerializeField] public GameObject ButtonPrefab;
	[SerializeField] public GameObject SideButtonPrefab;

	[SerializeField] public Material ButtonSelectedMat;
	public void Init()
	{
		CanvasRectTrans = GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
	}

	public void Clickddd()
	{
		Debug.Log("Umin On");
	}
}
