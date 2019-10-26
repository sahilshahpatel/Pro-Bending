using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthbenderHandController : MonoBehaviour
{
    public const float PICKUP_RANGE = Mathf.Infinity;
    public bool isRightHand;
    private OVRInput.Axis1D handTrigger, indexTrigger;
    private OVRInput.Controller OVRController;

    private EarthDiskController earthDisk;
    public enum BendingState { NONE, CONTROLLING, FLYING }
    public BendingState bendingState;

    public VelocityTracker vTrack;
    public enum PunchState { NONE, PUNCHING }
    public PunchState punchState;
    public Transform shoulder; // TODO: setup shoulder tracking 
    public Vector3 forward; // TODO: setup forward vector to point out from shoulders

    public List<Vector3> velDuringPunch;

    private float punchStartThreshold, punchEndThreshold;
    private float triggerThreshold = 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        if (isRightHand)
        {
            handTrigger = OVRInput.Axis1D.SecondaryHandTrigger;
            indexTrigger = OVRInput.Axis1D.SecondaryIndexTrigger;
            OVRController = OVRInput.Controller.RTrackedRemote;
        }
        else
        {
            handTrigger = OVRInput.Axis1D.PrimaryHandTrigger;
            indexTrigger = OVRInput.Axis1D.PrimaryIndexTrigger;
            OVRController = OVRInput.Controller.LTrackedRemote;
        }

        vTrack = this.gameObject.GetComponent<VelocityTracker>();
        if (vTrack == null) Debug.LogError("HandController missing VelocityTracker");

        punchState = PunchState.NONE;
        bendingState = BendingState.NONE;
        velDuringPunch = new List<Vector3>();
        earthDisk = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(bendingState == BendingState.NONE){
            // Tether to element if 
            if(OVRInput.Get(indexTrigger) > triggerThreshold && earthDisk == null){
                // Cast ray to see what is hit
                RaycastHit hitInfo;
                Physics.Raycast(transform.position, this.transform.position - shoulder.transform.position, out hitInfo, PICKUP_RANGE);
                Transform hitTransform = hitInfo.transform;
                GameObject hitObj = null;
                if (hitTransform != null) hitObj = hitTransform.gameObject;
                
                EarthDiskController hitDisk = hitObj.GetComponent<EarthDiskController>();
                if (hitObj != null && hitDisk != null){
                    tether(hitDisk);
                    bendingState = BendingState.CONTROLLING;
                }
            }
        }
        else if(bendingState == BendingState.CONTROLLING){
            // TODO: Detect Punch
        }
        else if(bendingState == BendingState.FLYING){
            // TODO: Detect release
        }   
    }

    void tether(EarthDiskController disk){
        this.earthDisk = disk;
        disk.tether(this);
    }
}
