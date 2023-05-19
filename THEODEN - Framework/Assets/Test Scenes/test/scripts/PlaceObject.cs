using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Test_Scenes.test.scripts
{
    public class PlaceObject : MonoBehaviour
    {
        public GameObject objectToPlace;
        public ARRaycastManager raycastManager;
        public ARPlaneManager planeManager;
        private bool _isElementPlaced;
        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            Touch.onFingerDown += OnFingerDown;
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
            Touch.onFingerDown -= OnFingerDown;
        }

        private void OnFingerDown(Finger finger)
        {
            if (_isElementPlaced) return;
            _isElementPlaced = true;
            if (finger.index != 0) return;
            List<ARRaycastHit> hits = new();
            var raycast = raycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon);
            if (!raycast) return;
            var hit = hits[0];
            var isCorrectSurface = planeManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp;
            if (!isCorrectSurface) return;
            var pose = hit.pose;
            var unused = Instantiate(objectToPlace, pose.position, pose.rotation);
        }
    }
}
