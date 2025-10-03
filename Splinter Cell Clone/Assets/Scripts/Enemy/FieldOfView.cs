using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] float radius = 7f;
    [SerializeField] float angle = 60f;
    [SerializeField] LayerMask targetLayers;
    [SerializeField] LayerMask obstacleLayers;
    [SerializeField] Color debugColor = Color.yellow;

    public List<GameObject> VisibleObjects { get; private set; } = new();

    public event Action OnVisibleObjectsUpdated;

    void Start()
    {
        StartCoroutine(ScanWithDelay(0.2f));
    }

    IEnumerator ScanWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            Scan();
        }
    }

    void Scan()
    {
        //clear list of visible objects
        List<GameObject> temp = new();

        Collider[] objectsInRadius = new Collider[20];
        //get objects in radius
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, objectsInRadius, targetLayers, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < count; i++)
        {
            Transform objectInRadius = objectsInRadius[i].transform;
            Vector3 directionToObject = (objectInRadius.position - transform.position).normalized;

            //check if object is in angle
            if (Vector3.Angle(transform.forward, directionToObject) > angle / 2)
                continue;

            //check if any obstacle is in the way
            if (IsObstacleInWay(objectInRadius))
                continue;

            temp.Add(objectInRadius.gameObject);
        }

        if (!AreListsEqual(temp, VisibleObjects))
        {
            VisibleObjects = temp;
            OnVisibleObjectsUpdated?.Invoke();
        }
    }

    bool IsObstacleInWay(Transform targetObject)
    {
        Vector3 directionToObject = (targetObject.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, directionToObject, radius, obstacleLayers, QueryTriggerInteraction.Ignore))
            return true;

        return false;
    }

    bool AreListsEqual(List<GameObject> a, List<GameObject> b)
    {
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i]) return false;
        }
        return true;
    }

    void OnDrawGizmosSelected()
    {
        Handles.color = debugColor;

        Vector3 rotatedVector = Quaternion.AngleAxis(-angle / 2, Vector3.up) * transform.forward;
        Handles.DrawSolidArc(transform.position, Vector3.up, rotatedVector, angle, radius);
    }
}
