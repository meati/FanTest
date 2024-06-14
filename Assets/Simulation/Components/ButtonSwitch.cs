using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static TweenService;

public class ButtonSwitch : Switch
{
	[Header("Visual object")]
	[SerializeField] private Transform switchTrigger;


	[Header("Transition parameters")]
	[SerializeField] private float transitionDuration = 0.25f;

	[Header("States")]
	[SerializeField] private List<StateTransformation> transformations;

	TweenService tweenService;
	TweenHandler handler;

	[Inject]
	public void Construct(TweenService tweenService)
	{
		this.tweenService = tweenService;
		Actualize(true);
	}

	public override string GetName()
	{
		return transformations[CurrentState.stateIndex].name;
	}

	public override int GetValuesCount()
	{
		return transformations.Count;
	}

	protected override void Actualize(bool immediate = false)
	{
		handler?.Stop();
		var endRotation = Quaternion.Euler(transformations[CurrentState.stateIndex].localEuler);
			if(immediate)
			{
				switchTrigger.localRotation = endRotation;
				return;
			}
		var startRotation = switchTrigger.localRotation;
		handler = tweenService.Tween(t => {
			switchTrigger.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
		}, 0.2f);
	}

	protected override ValueIncrementation GetIncrementation()
	{
		return ValueIncrementation.Forward;
	}

	[Serializable]
	public struct StateTransformation
	{
		public Vector3 localEuler;
		public string name;
	}
}
