using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMesh : MonoBehaviour
{
    public int lineCount = 100;
    public float arcAngle = 90;
    public float maxDist = 100;

    private void FixedUpdate()
    {
        return;
        var nLines=lineCount+1;
        var ranges = new float[nLines];
        var halfAngle = -1 * arcAngle / 2;
        var segmentArcAngle = arcAngle / lineCount;
        for (var l=0;l<nLines;l++) {
            var shootVec = transform.rotation * Quaternion.AngleAxis(halfAngle + l*segmentArcAngle, Vector3.forward) * Vector3.up;
            RaycastHit hit;
            Debug.DrawRay(transform.position,shootVec,Color.blue);
            if (Physics.Raycast(new Ray(transform.position, shootVec), out hit, maxDist)) {
                Debug.DrawLine(transform.position, hit.point, Color.blue);
                ranges[l]=hit.distance;
            }
            else ranges[l]=maxDist;
        }
    }
}
