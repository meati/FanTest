using System;
using UniRx;
using UnityEngine;

public class FrameUpdateService : MonoBehaviour
{
	private Subject<float> updateStream = new ();
	private Subject<float> lateUpdateStream = new ();
	public IObservable<float> FrameUpdate => updateStream.AsObservable();
	public IObservable<float> LateFrameUpdate => lateUpdateStream.AsObservable();

	private void Update()
	{
		updateStream.OnNext(Time.deltaTime);	
	}

	private void LateUpdate()
	{
		lateUpdateStream.OnNext(Time.deltaTime);
	}
}
