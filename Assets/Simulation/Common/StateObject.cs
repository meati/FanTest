using System;
using UniRx;
using UnityEngine;

public class StateObject<S> : MonoBehaviour, IStateObject where S : IState
{
	private ReactiveProperty<S> state = new (default);
	public IObservable<S> StateStream => state.AsObservable();

	protected S CurrentState => state.Value;

	protected void Publish(S state)
	{
		this.state.Value = state;
	}

	IObservable<IState> IStateObject.StateStream => state.Select(state=> state as IState).AsObservable<IState>();
}
