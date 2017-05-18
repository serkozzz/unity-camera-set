using UnityEngine;
using System.Collections;


namespace BM
{
	public class AABBCollisionsDetector : ICollisionsDetector
	{
		Vector3 _modelPosition;
		Vector3 _modelSize;

		public void SetModelPlacementData(Vector3 modelSize, Vector3 modelPosition)
		{
			_modelPosition = modelPosition;
			_modelSize = modelSize;
		}

		public float CollisionOffset
		{
			get;
			set;
		}

		public bool IsPointInModel(Vector3 position)
		{
			Vector3 positiveBounds = _modelPosition + _modelSize / 2 + new Vector3(CollisionOffset, CollisionOffset, CollisionOffset);
			Vector3 negativeBounds = _modelPosition - _modelSize / 2 - new Vector3(CollisionOffset, CollisionOffset, CollisionOffset);

			if ((position.x < positiveBounds.x && position.x > negativeBounds.x)
				&& (position.y < positiveBounds.y && position.y > negativeBounds.y)
				&& (position.z < positiveBounds.z && position.z > negativeBounds.z))
			{
				return true;
			}
			return false;
		}
	}
}
