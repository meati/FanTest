using UnityEngine;
using Zenject;
using UniRx;
using System;
using UnityEngine.Audio;

public class FanBlades : MonoBehaviour
{
	[SerializeField] private ButtonSwitch powerSwitch;
	[SerializeField] private ButtonSwitch speedSwitch;
	[SerializeField] private HingeJoint hinge;

	[Header("Speed")]

	[SerializeField] private float lowSpeed = 500f;
	[SerializeField] private float highSpeed = 1000f;

	[Header("Sound")]
	[SerializeField] private AudioSource audioSource;

	private float currentSpeed;

	private bool isWorking = false;

	private IDisposable updateDisposable;
	private IDisposable powerSwitchDisposable;
	private IDisposable speedSwitchDisposable;

	[Inject]
	public void Construct(FrameUpdateService frameUpdateService)
	{
		updateDisposable = frameUpdateService.FrameUpdate.Subscribe(delta =>
		{
			var targetVolume = isWorking ? 1f : 0f;
			var targetPitch = isWorking ? 1f+(currentSpeed / lowSpeed-1)*0.2f : 0.8f;
			if (audioSource != null)
			{
				if (isWorking && !audioSource.isPlaying)
				{
					audioSource.Play();
				}
				if (!isWorking && audioSource.volume < 0.2f)
				{
					audioSource.Pause();
				}
				audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, delta);
				audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, delta);
			}
			else
			{
				Debug.Log("No audioSource");
			}
		});
		speedSwitchDisposable = speedSwitch.StateStream.Subscribe(state =>
		{
			currentSpeed = state.stateIndex > 0 ? highSpeed : lowSpeed;
			var motor = hinge.motor;
			motor.targetVelocity = currentSpeed;
			hinge.motor = motor;
		});
		powerSwitchDisposable = powerSwitch.StateStream.Subscribe(state =>
		{
			if (state.stateIndex > 0)
			{
				isWorking = true;
				hinge.useMotor = true;
			}
			else
			{
				isWorking = false;
				hinge.useMotor = false;
			}
		});
	}

	private void OnDestroy()
	{
		speedSwitchDisposable?.Dispose();
		powerSwitchDisposable?.Dispose();
		updateDisposable?.Dispose();
	}
}
