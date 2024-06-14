using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IInteractable : IStateObject
{
	public string GetName();
	public void Interact();
}
