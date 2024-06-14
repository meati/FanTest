using System;
using UniRx;
using UnityEngine;

public class RayService
{
	private IDisposable mousePosition;

	private ReactiveProperty<IInteractable> focused = new ReactiveProperty<IInteractable>();

	public IObservable<IInteractable> Focused => focused.AsObservable();

	public RayService(CameraService cameraService, MouseService mouseService)
	{
		mousePosition = mouseService.MousePosition.Subscribe(position =>
		{
			if (cameraService?.RealCamera != null)
			{
				var ray = cameraService.RealCamera.ScreenPointToRay(position);
				if(Physics.Raycast(ray, out RaycastHit hit))
				{
					var obj = hit.collider.gameObject.GetComponentInParent<IInteractable>();
					if (obj != null)
					{
						focused.Value = obj;
					} else
					{
						focused.Value = null;
					}
				} else
				{
					focused.Value = null;
				}
			}
		});
	}

	~RayService()
	{
		mousePosition.Dispose();
	}
}
