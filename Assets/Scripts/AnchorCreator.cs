﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

//
// This script allows us to create anchors with
// a prefab attached in order to visbly discern where the anchors are created.
// Anchors are a particular point in space that you are asking your device to track.
//

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class AnchorCreator : MonoBehaviour
{
    public GameObject anchorPrefab;

    private GameObject placedBoard;
    private ARAnchor anchorPoint;
    private ARRaycastManager raycastManager;
    private ARAnchorManager anchorManager;
    private ARPlaneManager planeManager;

    // On Awake(), we obtains a reference to all the required components.
    // The ARRaycastManager allows us to perform raycasts so that we know where to place an anchor.
    // The ARPlaneManager detects surfaces we can place our objects on.
    // The ARAnchorManager handles the processing of all anchors and updates their position and rotation.
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

                if (placedBoard)
                {
                    return;

                    Destroy(placedBoard);
                    Destroy(anchorPoint);

                    placedBoard = null;
                }

                // This attaches an anchor to the area on the plane corresponding to the raycast hit,
                // and afterwards instantiates an instance of your chosen prefab at that point.
                // This prefab instance is parented to the anchor to make sure the position of the prefab is consistent
                // with the anchor, since an anchor attached to an ARPlane will be updated automatically by the ARAnchorManager as the ARPlane's exact position is refined.
                var anchor = anchorManager.AttachAnchor(hitPlane, hitPose);
                placedBoard = Instantiate(anchorPrefab, anchor.transform);
                anchorPoint = anchor;
            }
        }
    }
}