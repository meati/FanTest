using UnityEngine;
using Zenject;
using UniRx;
using System;

public class TweenService 
{
	private IterationList<TweenJob> jobs = new();

	private IDisposable updateDisposable;

	public TweenService(FrameUpdateService frameUpdateService)
	{
		updateDisposable = frameUpdateService.FrameUpdate.Subscribe(delta =>
		{
			jobs.Iterate((index, job) => job.Simulate(delta));
		});
	}

	~TweenService()
	{
		updateDisposable?.Dispose();
	}

	public TweenHandler Tween(Action<float> action, float duration)
	{
		var tweenJob = new TweenJob(action, duration);
		jobs.Add(tweenJob);
		return new TweenHandler(tweenJob);
	}

	public class TweenHandler
	{
		public TweenJob tweenJob;
		public TweenHandler(TweenJob tweenJob)
		{
			this.tweenJob = tweenJob;
		}

		public void Stop()
		{
			this.tweenJob?.Stop();
		}
		public void Finish()
		{
			this.tweenJob?.Finish();
		}
	}

	public class TweenJob
	{
		private float time = 0;
		private float duration = 0;
		private Action<float> action;
		private bool stopped;

		public TweenJob() { }

		public TweenJob(Action<float> action, float duration)
		{
			this.action = action;
			this.duration = duration;
		}

		public void Stop()
		{
			stopped = true;
		}

		public void Finish()
		{
			action.Invoke(1f);
			stopped = true;
		}

		public bool Simulate(float delta)
		{
			if (stopped) return true;
			time = Mathf.Min(duration, time + delta);
			action.Invoke(time / duration);
			return time == duration;
		}
	}
}
