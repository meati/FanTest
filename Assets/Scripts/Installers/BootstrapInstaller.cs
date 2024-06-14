using Zenject;

public class BootstrapInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		// Настройки движка
		Container.Bind<EngineSetup>().AsSingle().NonLazy();

		// Провайдер сообщений обновления кадра
		Container.Bind<FrameUpdateService>().FromNewComponentOnNewGameObject().AsSingle();
		
		// Tween-сервис
		Container.Bind<TweenService>().AsSingle();

		// Обработчик мыши
		Container.Bind<MouseService>().AsSingle();
	}
}
