using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class ObjectZoomAndRotationVR : MonoBehaviour
{
    [Header("Tags :")]
    [Space(10)]
    [SerializeField]
    protected string[] interactableTags;

    [Header("Inspect point position :")]
    [Space(10)]
    [SerializeField]
    protected Transform inspectPointPosition;

    [Header("Controllers properties :")]
    [Space(10)]
    [SerializeField]
    protected GameObject rightController;
    [Space(5)]
    [SerializeField]
    protected GameObject leftController;
    [Space(5)]
    [SerializeField]
    protected InputActionReference rightControllerTriggerInput;
    [Space(5)]
    [SerializeField]
    protected InputActionReference leftControllerTriggerInput;
    [Space(5)]
    [SerializeField]
    protected InputActionReference rightControllerGripInput;
    [Space(5)]
    [SerializeField]
    protected InputActionReference leftControllerGripInput;
    [Space(5)]
    [SerializeField]
    protected InputActionReference leftControllerJoystick;
    [Space(5)]
    [SerializeField]
    protected InputActionReference rightControllerJoystick;

    [Header("Collision stop :")]
    [Space(10)]
    [SerializeField]
    protected Collider toCollideWith;

    private GameObject inspectedObject;
    private GameObject highLightedObjectLeftController;
    private GameObject highLightedObjectRightController;

    private bool zoomMode = false;
    private bool stopZoom = false;
    private bool rotationMode = false;


    [Header("Rotation speed:")]
    [Space(10)]
    [SerializeField]
    private float ROTATION_SPEED_MODIFIER = 1;

    [Header("Zoom speed:")]
    [Space(10)]
    [SerializeField]
    private float ZOOM_SPEED_MODIFIER = 1;

    [Header("Inspected Object layer effect: ")]
    [Space(10)]
    [SerializeField]
    GameObject effect;
    [Header("Inspected Object layer: ")]
    [Space(10)]
    [SerializeField]
    [Tooltip("The main camera must not render this layer, the secondary camera must render only this layer")]
    int inspectedObjectLayer;
    private HighLightBorders highLightBorders;
    private Vector3 previousPosition = Vector3.zero;
    private float previousDistanceOfControllers = 0;

    GameObject targetOfInspection;
    private void Awake()
    {
        TryGetComponent<HighLightBorders>(out highLightBorders);
        effect.SetActive(false);
    }

    void Update()
    {
        if (inspectedObject == null)
        {
            targetOfInspection = ObjectSelection();
            if(targetOfInspection!=null)
            {
                highLightBorders.RemoveAllHighLight();
                inspectedObject = InstantiateObject(targetOfInspection, inspectPointPosition.transform.position);
                highLightedObjectLeftController = null;
                highLightedObjectRightController = null;
                effect.SetActive(true);

            }
        }
        if ((rightControllerGripInput.action.ReadValue<float>() >0.5 || leftControllerGripInput.action.ReadValue<float>()>0.5) && inspectedObject != null)
        {
            ResetMode();
            Renderer[] tmpRenderers = targetOfInspection.transform.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in tmpRenderers)
                renderer.enabled = true;
            effect.SetActive(false);
            Destroy(inspectedObject);

        }

        if (inspectedObject != null)
        {
            if (rightControllerTriggerInput.action.ReadValue<float>() > 0.5 && leftControllerTriggerInput.action.ReadValue<float>() > 0.5)
            {
                zoomMode = true;
                rotationMode = false;
                previousPosition = Vector3.zero;
            }
            else if (rightControllerTriggerInput.action.ReadValue<float>() > 0.5 || leftControllerTriggerInput.action.ReadValue<float>() > 0.5)
            {
                rotationMode = true;
                zoomMode = false;
            }
            else
            {
                ResetMode();
                JoystickRotate();
            }
            if (rotationMode)
                Rotate();
            if (zoomMode)
                Zoom();
        }
    }

    protected void ResetMode()
    {
        zoomMode = false;
        rotationMode = false;
        previousPosition = Vector3.zero;
        previousDistanceOfControllers = 0;
}
    protected void Zoom()
    {
        float distance = Mathf.Abs(Vector3.Distance(rightController.transform.position, leftController.transform.position));
        float deltaDistance = (distance-previousDistanceOfControllers)*ZOOM_SPEED_MODIFIER;
        Vector3 tmpScale = inspectedObject.transform.localScale;
        if (deltaDistance > 0 && stopZoom || ( previousDistanceOfControllers==0))
            deltaDistance = 0;
        inspectedObject.transform.localScale = new Vector3(
            Mathf.Clamp(tmpScale.x + deltaDistance, 0.5f, 10f),
            Mathf.Clamp(tmpScale.y + deltaDistance, 0.5f, 10f),
            Mathf.Clamp(tmpScale.z + deltaDistance, 0.5f, 10f));
        previousDistanceOfControllers = distance;
    }

    protected void Rotate()
    {
         GameObject usedController = (rightControllerTriggerInput.action.ReadValue<float>() > 0.5) ?
                rightController : leftController;

        if (previousPosition != Vector3.zero)
        {
            Vector3 deltaPosition = usedController.transform.position - previousPosition;
            
            float tmpX = deltaPosition.x;
            deltaPosition = new Vector3(
                deltaPosition.y,
                tmpX,
                0
                );
                inspectedObject.transform.Rotate(deltaPosition * ROTATION_SPEED_MODIFIER);


        }
        previousPosition = usedController.transform.position; 
    }

    protected void JoystickRotate()
    {
        Vector2 joystickRotate = Vector2.zero;
        if (leftControllerJoystick.action.ReadValue<Vector2>() != Vector2.zero)
        {
            Vector2 tmpJoystick = leftControllerJoystick.action.ReadValue<Vector2>();
            joystickRotate = new Vector2
                (tmpJoystick.y,
                tmpJoystick.x * -1);

        }
        else
        {
            Vector2 tmpJoystick = rightControllerJoystick.action.ReadValue<Vector2>();
            joystickRotate = new Vector2
                (tmpJoystick.y,
                tmpJoystick.x*-1);
        }
        if (Mathf.Abs(joystickRotate.x) > 0.5f && Mathf.Abs(joystickRotate.x) > Mathf.Abs(joystickRotate.y))
            joystickRotate = new Vector2(joystickRotate.x, 0);
        else if(Mathf.Abs(joystickRotate.y)>0.5f)
            joystickRotate = new Vector2(0, joystickRotate.y);
        inspectedObject.transform.Rotate(joystickRotate);

    }
    GameObject InstantiateObject(GameObject gameObject, Vector3 pos)
    {
        GameObject tmpObject = Instantiate(gameObject, pos, Quaternion.identity);
        CollisionDetection tmpCollision = tmpObject.AddComponent<CollisionDetection>();
        tmpCollision.collisionHandlerZoomAndRotation = this;

        //We scroll through all of the renderers of the object we chose to inspect
        //and we disable every one of them.
        //The resulting effect is that the original inspected object is
        //invisible until the user stops the interaction
        Renderer[] tmpRenderers = gameObject.transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in tmpRenderers)
            renderer.enabled = false;
        for(int x=0;x<tmpObject.transform.childCount;x++)
        {
            tmpObject.transform.GetChild(x).gameObject.layer = inspectedObjectLayer;
        }
        tmpObject.layer = 6;
        return tmpObject;
    }

    public void TriggerEnter(Collider other)
    {
        if (toCollideWith!=null && other.Equals(toCollideWith))
            stopZoom = true;
    }
    public void TriggerExit(Collider other)
    {
        if (toCollideWith != null && other.Equals(toCollideWith))
            stopZoom = false;
    }

    public void TriggerStay(Collider other)
    {

        if (toCollideWith != null && other.Equals(toCollideWith))
        {
            Vector3 tmpScale = inspectedObject.transform.localScale;
            inspectedObject.transform.localScale = new Vector3(
                   Mathf.Clamp(tmpScale.x - 0.05f, 0.5f, 10f),
                   Mathf.Clamp(tmpScale.y - 0.05f, 0.5f, 10f),
                   Mathf.Clamp(tmpScale.z - 0.05f, 0.5f, 10f));
        }
    }

    private GameObject ObjectSelection()
    {
        RaycastHit hitRight;
        RaycastHit hitLeft;

        if (inspectPointPosition == null) 
            return null;

        //Left Hand Controller
        if (Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hitLeft) &&
            interactableTags.Contains(hitLeft.transform.tag))
        {
            if(highLightedObjectLeftController == null)
            {
                highLightedObjectLeftController = hitLeft.transform.GameObject();
                highLightBorders.ApplyHighLight(highLightedObjectLeftController);
            }

        }
        if (hitLeft.transform != null && highLightedObjectLeftController!=null &&  hitLeft.transform.gameObject != highLightedObjectLeftController)
        {
            if (highLightedObjectRightController != null && highLightBorders.highLightedObjects.Contains(highLightedObjectRightController))
            {
                highLightBorders.RemoveHighLight(highLightedObjectLeftController);
                if (highLightedObjectLeftController == highLightedObjectRightController)
                    highLightedObjectRightController = null;
                highLightedObjectLeftController = null;
            }
        }
        if (leftControllerTriggerInput.action.ReadValue<float>() > 0.5)
            return highLightedObjectLeftController;

        //Right Hand Controller
        if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hitRight) &&
            interactableTags.Contains(hitRight.transform.tag))
        {
            if (highLightedObjectRightController == null)
            {
                highLightedObjectRightController = hitRight.transform.GameObject();
                highLightBorders.ApplyHighLight(highLightedObjectRightController);
            }

        }
        if(hitRight.transform != null && highLightedObjectRightController!=null && hitRight.transform.gameObject != highLightedObjectRightController)
        {
            if (highLightedObjectRightController != null && highLightBorders.highLightedObjects.Contains(highLightedObjectRightController))
            {
                highLightBorders.RemoveHighLight(highLightedObjectRightController);
                if (highLightedObjectLeftController == highLightedObjectRightController)
                    highLightedObjectLeftController = null;
                highLightedObjectRightController = null;
            }
        }

        if (rightControllerTriggerInput.action.ReadValue<float>() > 0.5)
            return highLightedObjectRightController;

        return null;
    }
}


