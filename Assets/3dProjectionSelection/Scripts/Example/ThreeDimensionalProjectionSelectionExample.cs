using System.Collections.Generic;
using System.Linq;
using _3dProjectionSelection.Scripts.Math;
using _3dProjectionSelection.Scripts.Physics;
using _3dProjectionSelection.Scripts.Viewport;
using UnityEngine;

namespace _3dProjectionSelection.Scripts.Example
{
    public class ThreeDimensionalProjectionSelectionExample : MonoBehaviour
    {
        public GameObject selectionRectPanel; // Rect selection is a panel
        RectTransform rt; // transform for the rect selection


        public ViewportToWorldPoint ViewportToWorldPoint;

        Vector2 _startDragMousePos = new Vector2();

        public Material debugMat;

        private static KeyCode holdSelectKey = KeyCode.LeftControl;

        private void Start()
        {
            rt = selectionRectPanel.GetComponent<RectTransform>();
        }

        private void Update()
        {
            // When i start my rect selection
            if (Input.GetKey(holdSelectKey) && Input.GetMouseButtonDown(0))
            {
                _startDragMousePos = Input.mousePosition;
                selectionRectPanel.SetActive(true);
                rt.transform.position = Input.mousePosition;
            }

            // When i hold my selection
            if (Input.GetKey(holdSelectKey) && Input.GetMouseButton(0))
            {
                Vector2 diff = (Vector2)Input.mousePosition - _startDragMousePos;
                Vector2 delta = new Vector2(diff.x, diff.y);
                rt.sizeDelta = delta;
                rt.transform.position = _startDragMousePos + (delta / 2);
            }

            if (Input.GetKey(holdSelectKey) && Input.GetMouseButtonUp(0))
            {
                // When the rect selection is determined

                List<GeoPoint> geoPoints = new List<GeoPoint>();

                Vector2 currentMousePos = Input.mousePosition;

                int a = 0;
                float startZ = 1;

                foreach (Vector3 v in ViewportToWorldPoint.GetCameraProjectionBoundsSelection(_startDragMousePos,
                    currentMousePos, startZ))
                {
                    geoPoints.Add(new GeoPoint(v));

                    // var debugSphere = Instantiate(DebugBounds.Singleton.debugSphere, v, Camera.main.transform.rotation);
                    // debugSphere.name = "CamPoint " + a;

                    a++;
                }

                GeoPolygonProc geoPolygonProcess = new GeoPolygonProc(new GeoPolygon(geoPoints));

                List<GameObject> candidates = new List<GameObject>();

                foreach (Transform child in GameObject.Find("Objects").transform)
                {
                    candidates.Add(child.gameObject);
                }
                

                SetGameObjectsColor(candidates, Color.gray);

                var candidatesInProjection = 
                    SelectionCollector.CollectFromPolygonProcBoundingBox(geoPolygonProcess, 
                        candidates.Select(go => go.GetComponent<GameObjectWithCollider>()).ToList());

                Debug.Log("candidates in projection " + candidatesInProjection.Count);

                SetGameObjectsColor(candidatesInProjection, Color.magenta);
                
                selectionRectPanel.SetActive(false);
            }
        }

        void SetGameObjectsColor(List<GameObject> list, Color c)
        {
            foreach (var go in list)
            {
                var meshRenderer = go.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.material.color = c;
                }
            }
        }
    }
}