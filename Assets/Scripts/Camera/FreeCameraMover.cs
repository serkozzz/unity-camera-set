using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BM
{
    public class FreeCameraMover : BaseCameraMover
    {


        public override void RotateCamera(float rotationX, float rotationY)
        {
            transform.RotateAround(transform.position, transform.right, -rotationY);
            transform.RotateAround(transform.position, Vector3.up, rotationX);
        }

        public override void MoveCamera(float zoomValue)
        {
            transform.position += transform.forward * zoomValue;
        }

        public override void MoveToInitialPosition()
        {
            transform.position = this.InitialPosition;
        }

    }
}