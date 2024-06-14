using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TweenService;
using Zenject;
using System;
using UniRx;

public class FanHead : MonoBehaviour
{
	[SerializeField] private AdjusterSwitch adjuster;
	[SerializeField] private float transitionDuration;

	[SerializeField] private List<Vector3> positions = new List<Vector3>();

	private TweenHandler handler;

	private IDisposable stateStream;

	[Inject]
	public void Construct(TweenService tweenService)
	{
		adjuster.SetValuesCount(positions.Count);
		if(positions.Count > 0) {
			stateStream = adjuster.StateStream.Subscribe(state =>
			{
				handler?.Stop();
				var start = transform.localRotation;
				var end = Quaternion.Euler(positions[state.stateIndex%positions.Count]);
				handler = tweenService.Tween(t => transform.localRotation = Quaternion.SlerpUnclamped(start, end, t), transitionDuration);
			});
		}
	}

	public void OnDestroy()
	{
		stateStream.Dispose();
	}
}
