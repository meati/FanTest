using System;
using UniRx;

public class WorldClickService
{
	private IDisposable focusedDisposable;
	private IDisposable clickDisposable;

	private IInteractable focused;
	public WorldClickService(RayService rayService, MouseService mouseService)
	{
		focusedDisposable = rayService.Focused.Subscribe(focused =>
		{
			this.focused = focused;
		});
		clickDisposable = mouseService.LeftButton.Where(pressed => pressed).Subscribe(_ => {
			focused?.Interact();
		});
	}

	~WorldClickService()
	{
		focusedDisposable?.Dispose();
		clickDisposable?.Dispose();
	}
}
