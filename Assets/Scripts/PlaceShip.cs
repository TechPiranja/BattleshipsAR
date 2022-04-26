using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class PlaceShip : MonoBehaviour
{
    public GameObject shipAnchor;

    private GameObject placedShip;
    private ARAnchor anchorPoint;
    private ARRaycastManager raycastManager;
    private ARAnchorManager anchorManager;
    private ARPlaneManager planeManager;
    private Vector3 rotationVector = new Vector3(0, 90, 0);

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    void Update()
    {
        // If there is no tap, then simply do nothing until the next call to Update().
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;

        if (Input.touchCount == 1)
        {

            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = hits[0].pose;
                var hitTrackableId = hits[0].trackableId;
                var hitPlane = planeManager.GetPlane(hitTrackableId);

                if (placedShip)
                {
                    Destroy(placedShip);
                    Destroy(anchorPoint);

                    placedShip = null;
                }

                // This attaches an anchor to the area on the plane corresponding to the raycast hit,
                // and afterwards instantiates an instance of your chosen prefab at that point.
                // This prefab instance is parented to the anchor to make sure the position of the prefab is consistent
                // with the anchor, since an anchor attached to an ARPlane will be updated automatically by the ARAnchorManager as the ARPlane's exact position is refined.
                var anchor = anchorManager.AttachAnchor(hitPlane, hitPose);
                placedShip = Instantiate(shipAnchor, anchor.transform);
                anchorPoint = anchor;
            }
        }
        else if(Input.touchCount == 2 && placedShip)
        {
            placedShip.transform.Rotate(rotationVector);
            return;
        }
    }
}
