using UnityEngine;
using System.Collections;

namespace BM
{
	public class CameraMoverAroundTarget : BaseCameraMover
	{
		public Vector3 Target = Vector3.zero;

		public float MinDistance = 0.1f;
		public float MaxDistance = 1.4f;

		public float MaxOxRotation = 60F;

		public override void RotateCamera(float rotationX, float rotationY)
		{
			Vector3 oldPosition = transform.position;

			transform.RotateAround(Vector3.zero, transform.right, -rotationY);

			//calculate angle with horizon plane
			Vector3 directionVector = transform.position - this.Target;
			directionVector.Normalize();
			Vector3 directionVectorOXZprojection = new Vector3(directionVector.x, 0, directionVector.z);
			directionVectorOXZprojection.Normalize();
			float dotProduct = Vector3.Dot(directionVector, directionVectorOXZprojection);
			float angle = Mathf.Abs(Mathf.Acos(dotProduct)) * Mathf.Rad2Deg; // in degrains

			if (angle >= this.MaxOxRotation)
			{
				transform.position = oldPosition;
			}
			transform.RotateAround(this.Target, Vector3.up, rotationX);
			transform.LookAt(this.Target);
		}

		public override void MoveCamera(float zoomValue)
		{
			Vector3 desiredPos = transform.position + transform.forward * zoomValue;
			float distatnceToTarget = (desiredPos - this.Target).magnitude;

			if (zoomValue > 0 && distatnceToTarget <= this.MinDistance)
			{
				desiredPos = transform.position + transform.forward * (transform.position.magnitude - this.MinDistance);
			}
			else if (zoomValue < 0 && distatnceToTarget >= MaxDistance)
			{
				desiredPos = transform.position + transform.forward * (transform.position.magnitude - this.MaxDistance);
			}

			transform.position = desiredPos;
		}

		public override void MoveToInitialPosition()
		{
			transform.position = this.InitialPosition;
			transform.LookAt(this.Target);
		}
	}
}