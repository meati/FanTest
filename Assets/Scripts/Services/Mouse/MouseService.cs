using System;
using UniRx;
using UnityEngine;

public class MouseService 
{
	private IDisposable frameUpdateDisposable;

	private ReactiveProperty<bool> leftButton = new();
	private ReactiveProperty<bool> rightButton = new();
	private ReactiveProperty<bool> middleButton = new();
	private ReactiveProperty<float> wheel = new();
	private ReactiveProperty<Vector2> mousePosition = new();
	private ReactiveProperty<Vector2> mouseDelta = new();

	public IObservable<bool> LeftButton => leftButton.AsObservable();
	public IObservable<bool> RightButton => rightButton.AsObservable();
	public IObservable<bool> MiddleButton => middleButton.AsObservable();
	public IObservable<float> Wheel => wheel.AsObservable();
	public IObservable<Vector2> MousePosition => mousePosition.AsObservable();
	public IObservable<Vector2> MouseDelta => mouseDelta.AsObservable();

	public MouseService(FrameUpdateService frameUpdateService)
	{
		frameUpdateDisposable = frameUpdateService.FrameUpdate.Subscribe(deltaTime =>
		{
			leftButton.Value = Input.GetMouseButton(0);
			rightButton.Value = Input.GetMouseButton(1);
			middleButton.Value = Input.GetMouseButton(2);
			wheel.Value = Input.mouseScrollDelta.y;
			mousePosition.Value = Input.mousePosition;
			mouseDelta.Value = Input.mousePositionDelta;
		});
	}

	~MouseService()
	{
		frameUpdateDisposable?.Dispose();
	}
}
