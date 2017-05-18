using UnityEngine;
using System.Collections;


namespace BM
{
	public interface ICollisionsDetector 
	{
		bool IsPointInModel(Vector3 position);
		
		void SetModelPlacementData(Vector3 modelSize, Vector3 modelPosition);

		float CollisionOffset
		{
			get;
			set;
		}
	}
}