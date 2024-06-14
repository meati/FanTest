using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
	[SerializeField] private CameraService cameraServicePrefab;
	[SerializeField] private TooltipService tooltipServicePrefab;

	public override void InstallBindings()
	{
		// Сервис камеры
		Container.Bind<CameraService>().FromComponentInNewPrefab(cameraServicePrefab).AsSingle().NonLazy();

		// Луч-указатель
		Container.Bind<RayService>().AsSingle().NonLazy();

		// Подсказки
		Container.Bind<TooltipService>().FromComponentInNewPrefab(tooltipServicePrefab).AsSingle().NonLazy();
		
		// Клики по объектам
		Container.Bind<WorldClickService>().AsSingle().NonLazy();
	}
}
