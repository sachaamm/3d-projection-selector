﻿using System.Collections.Generic;
using UnityEngine;

namespace _3dProjectionSelection.Scripts.Physics
{
    [RequireComponent(typeof(Collider))]
    public class GameObjectWithCollider : MonoBehaviour
    {
        private Collider _collider;

        private void Start()
        {
            var colliders = GetComponents<Collider>();
            
            if(colliders.Length > 1) Debug.LogError("Attach only one collider to your component");

            _collider = colliders[0];

            foreach (var boundsPoint in GetBoundsPoints())
            {
                Instantiate(DebugBounds.Singleton.debugSphere, boundsPoint, transform.rotation, null);
            }
        }

        public Bounds GetBounds()
        {
            return _collider.bounds;
        }

        public List<Vector3> GetBoundsPoints()
        {
            List<Vector3> points = new List<Vector3>();
            Bounds bounds = GetBounds();
            
            float dx = (bounds.max.x - bounds.min.x);
            float dy = (bounds.max.y - bounds.min.y);
            float dz = (bounds.max.z - bounds.min.z);

            dx = transform.localScale.x;
            dy = transform.localScale.y;
            dz = transform.localScale.z;

            float s = 1;

            var ptA = bounds.center + new Vector3(-dx / 2, -dy / 2, -dz / 2) * s;
            var ptB = bounds.center + new Vector3(-dx / 2, -dy / 2, dz / 2) * s;
            var ptC = bounds.center + new Vector3(dx / 2, -dy / 2, dz / 2) * s;
            var ptD = bounds.center + new Vector3(dx / 2, -dy / 2, -dz / 2) * s;
            
            var ptE = bounds.center + new Vector3(-dx / 2, dy / 2, -dz / 2) * s;
            var ptF = bounds.center + new Vector3(-dx / 2, dy / 2, dz / 2) * s;
            var ptG = bounds.center + new Vector3(dx / 2, dy / 2, dz / 2) * s;
            var ptH = bounds.center + new Vector3(dx / 2, dy / 2, -dz / 2) * s;
            
            var rotation = transform.rotation;
            var r = rotation.eulerAngles;

            points.Add(RotatePointAroundPivot(ptA, bounds.center, r));
            points.Add(RotatePointAroundPivot(ptB, bounds.center, r));
            points.Add(RotatePointAroundPivot(ptC, bounds.center, r));
            points.Add(RotatePointAroundPivot(ptD, bounds.center, r));
            points.Add(RotatePointAroundPivot(ptE, bounds.center, r));
            points.Add(RotatePointAroundPivot(ptF, bounds.center, r));
            points.Add(RotatePointAroundPivot(ptG, bounds.center, r));
            points.Add(RotatePointAroundPivot(ptH, bounds.center, r));
            
            return points;
        }
        
        Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            // return point;
            Vector3 dir = point - pivot;
            dir = Quaternion.Euler(angles) * dir;
            point = (dir ) + pivot;
            return point;
        }
    }
}