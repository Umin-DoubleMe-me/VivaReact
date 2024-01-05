using UnityEngine;
using UnityEngine.UI;

public class CurvedUIData : MonoBehaviour
{
	[HideInInspector] public RectTransform CanvasRectTrans;

	[Header("Part")]
	[SerializeField] public Transform BackImageGenTransform;
	[SerializeField] public Transform RightButtonGenTransform;
	[SerializeField] public Transform SideButtonGenTransform;

	[Header("ButtonPrefab")]
	[SerializeField] public GameObject ButtonPrefab;
	[SerializeField] public GameObject SideButtonPrefab;

	[Header("ButtonMaterial")]
	[SerializeField] public Material ButtonSelectedMat;

	[Header("Back Image Position")]
	[SerializeField] public Transform NewImageWaitUpPos1;
	[SerializeField] public Transform NewImageWaitUpPos2;
	[SerializeField] public Transform NewImageWaitDownPos1;
	[SerializeField] public Transform NewImageWaitDownPos2;
	[SerializeField] public Transform NewImageWaitLeftSidePos1;
	[SerializeField] public Transform NewImageWaitLeftSidePos2;
	[SerializeField] public Transform CurImagePos1;
	[SerializeField] public Transform CurImagePos2;

	[Header("Back Image Objects")]
	[SerializeField] public Image CurImage_1;
	[SerializeField] public Image CurImage_2;
	[SerializeField] public Image NewImage_1;
	[SerializeField] public Image NewImage_2;

	public void Init()
	{
		CanvasRectTrans = GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
	}
}
