using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum ImageEffect
{
	SwapUpToDown = 1,	//위에서 아래로
	SwapDynamic,		//버튼 위치에 맞춰서
	ScrollView,			//현재, New 이미지 모두 Swap
	Fade,				//서서히 엎어지기
	TwoImageAway		//두 이미지가 교차하기
}

[Serializable]
public class ButtonTargetImage
{
	[SerializeField] public Sprite TargetImage_1;
	[SerializeField] public Sprite TargetImage_2;
}

public class CurvedUIEditor : MonoBehaviour
{
	private CurvedUIData _curvedUIData;

	[SerializeField] private bool _autoUpdate = false;

	[Header("RightButtonSetting")]
	[SerializeField] private float _buttonRightPadding = 0.0f;
	[SerializeField] private List<ButtonTargetImage> _buttonRightSetting;

	[Header("SideButtonSetting")]
	[SerializeField] private float _buttonSideUpMargin = 0.0f;
	[SerializeField] private float _buttonSidePadding = 0.0f;
	[SerializeField] private int _buttonSideCount = 2;

	[Header("BackImageSetting")]
	[SerializeField] private ImageEffect _curImageEffect = ImageEffect.SwapUpToDown;
	[SerializeField] private float _swapSpeed = 1.0f;
	[SerializeField] private Ease _swapMoveType = Ease.Unset;

	private int _curSelectButtonNumber = 0;
	private Sequence _mySequence = null;

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
		await BackGroundImageSetting();
		await CreateSideButton();
	}

	#region BackImage
	private async Task BackGroundImageSetting()
	{
		List<Image> images = _curvedUIData.BackImageGenTransform.GetComponentsInChildren<Image>().ToList();
		List<RectTransform> rectTransforms = new List<RectTransform>();
		foreach (var image in images)
		{
			rectTransforms.Add(image.rectTransform);
		}

		List<Button> buttons = _curvedUIData.RightButtonGenTransform.GetComponentsInChildren<Button>().ToList();
		for(int index = 0; index < buttons.Count; index++)
		{
			int number = index;
			buttons[index].onClick.RemoveAllListeners();
			buttons[index].onClick.AddListener(() => ButtonEffect(number));
		}

		ButtonEffectSetting();
	}

	private void ButtonEffectSetting()
	{
		_curvedUIData.CurImage_1.sprite = _buttonRightSetting[0].TargetImage_1;
		_curvedUIData.CurImage_2.sprite = _buttonRightSetting[0].TargetImage_2;
	}

	private void ButtonEffect(int number)
	{
		switch (_curImageEffect)
		{
			case ImageEffect.SwapUpToDown:
				ImageSwapUpToDownEffect(number);
				break;
			case ImageEffect.SwapDynamic:
				ImageSwapDynamicEffect(number);
				break;
			case ImageEffect.ScrollView:
				ImageScrollViewEffect(number);
				break;
			case ImageEffect.Fade:
				ImageFadeEffect(number);
				break;
			case ImageEffect.TwoImageAway:
				ImageTwoImageAwayEffect(number);
				break;
		}
	}

	private void ImageSwapUpToDownEffect(int number)
	{
		if (_curSelectButtonNumber == number)
			return;

		if(_mySequence != null && _mySequence.IsPlaying())
		{
			_mySequence.onComplete?.Invoke();
			_mySequence.Kill();
		}

		_curSelectButtonNumber = number;

		_mySequence = DOTween.Sequence()
			.SetAutoKill(false)
			.OnStart(() =>
			{
				_curvedUIData.NewImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos1.localPosition;
				_curvedUIData.NewImage_2.sprite = _buttonRightSetting[number].TargetImage_2;
				_curvedUIData.NewImage_2.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos2.localPosition;
			})
			.Append(_curvedUIData.NewImage_1.transform.DOLocalMove(_curvedUIData.CurImagePos1.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.OnComplete(() =>
			{
				_curvedUIData.CurImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos1.localPosition;
				_curvedUIData.CurImage_2.sprite = _buttonRightSetting[number].TargetImage_2;
				_curvedUIData.NewImage_2.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos2.localPosition;
			})
			.Join(_curvedUIData.NewImage_2.transform.DOLocalMove(_curvedUIData.CurImagePos2.localPosition, _swapSpeed).SetEase(_swapMoveType));
	}

	private void ImageSwapDynamicEffect(int number)
	{
		if (_curSelectButtonNumber == number)
			return;

		if (_mySequence != null && _mySequence.IsPlaying())
		{
			_mySequence.onComplete?.Invoke();
			_mySequence.Kill();
		}

		if (_curSelectButtonNumber < number)
		{
			_curSelectButtonNumber = number;

			_mySequence = DOTween.Sequence()
			.SetAutoKill(false)
			.OnStart(() =>
			{
				_curvedUIData.NewImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos1.localPosition;
				_curvedUIData.NewImage_2.sprite = _buttonRightSetting[number].TargetImage_2;
				_curvedUIData.NewImage_2.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos2.localPosition;
			})
			.Append(_curvedUIData.NewImage_1.transform.DOLocalMove(_curvedUIData.CurImagePos1.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.OnComplete(() =>
			{
				_curvedUIData.CurImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos1.localPosition;
				_curvedUIData.CurImage_2.sprite = _buttonRightSetting[number].TargetImage_2;
				_curvedUIData.NewImage_2.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos2.localPosition;
			})
			.Join(_curvedUIData.NewImage_2.transform.DOLocalMove(_curvedUIData.CurImagePos2.localPosition, _swapSpeed).SetEase(_swapMoveType));
		}
		else
		{
			_curSelectButtonNumber = number;

			_mySequence = DOTween.Sequence()
			.SetAutoKill(false)
			.OnStart(() =>
			{
				_curvedUIData.NewImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos1.localPosition;
				_curvedUIData.NewImage_2.sprite = _buttonRightSetting[number].TargetImage_2;
				_curvedUIData.NewImage_2.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos2.localPosition;
			})
			.Append(_curvedUIData.NewImage_1.transform.DOLocalMove(_curvedUIData.CurImagePos1.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.OnComplete(() =>
			{
				_curvedUIData.CurImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos1.localPosition;
				_curvedUIData.CurImage_2.sprite = _buttonRightSetting[number].TargetImage_2;
				_curvedUIData.NewImage_2.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos2.localPosition;
			})
			.Join(_curvedUIData.NewImage_2.transform.DOLocalMove(_curvedUIData.CurImagePos2.localPosition, _swapSpeed).SetEase(_swapMoveType));
		}
	}

	private void ImageScrollViewEffect(int number)
	{
		if (_curSelectButtonNumber == number)
			return;

		if (_mySequence != null && _mySequence.IsPlaying())
		{
			_mySequence.onComplete?.Invoke();
			_mySequence.Kill();
		}

		if (_curSelectButtonNumber < number)
		{
			_curSelectButtonNumber = number;

			_mySequence = DOTween.Sequence()
			.SetAutoKill(false)
			.OnStart(() =>
			{
				_curvedUIData.NewImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos1.localPosition;
				_curvedUIData.NewImage_2.sprite = _buttonRightSetting[number].TargetImage_2;
				_curvedUIData.NewImage_2.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos2.localPosition;
			})
			.Append(_curvedUIData.NewImage_1.transform.DOLocalMove(_curvedUIData.CurImagePos1.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.OnComplete(() =>
			{
				_curvedUIData.CurImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos1.localPosition;
				_curvedUIData.CurImage_1.gameObject.transform.localPosition = _curvedUIData.CurImagePos1.localPosition;
				_curvedUIData.CurImage_2.sprite = _buttonRightSetting[number].TargetImage_2;
				_curvedUIData.NewImage_2.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos2.localPosition;
				_curvedUIData.CurImage_2.gameObject.transform.localPosition = _curvedUIData.CurImagePos2.localPosition;
			})
			.Join(_curvedUIData.CurImage_1.transform.DOLocalMove(_curvedUIData.NewImageWaitUpPos1.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.Join(_curvedUIData.NewImage_2.transform.DOLocalMove(_curvedUIData.CurImagePos2.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.Join(_curvedUIData.CurImage_2.transform.DOLocalMove(_curvedUIData.NewImageWaitUpPos2.localPosition, _swapSpeed).SetEase(_swapMoveType));
		}
		else
		{
			_curSelectButtonNumber = number;

			_mySequence = DOTween.Sequence()
			.SetAutoKill(false)
			.OnStart(() =>
			{
				_curvedUIData.NewImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos1.localPosition;
				_curvedUIData.NewImage_2.sprite = _buttonRightSetting[number].TargetImage_2;
				_curvedUIData.NewImage_2.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos2.localPosition;
			})
			.Append(_curvedUIData.NewImage_1.transform.DOLocalMove(_curvedUIData.CurImagePos1.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.OnComplete(() =>
			{
				_curvedUIData.CurImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos1.localPosition;
				_curvedUIData.CurImage_1.gameObject.transform.localPosition = _curvedUIData.CurImagePos1.localPosition;
				_curvedUIData.CurImage_2.sprite = _buttonRightSetting[number].TargetImage_2;
				_curvedUIData.NewImage_2.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos2.localPosition;
				_curvedUIData.CurImage_2.gameObject.transform.localPosition = _curvedUIData.CurImagePos2.localPosition;
			})
			.Join(_curvedUIData.CurImage_1.transform.DOLocalMove(_curvedUIData.NewImageWaitDownPos1.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.Join(_curvedUIData.NewImage_2.transform.DOLocalMove(_curvedUIData.CurImagePos2.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.Join(_curvedUIData.CurImage_2.transform.DOLocalMove(_curvedUIData.NewImageWaitDownPos2.localPosition, _swapSpeed).SetEase(_swapMoveType));
		}
	}

	private void ImageFadeEffect(int number)
	{
		if (_curSelectButtonNumber == number)
			return;

		if (_mySequence != null && _mySequence.IsPlaying())
		{
			_mySequence.onComplete?.Invoke();
			_mySequence.Kill();
		}

		if (_curSelectButtonNumber < number)
		{
			_curSelectButtonNumber = number;

			_mySequence = DOTween.Sequence()
			.SetAutoKill(false)
			.OnStart(() =>
			{
				_curvedUIData.NewImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos1.localPosition;
				_curvedUIData.NewImage_2.sprite = _buttonRightSetting[number].TargetImage_2;
				_curvedUIData.NewImage_2.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos2.localPosition;
			})
			.Append(_curvedUIData.NewImage_1.transform.DOLocalMove(_curvedUIData.CurImagePos1.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.OnComplete(() =>
			{
				_curvedUIData.CurImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos1.localPosition;
				_curvedUIData.CurImage_1.gameObject.transform.localPosition = _curvedUIData.CurImagePos1.localPosition;
				_curvedUIData.CurImage_2.sprite = _buttonRightSetting[number].TargetImage_2;
				_curvedUIData.NewImage_2.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos2.localPosition;
				_curvedUIData.CurImage_2.gameObject.transform.localPosition = _curvedUIData.CurImagePos2.localPosition;
			})
			.Join(_curvedUIData.CurImage_1.transform.DOLocalMove(_curvedUIData.NewImageWaitUpPos1.localPosition, _swapSpeed).SetEase(_swapMoveType));
		}
		else
		{
			_curSelectButtonNumber = number;

			_mySequence = DOTween.Sequence()
			.SetAutoKill(false)
			.OnStart(() =>
			{
				_curvedUIData.NewImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos1.localPosition;
			})
			.Append(_curvedUIData.NewImage_1.transform.DOLocalMove(_curvedUIData.CurImagePos1.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.OnComplete(() =>
			{
				_curvedUIData.CurImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos1.localPosition;
				_curvedUIData.CurImage_1.gameObject.transform.localPosition = _curvedUIData.CurImagePos1.localPosition;
			})
			.Join(_curvedUIData.CurImage_1.transform.DOLocalMove(_curvedUIData.NewImageWaitDownPos1.localPosition, _swapSpeed).SetEase(_swapMoveType));
		}
	}

	private void ImageTwoImageAwayEffect(int number)
	{
		if (_curSelectButtonNumber == number)
			return;

		if (_mySequence != null && _mySequence.IsPlaying())
		{
			_mySequence.onComplete?.Invoke();
			_mySequence.Kill();
		}

		if (_curSelectButtonNumber < number)
		{
			_curSelectButtonNumber = number;

			_mySequence = DOTween.Sequence()
			.SetAutoKill(false)
			.OnStart(() =>
			{
				_curvedUIData.NewImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos1.localPosition;
			})
			.Append(_curvedUIData.NewImage_1.transform.DOLocalMove(_curvedUIData.CurImagePos1.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.OnComplete(() =>
			{
				_curvedUIData.CurImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitDownPos1.localPosition;
				_curvedUIData.CurImage_1.gameObject.transform.localPosition = _curvedUIData.CurImagePos1.localPosition;
			})
			.Join(_curvedUIData.CurImage_1.transform.DOLocalMove(_curvedUIData.NewImageWaitUpPos1.localPosition, _swapSpeed).SetEase(_swapMoveType));
		}
		else
		{
			_curSelectButtonNumber = number;

			_mySequence = DOTween.Sequence()
			.SetAutoKill(false)
			.OnStart(() =>
			{
				_curvedUIData.NewImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos1.localPosition;
			})
			.Append(_curvedUIData.NewImage_1.transform.DOLocalMove(_curvedUIData.CurImagePos1.localPosition, _swapSpeed).SetEase(_swapMoveType))
			.OnComplete(() =>
			{
				_curvedUIData.CurImage_1.sprite = _buttonRightSetting[number].TargetImage_1;
				_curvedUIData.NewImage_1.gameObject.transform.localPosition = _curvedUIData.NewImageWaitUpPos1.localPosition;
				_curvedUIData.CurImage_1.gameObject.transform.localPosition = _curvedUIData.CurImagePos1.localPosition;
			})
			.Join(_curvedUIData.CurImage_1.transform.DOLocalMove(_curvedUIData.NewImageWaitDownPos1.localPosition, _swapSpeed).SetEase(_swapMoveType));
		}
	}
	#endregion

	#region RightButton
	private async Task CreateRightButton()
	{
		int buttonRightCount = _buttonRightSetting.Count;
		float canvasHeight = _curvedUIData.CanvasRectTrans.sizeDelta.y;
		float buttonHeight = (canvasHeight - _buttonRightPadding) / buttonRightCount;

		//갯수 조절
		List<Button> buttons = _curvedUIData.RightButtonGenTransform.GetComponentsInChildren<Button>().ToList();
		if (buttons.Count() < buttonRightCount)
		{
			for (int i = 0; i < buttonRightCount - buttons.Count(); i++)
			{
				GameObject buttonClone = Instantiate(_curvedUIData.ButtonPrefab, _curvedUIData.RightButtonGenTransform);
				await Task.Yield();
				buttons.Add(buttonClone.GetComponent<Button>());
			}
		}
		else if (buttons.Count() > buttonRightCount)
		{
			for (int i = 0; i < buttons.Count() - buttonRightCount;)
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

		for (int index = 0; index < buttonRightCount; index++)
		{
			float targetPosY = canvasHeight / 2 - (buttonHeight / 2 * (1 + (index * 2))) - index * _buttonRightPadding / (buttonRightCount - 1);
			Vector3 targetPos = new Vector3(0, targetPosY, 0);
			buttonRectTrans[index].localPosition = targetPos;

			Vector2 targetSizeDelta = buttonRectTrans[index].sizeDelta;
			targetSizeDelta.y = buttonHeight;
			buttonRectTrans[index].sizeDelta = targetSizeDelta;
		}
	}
	#endregion

	#region SideButton
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
	#endregion
}