using _3dProjectionSelection.Scripts.Math;
using UnityEngine;

namespace _3dProjectionSelection.Scripts.Viewport
{
    public class ViewportToWorldPoint : MonoBehaviour
    {
        public GameObject debugSphere;

        Vector3 a, b, c, d, e, f, g, h;

        GameObject aa, bb, cc, dd, ee, ff, gg, hh;


        public static UnityEngine.Camera ViewportCamera;
        public static float EndZ = 500;
        
        private void Start()
        {
            if (ViewportCamera == null)
            {
                ViewportCamera = UnityEngine.Camera.main;
            }
            
            ee = GameObject.Instantiate(debugSphere);
            ee.transform.name = "ee";
            ff = GameObject.Instantiate(debugSphere);
            ff.transform.name = "ff";
            gg = GameObject.Instantiate(debugSphere);
            gg.transform.name = "gg";
            hh = GameObject.Instantiate(debugSphere);
            hh.transform.name = "hh";
        }
        
        private void Update()
        {
            
            if (ViewportCamera)
            {
                debugSphere.transform.position = ViewportCamera.ViewportToWorldPoint(new Vector3(1, 1, 200));
                
                SetPos(ee, GetPosFromCameraViewport(0, 1, EndZ, ref e));
                SetPos(ff, GetPosFromCameraViewport(1, 1, EndZ, ref f));
                SetPos(gg, GetPosFromCameraViewport(1, 0, EndZ, ref g));
                SetPos(hh, GetPosFromCameraViewport(0, 0, EndZ, ref h));
            }
        }

        private Vector3 GetPosFromCameraViewport(float x, float y, float z, ref Vector3 v)
        {
            v = ViewportCamera.ViewportToWorldPoint(new Vector3(x, y, z));
            return ViewportCamera.ViewportToWorldPoint(new Vector3(x, y, z));
        }

        private void SetPos(GameObject go, Vector3 v)
        {
            go.transform.position = v;
        }
        
        public Vector3[] GetCameraBounds()
        {
            Vector3[] bounds = new Vector3[4];

            bounds[0] = e;
            bounds[1] = f;
            bounds[2] = g;
            bounds[3] = h;
            
            return bounds;
        }

        public Vector3[] GetCameraProjectionBoundsSelection(Vector2 startDragMousePos, Vector2 releaseMousePos, float startZ = 2)
        {
            
            Vector3 leftTopPoint = ViewportCamera.ScreenToWorldPoint(new Vector3(startDragMousePos.x, startDragMousePos.y, startZ));
            Vector3 rightBottomPoint = ViewportCamera.ScreenToWorldPoint(new Vector3(releaseMousePos.x, releaseMousePos.y, startZ));

            Vector3 diff = rightBottomPoint - leftTopPoint;
            Vector3 rightTopPoint = rightBottomPoint + new Vector3(0, -diff.y, 0);
            Vector3 leftBottomPoint = leftTopPoint + new Vector3(0, diff.y, 0);

            Vector3[] cameraBounds = GetCameraBounds();

            // -1 // 1
            float mapLeftTopPointX = GeoMath.Remap(startDragMousePos.x, 0, Screen.width, 0, 1);
            float mapLeftTopPointY = GeoMath.Remap(startDragMousePos.y, 0, Screen.height, 0, 1);
            float mapRightBottomPointX = GeoMath.Remap(releaseMousePos.x, 0, Screen.width, 0, 1);
            float mapRightBottomPointY = GeoMath.Remap(releaseMousePos.y, 0, Screen.height, 0, 1);

            Vector3 widthDiff = cameraBounds[1] - cameraBounds[0];
            Vector3 heightDiff = cameraBounds[3] - cameraBounds[0];

            Vector3 projectedLeftTopPoint = cameraBounds[0];

            // print("map left top p y" + mapLeftTopPointY);

            Vector3 lerpProjectLeftTopX = cameraBounds[0] + widthDiff * mapLeftTopPointX;
            Vector3 lerpProjectLeftBottomX = cameraBounds[3] + widthDiff * mapLeftTopPointX;
            Vector3 lerpProjectLeftBottomY = cameraBounds[3] - heightDiff * mapLeftTopPointY;
            Vector3 lerpProjectLeftTopY = cameraBounds[3] - heightDiff * mapRightBottomPointY;

            Vector3 lerpProjectRightTopX = cameraBounds[0] + widthDiff * mapRightBottomPointX;
            Vector3 lerpProjectRightBottomX = cameraBounds[3] + widthDiff * mapRightBottomPointX;
            Vector3 lerpProjectRightTopY = cameraBounds[2] - heightDiff * mapRightBottomPointY;
            Vector3 lerpProjectRightBottomY = cameraBounds[2] - heightDiff * mapLeftTopPointY;

            Vector3 leftBottomPointInProjection = lerpProjectLeftTopX + heightDiff * (1 - mapLeftTopPointY);
            Vector3 leftTopPointInProjection = lerpProjectLeftBottomX - heightDiff * (mapRightBottomPointY);
            Vector3 rightBottomPointInProjection = lerpProjectRightTopX + heightDiff * (1 - mapLeftTopPointY);
            Vector3 rightTopPointInProjection = lerpProjectRightBottomX - heightDiff * (mapRightBottomPointY);

            Vector3[] vertices = {
                leftTopPoint,rightTopPoint,rightBottomPoint,leftBottomPoint,
                leftTopPointInProjection,rightTopPointInProjection,rightBottomPointInProjection,leftBottomPointInProjection,
            };
            
            // CreateDebugPoint(leftTopPoint, "Left-Top-Point");
            // CreateDebugPoint(leftBottomPoint, "Left-Bottom-Point");
            // CreateDebugPoint(rightTopPoint, "Right-Top-Point");
            // CreateDebugPoint(rightBottomPoint, "Right-Bottom-Point");
            //
            // CreateDebugPoint(leftTopPointInProjection, "Left-Top-pp");
            // CreateDebugPoint(leftBottomPointInProjection, "Left-Bottom-pp");
            // CreateDebugPoint(rightTopPointInProjection, "Right-Top-pp");
            // CreateDebugPoint(rightBottomPointInProjection, "Right-Bottom-pp");

           
            // print(leftTopPoint);
            // print(rightTopPoint);
            // print(rightBottomPoint);
            // print(leftBottomPoint);
            // print(leftTopPointInProjection);
            // print(rightTopPointInProjection);
            // print(rightBottomPointInProjection);
            // print(leftBottomPointInProjection);
            

            // Mesh m = new Mesh();
            // GameObject n = new GameObject("DebugColliderProjection");
            //
            // MeshFilter mf = n.AddComponent<MeshFilter>();
            // MeshRenderer mr = n.AddComponent<MeshRenderer>();
            // mf.mesh = m;
            //
            // mr.material = mat;


            return vertices;
        }

        void CreateDebugPoint(Vector3 v, string name)
        {
            GameObject debug = GameObject.Instantiate(debugSphere);
            debug.name = name;
            debug.transform.position = v;
        }

    }

}