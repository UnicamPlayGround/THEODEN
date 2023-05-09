using UnityEngine;

namespace Utility.CameraManager._2D
{
    public class CameraManager: Utility.CameraManager.CameraManager
    {
        protected override void ManageInput()
        {
            switch (Input.touchCount)
            {
                case 1:
                    var touch = Input.GetTouch(0);
                    if (touch.phase != TouchPhase.Moved) return;
                    Scroll(touch.deltaPosition);
                    break;
                case 2:
                    var touchZero = Input.GetTouch(0);
                    var touchOne = Input.GetTouch(1);
                    var touchZeroCurrentPosition = touchZero.position;
                    var touchOneCurrentPosition = touchOne.position;
                    var touchZeroPreviousPosition = touchZeroCurrentPosition - touchZero.deltaPosition;
                    var touchOnePreviousPosition = touchOneCurrentPosition - touchOne.deltaPosition;
                    var previousPositionsMagnitude = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
                    var currentPositionsMagnitude = (touchZeroCurrentPosition - touchOneCurrentPosition).magnitude;
                    var magnitudeDifference = currentPositionsMagnitude - previousPositionsMagnitude;
                    ZoomCamera(magnitudeDifference);
                    break;
            }
        }

        protected sealed override void Scroll(Vector2 deltaPosition)
        {
            var xTranslation = deltaPosition.x * modelConfigs.camera.speedModifier.rotation;
            var yTranslation = deltaPosition.y * modelConfigs.camera.speedModifier.rotation;

            xTranslation = Mathf.Round(xTranslation);
            yTranslation = Mathf.Round(yTranslation);

            var currentPosition = cam.transform.position + new Vector3(-xTranslation, -yTranslation, 0);

            //clamp new values between bounds
            var fixedX = Mathf.Clamp(currentPosition.x, modelConfigs.camera.height.min, modelConfigs.camera.height.max);
            var fixedY = Mathf.Clamp(currentPosition.y, modelConfigs.camera.width.min, modelConfigs.camera.width.max);

            //set camera again with fixed values
            cam.transform.position = new Vector3(fixedX, fixedY, currentPosition.z);
        }
    }
}