using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static TweenService;

public abstract class Switch : StateObject<Switch.State>, IInteractable
{

	[Header("Sound")]
	[SerializeField] private AudioSource audioSource;

	private int direction = -1;
	public abstract int GetValuesCount();

	protected abstract ValueIncrementation GetIncrementation();

	protected abstract void Actualize(bool immediate = false);

	private int ToggleValue()
	{
		var currentValue = CurrentState.stateIndex;
		if(GetIncrementation() == ValueIncrementation.Mirrored)
		{
			if ((currentValue == 0) || (currentValue == GetValuesCount() - 1))
			{
				direction *= -1;
			}
			return currentValue + direction;
		} else
		{
			return (currentValue + 1) % GetValuesCount();
		}
	}

	public void Toggle()
	{
		Publish(new State(ToggleValue()));
		Actualize();
		PlaySound();
	}

	protected virtual void PlaySound()
	{
		if (audioSource != null)
		{
			audioSource.Play();
		}
	}

	public abstract string GetName();
	
	public void Interact()
	{
		Toggle();
	}

	public struct State : IState
	{
		public int stateIndex;
		public State(int stateIndex = 0)
		{
			this.stateIndex = stateIndex;
		}
	}

	public enum ValueIncrementation
	{
		Forward,
		Mirrored,
	}
}
