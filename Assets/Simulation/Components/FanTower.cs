using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class FanTower : MonoBehaviour
{
	private FrameUpdateService frameUpdateService;

	[SerializeField] private ButtonSwitch powerSwitch;
	[SerializeField] private Vector3 deviationEulers = new Vector3(0, 45, 0);
	[SerializeField] private float deviationPeriod = 4f;

	private IDisposable powerSwitchDisposable;
	private IDisposable updateDisposable;

	private float progress;

	private void Actualize(float delta)
	{
		progress += delta;
		transform.localRotation = Quaternion.Euler(deviationEulers * Mathf.Sin(2 * Mathf.PI * progress / deviationPeriod));
	}

	[Inject]
	public void Construct(FrameUpdateService frameUpdateService)
	{
		this.frameUpdateService = frameUpdateService;
		powerSwitchDisposable = powerSwitch.StateStream.Subscribe(state =>
		{
			updateDisposable?.Dispose();
			if(state.stateIndex == 1)
			{
				updateDisposable = frameUpdateService.FrameUpdate.Subscribe(delta =>
				{
					Actualize(delta);
				});
			}
		});
	}

	private void OnDestroy()
	{
		powerSwitchDisposable?.Dispose();
		updateDisposable?.Dispose();
	}
}
