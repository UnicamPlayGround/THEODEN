using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public ObjectZoomAndRotationVR collisionHandlerZoomAndRotation;
    private void OnTriggerEnter(Collider other)
    {
        collisionHandlerZoomAndRotation.TriggerEnter(other);
    }
    private void OnTriggerExit(Collider other)
    {
        collisionHandlerZoomAndRotation.TriggerExit(other);
    }
    private void OnTriggerStay(Collider other)
    {
        collisionHandlerZoomAndRotation.TriggerStay(other);
    }
}
