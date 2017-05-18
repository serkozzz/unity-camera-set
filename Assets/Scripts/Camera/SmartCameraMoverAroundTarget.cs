using UnityEngine;
using System.Collections;


namespace BM
{
	public class SmartCameraMoverAroundTarget : CameraMoverAroundTarget
	{
		ICollisionsDetector _collisionsDetector = new UnityPhysicsCollisionsDetector();
		
		public float CollisionOffset = 0.1f;

		void Start()
		{
			base.Start();
			_collisionsDetector.CollisionOffset = this.CollisionOffset;
		}

		public override void RotateCamera(float rotationX, float rotationY)
		{
			base.RotateCamera(rotationX, rotationY);
			this.CorrectTransformByModelBounds(this.GetPlacementRelativeModel());
		}


		float _desiredDistanceToModelCenter = float.MaxValue;
		public override void MoveCamera(float zoomValue)
		{
			PlacementRelativeModel placement = this.GetPlacementRelativeModel();
			base.MoveCamera(zoomValue);
			_desiredDistanceToModelCenter = (_modelPosition - transform.position).magnitude;
			this.CorrectTransformByModelBounds(placement);

		}

		Vector3 _modelPosition;
		public void SetModelPlacementData(Vector3 modelSize, Vector3 modelPosition)
		{
			_modelPosition = modelPosition;
			_collisionsDetector.SetModelPlacementData(modelSize, modelPosition);
		}



		void CorrectTransformByModelBounds(PlacementRelativeModel placement)
		{
			Vector3 cameraDirection = (this.Target - transform.position).normalized;

			if (!_collisionsDetector.IsPointInModel(transform.position))
			{
				float distanceToTarget = (this.Target - transform.position).magnitude;
				float delta = distanceToTarget - _desiredDistanceToModelCenter;
				if (delta > 0)
				{
					transform.position += cameraDirection * delta;
				}
			}

			float step = 0.005f;
			while (_collisionsDetector.IsPointInModel(transform.position))
			{
				transform.position += -cameraDirection * step;
			}
			transform.LookAt(this.Target);
		}


		PlacementRelativeModel GetPlacementRelativeModel()
		{
			PlacementRelativeModel result = new PlacementRelativeModel();
			Vector3 directionToModelCenter = _modelPosition - transform.position;
			result.isXPositive = directionToModelCenter.x < 0;
			result.isYPositive = directionToModelCenter.y < 0;
			result.isZPositive = directionToModelCenter.z < 0;
			return result;
		}

		class PlacementRelativeModel
		{
			public bool isXPositive;
			public bool isYPositive;
			public bool isZPositive;

			//public bool operator== (PlacementRelativeModel other)
			//{
			//	if (this.isXPositive != other.isXPositive)
			//		return false;
			//if (this.isYPositive != other.isYPositive)
			//		return false;
			//	if (this.isZPositive != other.isZPositive)
			//		return false;
			//	return true;
			//}

			//public bool operator!= (PlacementRelativeModel other)
			//{
			//	if (this.isXPositive != other.isXPositive)
			//		return false;
			//if (this.isYPositive != other.isYPositive)
			//		return false;
			//	if (this.isZPositive != other.isZPositive)
			//		return false;
			//	return true;
			//}
		}
	}
}