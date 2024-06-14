using System;

public interface IStateObject
{
	IObservable<IState> StateStream { get; }
}
