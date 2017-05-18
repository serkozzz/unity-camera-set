using UnityEngine;
using System.Collections;

namespace BM
{
	public abstract class BaseCameraMover : MonoBehaviour
	{
		public Vector3 InitialPosition = new Vector3(0.6f, 0.4f, 0.675f);

		protected void Start()
		{
			this.MoveToInitialPosition();
		}

		public abstract void RotateCamera(float rotationX, float rotationY);
		public abstract void MoveCamera(float zoomValue);

		public abstract void MoveToInitialPosition();
	}
}