using System;
using UnityEngine;
using Zenject;
using UniRx;
using TMPro;
using UnityEngine.UI;

public class TooltipService : MonoBehaviour
{
	[SerializeField] private RectTransform tooltip;
	[SerializeField] private TextMeshProUGUI tooltipText;

	private IDisposable mousePosition;
	private IDisposable focused;
	private IDisposable focusedState;

	private Vector2 canvasSize;

	[Inject]
	public void Construct(MouseService mouseService, RayService rayService)
	{
		mousePosition = mouseService.MousePosition.Subscribe(position =>
		{
			var rt = transform as RectTransform;
			var canvasPosition = new Vector2(
				rt.rect.width * position.x / Screen.width,
				rt.rect.height * position.y / Screen.height
				);
			tooltip.anchoredPosition = canvasPosition;
		});

		focused = rayService.Focused.Subscribe(focused =>
		{
			focusedState?.Dispose();
			if (focused != null)
			{
				focusedState = focused.StateStream.Subscribe(_ =>
				{
					SetText(focused?.GetName());
				});
			} else
			{
				SetText(null);
			}
		});
	}

	public void SetText(string text)
	{
		if(text == null)
		{
			tooltip.gameObject.SetActive(false);
		} else
		{
			tooltip.gameObject.SetActive(true);
			tooltipText.text = text;
			LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipText.transform as RectTransform);
			LayoutRebuilder.ForceRebuildLayoutImmediate(tooltip);
		}
	}

	private void OnDestroy()
	{
		mousePosition.Dispose();
		focused.Dispose();
	}
}
