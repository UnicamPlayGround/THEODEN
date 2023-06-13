using System;
using UnityEngine;

namespace Utility.CameraManager._3D
{
    public class CameraManager : Utility.CameraManager.CameraManager
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

                    // if swiping up or down -> abs(delta.y) < ThresholdSwipe
                    if (Math.Abs(touchOne.deltaPosition.x) < ThresholdSwipe &&
                        Math.Abs(touchZero.deltaPosition.x) < ThresholdSwipe &&
                        // the next condition checks if fingers are moving the same way
                        touchZero.deltaPosition.y - touchOne.deltaPosition.y <
                        MaximumDifferenceBetweenFingersDuringSlide)
                    {
                        TranslateCamera(new Vector3(0, touchZero.deltaPosition.y, 0));
                    }
                    else
                    {
                        var touchZeroCurrentPosition = touchZero.position;
                        var touchOneCurrentPosition = touchOne.position;
                        var touchZeroPreviousPosition = touchZeroCurrentPosition - touchZero.deltaPosition;
                        var touchOnePreviousPosition = touchOneCurrentPosition - touchOne.deltaPosition;
                        var previousPositionsMagnitude = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
                        var currentPositionsMagnitude = (touchZeroCurrentPosition - touchOneCurrentPosition).magnitude;
                        var magnitudeDifference = currentPositionsMagnitude - previousPositionsMagnitude;
                        ZoomCamera(magnitudeDifference);
                    }
                    break;
            }
        }

        protected sealed override void Scroll(Vector2 deltaPosition)
        {
            var xAngleRotation = deltaPosition.x * modelConfigs.camera.speedModifier.rotation;
            var yAngleRotation = deltaPosition.y * modelConfigs.camera.speedModifier.rotation;
            xAngleRotation = Mathf.Round(xAngleRotation);
            yAngleRotation = Mathf.Round(yAngleRotation);
            //set camera (x and y angles inverted because the screen is in landscape mode)
            //use y to turn left and right, use x to turn up and down
            transform.Rotate(new Vector3(-yAngleRotation, xAngleRotation, 0), Space.Self);

            var currentEulerRotation = transform.rotation.eulerAngles;

            //clamp new values between bounds
            var fixedX = ClampInAngles(currentEulerRotation.x, modelConfigs.camera.angle.min, modelConfigs.camera.angle.max);

            //no need to modify y, rotation over y is just going around model
            //no need to modify z, rotation over z is fixed automatically
            //set camera again with fixed values
            transform.rotation = Quaternion.Euler(fixedX, currentEulerRotation.y, startingZEulerAngle);
        }
    }
}