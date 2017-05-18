using UnityEngine;

namespace BM
{
	public static class CameraHelper
	{
		public static void Set3DObjectPlacementData(float sizeX, float sizeY, float sizeZ, float posX, float posY, float posZ)
		{
			//float[] boundingBoxEdges = new float[] {x , y, z};
			//Array.Sort(boundingBoxEdges);
			//float a = boundingBoxEdges[2] / 2;
			//float b = boundingBoxEdges[1] / 2;

			GameObject camera = GameObject.Find("Main Camera");
			SmartCameraMoverAroundTarget cameraScript = camera.GetComponent<SmartCameraMoverAroundTarget>();

			//cameraScript.setNewMinDistatnce(Mathf.Sqrt(a * a + b * b) + 0.4f);
			if (cameraScript)
			{
				cameraScript.SetModelPlacementData(new Vector3(sizeX, sizeY, sizeZ), new Vector3(posX, posY, posZ));
			}
		}
	}
}