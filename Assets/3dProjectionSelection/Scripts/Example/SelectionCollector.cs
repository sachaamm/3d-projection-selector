﻿using System.Collections.Generic;
using _3dProjectionSelection.Scripts.Math;
using _3dProjectionSelection.Scripts.Physics;
using UnityEngine;

namespace _3dProjectionSelection.Scripts.Example
{
    public static class SelectionCollector
    {
        public static List<GameObject> CollectFromPolygonProc(GeoPolygonProc geoPolygonProcess, List<GameObject> gameObjects)
        {
            List<GameObject> selectionInProjection = new List<GameObject>();
            
            foreach (var go in gameObjects)
            {
                if (PointInsidePolygonProjection(geoPolygonProcess, go.transform.position))
                {
                    selectionInProjection.Add(go);
                }
            }

            return selectionInProjection;
        }

        public static List<GameObject> CollectFromPolygonProcBoundingBox(GeoPolygonProc geoPolygonProcess,
            List<GameObjectWithCollider> gameObjectsWithBounds)
        {
            List<GameObject> selectionInProjection = new List<GameObject>();
            
            foreach (GameObjectWithCollider goWithBound in gameObjectsWithBounds)
            {
                bool allBoundingBoxInProjection = true;
                
                foreach (var boundsPoint in goWithBound.GetBoundsPoints())
                {
                    if (!PointInsidePolygonProjection(geoPolygonProcess, boundsPoint))
                    {
                        allBoundingBoxInProjection = false;
                    }
                }
                
                if(allBoundingBoxInProjection) selectionInProjection.Add(goWithBound.gameObject);
                
            }

            return selectionInProjection;
        }
        
        static bool PointInsidePolygonProjection(GeoPolygonProc geoPolygonProcess, Vector3 p)
        {
            if (geoPolygonProcess.PointInside3DPolygon(p.x, p.y, p.z))
            {
                return true;
            }

            return false;
        }
    }
}