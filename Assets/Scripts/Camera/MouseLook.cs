using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add a rigid body to the capsule
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSWalker script to the capsule

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
	public bool isCameraRotationEnabled = true;
	public Rect guiPanelRect;

	public enum GestureType
	{
		Touch,
		Zoom,
		MouseLeft,
		MouseRoll,
		None
	}

	public enum RotationAxes
	{
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2
	}
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 1F;
	public float sensitivityY = 1F;

	public float zoomSpeed = 30.0f;

	Vector2 oldTouchPosition;
	float _distanceBetween2Touches = 0;
	GestureType _currentGesture = GestureType.None;
	BM.BaseCameraMover _cameraMover;

	void Start()
	{
		_cameraMover = GetComponent<BM.BaseCameraMover>();
		if (!_cameraMover)
			Debug.LogError("Camera mover script reference = null.");
	}


	void Update()
	{

		//if (Input.GetMouseButtonDown(0))
		//{
		//	RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0);

		//	if (hitInfo)
		//	{
		//		if (hitInfo.collider.gameObject.CompareTag("Shelf"))
		//		{
		//			oldTouchPosition = Vector2.zero;
		//			_distanceBetween2Touches = 0;
		//			return;
		//		}
		//	}
		//} 
		

		if (!isCameraRotationEnabled)
			return;

		GestureType oldGesture = _currentGesture;
		_currentGesture = RecognizeGesture();
		if (oldGesture != _currentGesture)
		{
			oldTouchPosition = Vector2.zero;
			_distanceBetween2Touches = 0;
		}

		//if (_currentGesture != GestureType.Touch && _currentGesture != GestureType.MouseLeft)
		//{
		//	oldTouchPosition = Vector2.zero;
		//}

		//if (_currentGesture != GestureType.Zoom)
		//	_distanceBetween2Touches = 0;

		switch (_currentGesture)
		{
			case GestureType.Touch:
				{
					PerformTouchGestureAction();
					break;
				}
			case GestureType.Zoom:
				{
					PerformZoomGestureAction();
					break;
				}
			case GestureType.MouseLeft:
				{
					PerformMouseLeftGestureAction();
					break;
				}
			case GestureType.MouseRoll:
				{
					PerformMouseRollGestureAction();
					break;
				}
			case GestureType.None:
				return;
			default:
				return;
		}
	}


	public void testEvent()
	{
		Debug.Log("testEvent");
	}

	GestureType RecognizeGesture()
	{
		int fingerCount = 0;
		foreach (Touch touch in Input.touches)
		{
			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
			{
				fingerCount++;
			}
		}

		if (fingerCount == 2)
			return GestureType.Zoom;
		else if (fingerCount == 1)
			return GestureType.Touch;
		else if (fingerCount == 0 && Input.GetMouseButton(0))
			return GestureType.MouseLeft;
		else if (Input.GetAxis("Mouse ScrollWheel") != 0)
			return GestureType.MouseRoll;

		return GestureType.None;
	}

	void PerformTouchGestureAction()
	{
		if (oldTouchPosition == Vector2.zero)
		{
			oldTouchPosition = Input.touches[0].position;
			return;
		}
		Vector2 deltaTouchVector = Input.touches[0].position - oldTouchPosition;
		oldTouchPosition = Input.touches[0].position;

		float rotationX = (float)deltaTouchVector.x * sensitivityX;
		float rotationY = (float)deltaTouchVector.y * sensitivityY;

		_cameraMover.RotateCamera(rotationX, rotationY);
	}


	void PerformMouseLeftGestureAction()
	{
		float rotationX = Input.GetAxis("Mouse X") * sensitivityX * 5;
		float rotationY = Input.GetAxis("Mouse Y") * sensitivityX * 5;
		_cameraMover.RotateCamera(rotationX, rotationY);
	}


	void PerformZoomGestureAction()
	{
		float delta = GetDistanceBetweenTouches(Input.touches[0], Input.touches[1]);
		if (_distanceBetween2Touches == 0)
		{
			_distanceBetween2Touches = delta;
			return;
		}
		float innacuracy = 2.0f;
		float direction = 0.0f;
		if (delta - _distanceBetween2Touches > innacuracy)
		{
			direction = 1;
		}
		else if (delta - _distanceBetween2Touches < -innacuracy)
		{
			direction = -1;
		}
		_distanceBetween2Touches = delta;
		_cameraMover.MoveCamera(direction * zoomSpeed);
	}


	void PerformMouseRollGestureAction()
	{
		float direction = 0.0f;
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			direction = -1.0f;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			direction = 1.0f;
		}
		_cameraMover.MoveCamera(direction * 1.5f * zoomSpeed);
	}


	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}

	float GetDistanceBetweenTouches(Touch touch0, Touch touch1)
	{
		Vector2 touch0Touch1Vector = touch1.position - touch0.position;
		return (float)System.Math.Sqrt(touch0Touch1Vector.x * touch0Touch1Vector.x + touch0Touch1Vector.y * touch0Touch1Vector.y);
	}


	public void fixedCameraToogleOn(bool value)
	{
		isCameraRotationEnabled = !value;
	}
}