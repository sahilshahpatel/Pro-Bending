using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTracker : MonoBehaviour
{
    private Vector3[] velQ;
    private Vector3[] posQ;
    public int posSize;
    public int velSize;
    private int pCounter = 0;
    private int vCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        posQ = new Vector3[posSize];
        velQ = new Vector3[velSize];

        for(int i = 0; i<posSize; i++)
        {
            posQ[i] = Vector3.zero;
        }
        for (int i = 0; i < velSize; i++)
        {
            velQ[i] = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        posQ[pCounter] = this.gameObject.transform.position;

        velQ[vCounter] = (posQ[pCounter] - posQ[(pCounter - 1 + posSize) % posSize]) / Time.deltaTime;

        //Debug.Log(this.gameObject.name + 100*posQ[pCounter]/Time.deltaTime + " - " + 100*posQ[(pCounter - 1 + posSize) % posSize]/Time.deltaTime + " = " + 100*velQ[vCounter]);

        pCounter++;
        pCounter %= posSize;

        vCounter++;
        vCounter %= velSize;

        /*
        Vector3 avgVel = getAvgVel();
        float minVelToPrint = 1f;
        if (avgVel.magnitude > minVelToPrint) {
            Debug.Log("Average Velocity (" + this.gameObject.name + "): " + avgVel.magnitude + ", Vector: " + avgVel);
        }
        //*/
    }

    public Vector3 getAvgVel()
    {
        Vector3 avgVel = Vector3.zero;
        foreach(Vector3 v in velQ)
        {
            avgVel += v;
        }
        avgVel /= velSize;

        return avgVel;
    }
    public Vector3 getCurrentVel()
    {
        return velQ[(vCounter - 1 + velSize) % velSize];
    }
}
