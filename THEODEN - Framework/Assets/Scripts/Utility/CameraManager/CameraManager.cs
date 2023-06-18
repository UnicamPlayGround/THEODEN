using System;
using Models.ModelConfigurations;
using UnityEngine;

namespace Utility.CameraManager
{
    public abstract class CameraManager : MonoBehaviour
    {
        public ModelConfigs modelConfigs;
        public Camera cam;
        public GameObject trigger;
        public GameObject model;
        protected const float ThresholdSwipe = 10F;
        protected const float MaximumDifferenceBetweenFingersDuringSlide = 10F;
        private float _fieldOfView;
        protected float startingZEulerAngleCamera, startingZEulerAngleModel;

        private void Start()
        {
            cam ??= Camera.allCameras[0];
            _fieldOfView = cam.fieldOfView;
            startingZEulerAngleCamera = cam.transform.rotation.eulerAngles.z;
            if (model is not null)
                startingZEulerAngleModel = model.transform.rotation.eulerAngles.z;
        }

        private void Update()
        {
            if (trigger != null && !trigger.activeSelf) return;
            ManageInput();
        }

        protected virtual void ManageInput() { }

        protected void ZoomCamera(float deltaZoom)
        {
            if (deltaZoom > 0)
                _fieldOfView -= 1;
            else if (deltaZoom < 0) _fieldOfView += 1;

            _fieldOfView = Mathf.Clamp(_fieldOfView, modelConfigs.camera.fieldOfView.min, modelConfigs.camera.fieldOfView.max);
            cam.fieldOfView = _fieldOfView;
        }

        protected void ZoomModel(float deltaZoom)
        {
            float scale;
            if (deltaZoom > 0) scale = +0.01F;
            else if (deltaZoom < 0) scale = -0.01F;
            else return;
            var scaledModel = model.transform.localScale;
            scaledModel += new Vector3(scale, scale, scale);
            
            scaledModel.x = Mathf.Clamp(scaledModel.x, modelConfigs.prefab.scaleX.min, modelConfigs.prefab.scaleX.max);
            scaledModel.y = Mathf.Clamp(scaledModel.y, modelConfigs.prefab.scaleY.min, modelConfigs.prefab.scaleY.max);
            scaledModel.z = Mathf.Clamp(scaledModel.z, modelConfigs.prefab.scaleZ.min, modelConfigs.prefab.scaleZ.max);
            
            model.transform.localScale = scaledModel;
        }
        
        protected void TranslateModel(Vector3 deltas)
        {
            model.transform.Translate(deltas * modelConfigs.prefab.speedModifier.translation);
            var currentPosition = model.transform.localPosition;
            currentPosition.x = Mathf.Clamp(currentPosition.x, modelConfigs.prefab.width.min, modelConfigs.prefab.width.max);
            currentPosition.y = Mathf.Clamp(currentPosition.y, modelConfigs.prefab.height.min, modelConfigs.prefab.height.max);
            currentPosition.z = Mathf.Clamp(currentPosition.z, modelConfigs.prefab.depth.min, modelConfigs.prefab.depth.max);
            model.transform.localPosition = currentPosition;
        }

        protected void TranslateCamera(Vector3 deltas)
        {
            var currentCameraPosition = transform.position;
            var newCameraYPosition =
                currentCameraPosition.y - deltas.y * modelConfigs.camera.speedModifier.translation;
            currentCameraPosition.y =
                Mathf.Clamp(newCameraYPosition, modelConfigs.camera.height.min, modelConfigs.camera.height.max);
            var newCameraXPosition = currentCameraPosition.x + deltas.y * modelConfigs.camera.speedModifier.translation;
            currentCameraPosition.x = Mathf.Clamp(newCameraXPosition, modelConfigs.camera.width.min, modelConfigs.camera.width.max);
            currentCameraPosition.z = startingZEulerAngleCamera;
            transform.position = currentCameraPosition;
        }
        
        [Obsolete("Deprecated, Use TranslateCamera(Vector3 deltas) instead")]
        protected void TranslateCameraY(float deltaY)
        {
            var currentCameraPosition = transform.position;
            var newCameraYPosition =
                currentCameraPosition.y - deltaY * modelConfigs.camera.speedModifier.translation;
            currentCameraPosition.y =
                Mathf.Clamp(newCameraYPosition, modelConfigs.camera.height.min, modelConfigs.camera.height.max);
            currentCameraPosition.z = startingZEulerAngleCamera;
            transform.position = currentCameraPosition;
        }
        
        protected abstract void ScrollCamera(Vector2 deltaPosition);

        protected void ScrollModel(Vector2 deltaPosition)
        {
            var xAngleRotation = deltaPosition.x * modelConfigs.prefab.speedModifier.rotation;
            var yAngleRotation = deltaPosition.y * modelConfigs.prefab.speedModifier.rotation;
            xAngleRotation = Mathf.Round(xAngleRotation);
            yAngleRotation = Mathf.Round(yAngleRotation);
            //set camera (x and y angles inverted because the screen is in landscape mode)
            //use y to turn left and right, use x to turn up and down
            model.transform.Rotate(new Vector3(-yAngleRotation, xAngleRotation, 0), Space.Self);

            var currentEulerRotation = model.transform.rotation.eulerAngles;

            //clamp new values between bounds
            var fixedX = ClampInAngles(currentEulerRotation.x, modelConfigs.prefab.xAngle.min, modelConfigs.prefab.xAngle.max);

            //no need to modify y, rotation over y is just turning model left and right
            //no need to modify z, rotation over z is fixed automatically   
            //set camera again with fixed values
            transform.rotation = Quaternion.Euler(fixedX, currentEulerRotation.y, startingZEulerAngleModel);
            //model.transform.position = Vector3.zero;
        }
        
        protected static float ClampInAngles(float value, float min, float max)
        {
            //360°-max is closer to min then max
            if (value < (double)min || value > 360 - max)
                value = min;
            else if (value > (double)max)
                value = max;
            return value;
        }
    }
}