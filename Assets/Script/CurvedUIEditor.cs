using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CurvedUIEditor : MonoBehaviour
{
	private CurvedUIData _curvedUIData;

	[SerializeField] private bool _autoUpdate = false;

	[Header("RightButtonSetting")]
	[SerializeField] private float _buttonRightPadding = 0.0f;
	[SerializeField] private int _buttonRightCount = 2;

	[Header("BackImageSetting")]

	[Header("SideButtonSetting")]
	[SerializeField] private float _buttonSideUpMargin = 0.0f;
	[SerializeField] private float _buttonSidePadding = 0.0f;
	[SerializeField] private int _buttonSideCount = 2;

	private void Init()
	{

	}

	private void OnValidate()
	{
		if (!_autoUpdate) return;

		_curvedUIData = GetComponent<CurvedUIData>();
		_curvedUIData.Init();
		Init();
		CurvedSettingStart();
	}

	private async Task CurvedSettingStart()
	{
		await CreateRightButton();
		BackGroundImageSetting();
		await CreateSideButton();
	}

	private void BackGroundImageSetting()
	{
		List<Image> images = _curvedUIData.BackImageGenTransform.GetComponentsInChildren<Image>().ToList();
		List<RectTransform> rectTransforms = new List<RectTransform>();
		foreach (var image in images)
		{
			rectTransforms.Add(image.rectTransform);
		}

		foreach (var rectTransform in rectTransforms)
		{
			rectTransform.sizeDelta = _curvedUIData.CanvasRectTrans.sizeDelta;
		}
	}

	private async Task CreateRightButton()
	{
		float canvasHeight = _curvedUIData.CanvasRectTrans.sizeDelta.y;
		float buttonHeight = (canvasHeight - _buttonRightPadding) / _buttonRightCount;

		//갯수 조절
		List<Button> buttons = _curvedUIData.RightButtonGenTransform.GetComponentsInChildren<Button>().ToList();
		if (buttons.Count() < _buttonRightCount)
		{
			for (int i = 0; i < _buttonRightCount - buttons.Count(); i++)
			{
				GameObject buttonClone = Instantiate(_curvedUIData.ButtonPrefab, _curvedUIData.RightButtonGenTransform);
				await Task.Yield();
				buttons.Add(buttonClone.GetComponent<Button>());
			}
		}
		else if (buttons.Count() > _buttonRightCount)
		{
			for (int i = 0; i < buttons.Count() - _buttonRightCount;)
			{
				Button buttonClone = buttons[i];
				buttons.Remove(buttons[i]);

				buttonClone.gameObject.SetActive(false);
				await Task.Yield();
				DestroyImmediate(buttonClone.gameObject);
				await Task.Yield();
			}
		}

		//버튼 배치
		List<RectTransform> buttonRectTrans = new List<RectTransform>();
		foreach (var button in buttons)
			buttonRectTrans.Add(button.GetComponent<RectTransform>());

		for (int index = 0; index < _buttonRightCount; index++)
		{
			float targetPosY = canvasHeight / 2 - (buttonHeight / 2 * (1 + (index * 2))) - index * _buttonRightPadding / (_buttonRightCount - 1);
			Vector3 targetPos = new Vector3(0, targetPosY, 0);
			buttonRectTrans[index].localPosition = targetPos;

			Vector2 targetSizeDelta = buttonRectTrans[index].sizeDelta;
			targetSizeDelta.y = buttonHeight;
			buttonRectTrans[index].sizeDelta = targetSizeDelta;
		}
	}


	private async Task CreateSideButton()
	{
		float canvasHeight = _curvedUIData.CanvasRectTrans.sizeDelta.y;
		float buttonHeight = 0;

		//갯수 조절
		List<Button> buttons = _curvedUIData.SideButtonGenTransform.GetComponentsInChildren<Button>().ToList();
		if (buttons.Count() < _buttonSideCount)
		{
			for (int i = 0; i < _buttonSideCount - buttons.Count(); i++)
			{
				GameObject buttonClone = Instantiate(_curvedUIData.SideButtonPrefab, _curvedUIData.SideButtonGenTransform);
				await Task.Yield();
				buttons.Add(buttonClone.GetComponent<Button>());
			}
		}
		else if (buttons.Count() > _buttonSideCount)
		{
			for (int i = 0; i < buttons.Count() - _buttonSideCount;)
			{
				Button buttonClone = buttons[i];
				buttons.Remove(buttons[i]);

				buttonClone.gameObject.SetActive(false);
				await Task.Yield();
				DestroyImmediate(buttonClone.gameObject);
			}
		}

		//버튼 배치
		List<RectTransform> buttonRectTrans = new List<RectTransform>();
		foreach (var button in buttons)
			buttonRectTrans.Add(button.GetComponent<RectTransform>());

		buttonHeight = buttonRectTrans.Count() > 0 ? 0 : buttonRectTrans[0].sizeDelta.y;
		for (int index = 0; index < _buttonSideCount; index++)
		{
			float targetPosY = (canvasHeight / 2 - _buttonSideUpMargin) - (buttonHeight / 2 * (1 + (index * 2))) - index * _buttonSidePadding / (_buttonSideCount - 1);
			Vector3 targetPos = new Vector3(0, targetPosY, 0);
			buttonRectTrans[index].localPosition = targetPos;

			Vector2 targetSizeDelta = buttonRectTrans[index].sizeDelta;
			buttonRectTrans[index].sizeDelta = targetSizeDelta;
		}
	}
}