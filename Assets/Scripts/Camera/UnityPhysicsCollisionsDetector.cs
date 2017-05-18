using UnityEngine;
using System.Collections;

namespace BM
{
	public class UnityPhysicsCollisionsDetector : ICollisionsDetector
	{
		public float CollisionOffset
		{
			get;
			set;
		}

		public void SetModelPlacementData(Vector3 modelSize, Vector3 modelPosition)
		{}

		public bool IsPointInModel(Vector3 position)
		{
			if (Physics.CheckSphere(position, CollisionOffset))
				return true;
			return false;
		}
	}
}