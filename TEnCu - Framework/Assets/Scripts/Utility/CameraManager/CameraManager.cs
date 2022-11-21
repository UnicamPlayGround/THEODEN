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
        protected float startingZEulerAngle;

        private void Start()
        {
            cam = Camera.allCameras[0];
            _fieldOfView = cam.fieldOfView;
            startingZEulerAngle = cam.transform.rotation.eulerAngles.z;
        }

        private void Update()
        {
            if (!trigger.activeSelf) return;
            ManageInput();
        }

        protected virtual void ManageInput() { }

        protected void Zoom(float deltaZoom)
        {
             if (deltaZoom > 0)
                _fieldOfView -= modelConfigs.camera.speedModifier.zoom;
            else if (deltaZoom < 0) _fieldOfView += modelConfigs.camera.speedModifier.zoom;

            _fieldOfView = Mathf.Clamp(_fieldOfView, modelConfigs.camera.fieldOfView.min, modelConfigs.camera.fieldOfView.max);
            cam.fieldOfView = _fieldOfView;
        }

        protected void TranslateModel(Vector3 deltas)
        {
            model.transform.Translate(deltas * modelConfigs.prefab.speedModifier.translation);
            var currentPosition = model.transform.position;
            currentPosition.x = Mathf.Clamp(currentPosition.x, modelConfigs.prefab.width.min, modelConfigs.prefab.width.max);
            currentPosition.y = Mathf.Clamp(currentPosition.y, modelConfigs.prefab.height.min, modelConfigs.prefab.height.max);
            currentPosition.z = Mathf.Clamp(currentPosition.z, modelConfigs.prefab.depth.min, modelConfigs.prefab.depth.max);
            model.transform.position = currentPosition;
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
            currentCameraPosition.z = startingZEulerAngle;
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
            currentCameraPosition.z = startingZEulerAngle;
            transform.position = currentCameraPosition;
        }

        protected abstract void Scroll(Vector2 deltaPosition);

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
