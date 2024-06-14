
using UnityEngine;
using static TweenService;
using Zenject;

public class AdjusterSwitch : Switch
{
	[Header("Visual object")]
	[SerializeField] private Transform switchTrigger;
	[SerializeField] private float transitionDuration = 0.3f;
	[Header("Data")]
	[SerializeField] int valuesCount;
	[SerializeField] string actionName;
	[SerializeField] ValueIncrementation incrementation;

	[SerializeField] Vector3 deviationEulers = new Vector3(8, 0, 0);

	private Quaternion defaultRotation;

	TweenService tweenService;
	TweenHandler handler;

	[Inject]
	public void Construct(TweenService tweenService)
	{
		this.tweenService = tweenService;
		Actualize(true);
		defaultRotation = switchTrigger.localRotation;
	}

	public void SetValuesCount(int valuesCount)
	{
		this.valuesCount = valuesCount;
		Publish(new State(Mathf.Clamp(CurrentState.stateIndex, 0, valuesCount - 1)));
	}

	public override string GetName()
	{
		return actionName;
	}

	public override int GetValuesCount()
	{
		return Mathf.Max(1,valuesCount);
	}

	protected override void Actualize(bool immediate = false)
	{ 
		handler?.Stop();
		if(!immediate)
		{
			var start = switchTrigger.localRotation;
			var end = switchTrigger.localRotation;
			handler = tweenService.Tween(t =>
			{
				switchTrigger.localRotation = Quaternion.Slerp(start, end, t)
					* Quaternion.Euler(deviationEulers * Mathf.Sin(2*Mathf.PI * t));
			}, transitionDuration);
		}
	}

	protected override ValueIncrementation GetIncrementation()
	{
		return incrementation;
	}
}
