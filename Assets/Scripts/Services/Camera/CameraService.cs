using Cinemachine;
using System;
using UnityEngine;
using Zenject;
using UniRx;

public class CameraService : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera virtualCamera;
	[SerializeField] private Camera realCamera;
	[SerializeField] private Transform target;
	
	[Header("Zoom parameters")]
	[SerializeField] private float zoomSensivity = 0.8f;
	[SerializeField] private float minDistance = 2f;
	[SerializeField] private float maxDistance = 10f;
	[SerializeField] private float defaultDistance = 4f;

	[Header("Rotating parameters")]
	[SerializeField] private float horizontalSensivity = 0.8f;
	[SerializeField] private float verticalSensivity = 0.8f;
	[SerializeField] private float inertiaDamping = 5f;

	private IDisposable wheel;
	private IDisposable rightButton;
	private IDisposable mouseDelta;
	private IDisposable lateUpdate;

	private CinemachineFramingTransposer framingTransposer;
	private Vector3 targetEulers;

	public Camera RealCamera => realCamera;

	private CinemachineFramingTransposer FramingTransposer
	{
		get
		{
			if(framingTransposer == null)
			{
				framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
			}
			return framingTransposer;
		}
	}

	[Inject]
	public void Construct(MouseService mouseService, FrameUpdateService frameUpdate)
	{
		targetEulers = virtualCamera.transform.localEulerAngles;
		lateUpdate = frameUpdate.LateFrameUpdate.Subscribe(delta =>
		{
			virtualCamera.transform.rotation = Quaternion.Slerp(virtualCamera.transform.rotation, Quaternion.Euler(targetEulers), delta * inertiaDamping);
		});
		rightButton = mouseService.RightButton.Subscribe(pressed =>
		{
			if (pressed)
			{
				mouseDelta = mouseService.MouseDelta.Subscribe(delta =>
				{
					Rotate(new Vector2(-delta.y * verticalSensivity, delta.x * horizontalSensivity));
				});
			} else
			{
				mouseDelta?.Dispose();
			}
		});
		wheel = mouseService.Wheel.Subscribe(delta =>
		{
			ChangeDistance(-delta * zoomSensivity);
		});
	}

	public void OnDestroy()
	{
		lateUpdate?.Dispose();
		rightButton?.Dispose();
		mouseDelta?.Dispose();
		wheel?.Dispose();
	}

	private void Start()
	{
		SetDistance(defaultDistance);
	}

	public void FocusTo(Vector3 position)
	{
		target.transform.position = position;
	}

	public void FocusTo(Transform transform)
	{
		FocusTo(transform.position);
	}

	public void Rotate(Vector2 delta)
	{
		targetEulers += (Vector3)delta;
	}

	public void SetDistance(float distance)
	{
		FramingTransposer.m_CameraDistance = Mathf.Clamp(distance,minDistance, maxDistance);
	}

	public void ChangeDistance(float delta)
	{
		SetDistance(FramingTransposer.m_CameraDistance+delta);
	}

}
