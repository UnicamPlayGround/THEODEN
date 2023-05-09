using System;
using UnityEngine;

namespace Utility.CameraManager.XR.AR
{
    public class CameraManager: Utility.CameraManager._3D.CameraManager
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
                    if ((Math.Abs(touchOne.deltaPosition.x) < ThresholdSwipe &&
                        Math.Abs(touchZero.deltaPosition.x) < ThresholdSwipe &&
                        // the next condition checks if fingers are moving the same way
                        touchZero.deltaPosition.y - touchOne.deltaPosition.y <
                        MaximumDifferenceBetweenFingersDuringSlide) ||
                        // if swiping left or right -> abs(delta.x) < ThresholdSwipe
                        (Math.Abs(touchOne.deltaPosition.y) < ThresholdSwipe &&
                         Math.Abs(touchZero.deltaPosition.y) < ThresholdSwipe &&
                            // the next condition checks if fingers are moving the same way
                            touchZero.deltaPosition.x - touchOne.deltaPosition.x <
                         MaximumDifferenceBetweenFingersDuringSlide))
                    {
                        TranslateModel(new Vector3(touchZero.deltaPosition.x, touchZero.deltaPosition.y, 0));
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
                        ZoomModel(magnitudeDifference);
                    }
                    break;
            }
        }
    }
}