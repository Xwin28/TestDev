using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class S_DrawSlince : MonoBehaviour
    {

    public Transform[] controlPoints;

    public int segments = 10;
    public float lineWidth = 0.1f;
    public Color lineColor = Color.white;

    private void OnDrawGizmos()
    {
        // Draw the spline using Gizmos
        Gizmos.color = lineColor;

        for (int i = 0; i < segments; i++)
        {
            float t1 = (float)i / (float)segments;
            float t2 = (float)(i + 1) / (float)segments;

            Vector3 p1 = GetSplinePoint(t1);
            Vector3 p2 = GetSplinePoint(t2);

            Gizmos.DrawLine(p1, p2);
        }
    }

    private Vector3 GetSplinePoint(float t)
    {
        // Calculate the position of a point on the spline at time t
        int numPoints = controlPoints.Length;

        if (numPoints < 2)
        {
            Debug.LogError("Spline requires at least two control points!");
            return Vector3.zero;
        }

        if (t <= 0f)
        {
            return controlPoints[0].position;
        }
        else if (t >= 1f)
        {
            return controlPoints[numPoints - 1].position;
        }

        float totalTime = 0f;
        for (int i = 0; i < numPoints - 1; i++)
        {
            Vector3 p1 = controlPoints[i].position;
            Vector3 p2 = controlPoints[i + 1].position;

            float distance = Vector3.Distance(p1, p2);
            float segmentTime = distance / Mathf.Max(distance, lineWidth);

            if (t >= totalTime && t <= totalTime + segmentTime)
            {
                float segmentT = (t - totalTime) / segmentTime;
                return Vector3.Lerp(p1, p2, segmentT);
            }

            totalTime += segmentTime;
        }

        return Vector3.zero;
    }

}//end class

