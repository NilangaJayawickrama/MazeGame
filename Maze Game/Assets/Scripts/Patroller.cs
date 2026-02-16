using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Patroller : MonoBehaviour
{
    // Const for rotation
    private const float rotationSlerpAmount = .68f;

    // References
    [Header("References")]
    public Transform trans; // Reference to own Transform
    public Transform modelHolder; // Reference to model Transform

    // Stats
    [Header("Stats")]
    public float movespeed = 30; // Movement speed

    // Private variables
    private int currentPointIndex; // Index of current patrol point
    private Transform currentPoint; // Reference to current patrol point
    private Transform[] patrolPoints; // Array of patrol points

    //Returns a List containing the Transform of each child with a name that starts with "Patrol Point (".
    private List<Transform> GetUnsortedPatrolPoints()
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();

        var points = new List<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].gameObject.name.StartsWith("Patrol Point ("))
            {
                points.Add(children[i]);
            }
        }

        return points;
    }

    // Method to set the current patrol point
    private void SetCurrentPatrolPoint(int index)
    {
        currentPointIndex = index;
        currentPoint = patrolPoints[index];
    }

    void Start()
    {
        List<Transform> points = GetUnsortedPatrolPoints();

        if (points.Count > 0)
        {
            patrolPoints = new Transform[points.Count];

            for (int i = 0; i < points.Count; i++)
            {
                Transform point = points[i];

                int closinParanthesisIndex = point.gameObject.name.IndexOf(')');

                string indexSubstring = point.gameObject.name.Substring(14, closinParanthesisIndex - 14);

                int index = Convert.ToInt32(indexSubstring);

                patrolPoints[index] = point;

                point.SetParent(null);

                point.gameObject.hideFlags = HideFlags.HideInHierarchy;
            }

            SetCurrentPatrolPoint(0);
        }
    }

    void Update()
    {
        // Only operate if we have a currentPoint
        if (currentPoint != null)
        {
            // Move root GameObject towards the current point
            trans.position = Vector3.MoveTowards(trans.position, currentPoint.position, movespeed * Time.deltaTime);

            // If we're on top of the point already, change the current point
            if (trans.position == currentPoint.position)
            {
                // If we're at the last patrol point...
                if (currentPointIndex >= patrolPoints.Length - 1)
                {
                    // ...we'll set to the first patrol point (double back)
                    SetCurrentPatrolPoint(0);
                }
                else // Else if we're not at the last patrol point
                {
                    SetCurrentPatrolPoint(currentPointIndex + 1); // Go to the index after the current
                }
            }
            else // Else if we're not on the point yet, rotate the model towards it
            {
                Quaternion lookRotation = Quaternion.LookRotation((currentPoint.position - trans.position).normalized);
                modelHolder.rotation = Quaternion.Slerp(modelHolder.rotation, lookRotation, rotationSlerpAmount);
            }
        }
    }
}
