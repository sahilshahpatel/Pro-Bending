using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDiskController : MonoBehaviour
{
    private EarthbenderHandController controllingHand; // The HandController script for the controlling script
    private float distanceRatio; // The original ratio of magnitude of (dist hand to disk)/(dist shoulder to hand)
    private DiskState diskState; // The DiskState of this disk

    public enum DiskState {INACTIVE, CONTROLLED, FLYING}

    // Start is called before the first frame update
    void Start()
    {
        controllingHand = null;
        distanceRatio = 1;
        diskState = DiskState.INACTIVE;
    }

    // Update is called once per frame
    void Update()
    {
        if(diskState == DiskState.INACTIVE){
            // TODO: Logic for inactive disks
        }
        else if(diskState == DiskState.CONTROLLED){
            this.transform.position = this.distanceRatio * (controllingHand.transform.position - controllingHand.shoulder.transform.position);
            this.gameObject.transform.rotation = controllingHand.transform.rotation*Quaternion.Euler(0, 0, 90);
        }
        else if(diskState == DiskState.FLYING){
            // TODO: Logic for flying disks
        }
    }

    public void tether(EarthbenderHandController controllingHand){
        // Only tether to a controlling hand if currently inactive
        if(diskState == DiskState.INACTIVE){
            diskState = DiskState.CONTROLLED; // The disk is now controlled
            // Set initial values for controlled state
            this.controllingHand = controllingHand;
            this.distanceRatio = (controllingHand.transform.position - this.transform.position).magnitude /
                    (controllingHand.shoulder.transform.position - controllingHand.transform.position).magnitude;
        }
    }
}
