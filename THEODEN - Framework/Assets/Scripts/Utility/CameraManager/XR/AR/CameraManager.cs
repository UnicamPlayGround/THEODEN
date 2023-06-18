using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Utility.CameraManager.XR.AR
{
    public class CameraManager: Utility.CameraManager._3D.CameraManager
    {
        public GameObject modelPrefab;
        
        public ARTrackedImageManager arTrackedImageManager;

        private void OnEnable() => arTrackedImageManager.trackedImagesChanged += ArTrackerChanged;

        private void OnDisable() => arTrackedImageManager.trackedImagesChanged -= ArTrackerChanged;

        private void ArTrackerChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            if (eventArgs.added.Count > 0)
            {
                model = null;
            }
            
            if (eventArgs.removed.Count > 0)
            {
                Destroy(model);
                model = null;
            }
            
        }
        protected override void ManageInput()
        {
            if (model is null)
            {
                var trackedImage = FindObjectsOfType<ARTrackedImage>(false);
                if (trackedImage is not { Length: > 0 }) return;
                ARTrackedImage arTrackedImage = null;
                foreach (var image in trackedImage)
                {
                    if (image.trackingState != UnityEngine.XR.ARSubsystems.TrackingState.Tracking) continue;
                    arTrackedImage = image;
                }
                if (arTrackedImage is null) return;
                arTrackedImage.transform.localScale = Vector3.one;
                arTrackedImage.transform.position = Vector3.zero;
                arTrackedImage.transform.rotation = Quaternion.identity;
                model = Instantiate(modelPrefab, position: modelConfigs.prefab.position.GetVector3(), 
                    rotation: Quaternion.Euler(modelConfigs.prefab.eulerRotation.GetVector3()), 
                    parent: arTrackedImage.transform);
                model.transform.localScale = modelConfigs.prefab.scale.GetVector3();
                startingZEulerAngleModel = model.transform.rotation.eulerAngles.z;
            }

            switch (Input.touchCount)
            {
                case 1:
                    var touch = Input.GetTouch(0);
                    if (touch.phase != TouchPhase.Moved) return;
                    var delta = touch.deltaPosition;
                    //Note: allowing only to turn model, not to tilt it
                    delta.y = 0;
                    delta.x = -delta.x;
                    ScrollModel(delta);
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
                    ZoomModel(magnitudeDifference);
                    break;
                case 3:
                    var touchTranslate = Input.GetTouch(0);
                    TranslateModel(new Vector3(touchTranslate.deltaPosition.x, touchTranslate.deltaPosition.y, 0));
                    break;
            }
        }

        public void ResetPrefab()
        {
            model.transform.localScale = modelConfigs.prefab.scale.GetVector3();
            model.transform.rotation = Quaternion.Euler(modelConfigs.prefab.eulerRotation.GetVector3());
            model.transform.localPosition = modelConfigs.prefab.position.GetVector3();
        }
    }
}