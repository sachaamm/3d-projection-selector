using System;
using System.Collections.Generic;
using System.Linq;
using _3dProjectionSelection.Scripts.Example;
using _3dProjectionSelection.Scripts.Math;
using _3dProjectionSelection.Scripts.Model;
using _3dProjectionSelection.Scripts.Viewport;
using UnityEngine;

namespace _3dProjectionSelection.Scripts.Manager
{
    [RequireComponent(typeof(ViewportToWorldPoint))]
    public class ThreeDProjectionSelectorManager : MonoBehaviour
    {
        public CollectingStrategyEnum collectingStrategyEnum;
        
        [Tooltip("If you want, you can add additional keys to start selection process")]
        [SerializeField]
        private List<KeyCode> startSelectionAdditionalKeys = new List<KeyCode>();

        [SerializeField] private RectTransform selectionPanel;
        
        Vector2 _startDragMousePos = new Vector2();

        private ViewportToWorldPoint _viewportToWorldPoint;
        
        public delegate void OnInitRectSelectionCalculationEvent();
        public delegate void OnSelectionDoneEvent(RectSelectionOutput rectSelectionOutput);

        public OnInitRectSelectionCalculationEvent OnInitRectSelectionCalculation;
        public OnSelectionDoneEvent OnSelectionDone;
        
        bool IsInSelectingDefinitionProcess()
        {
            return startSelectionAdditionalKeys.All(Input.GetKey) && Input.GetMouseButton(0);
        }

        bool IsStartingSelectingDefinitionProcess()
        {
            return IsInSelectingDefinitionProcess() && Input.GetMouseButtonDown(0);
        }

        bool IsEndingSelectingDefinitionProcess()
        {
            return Input.GetMouseButtonUp(0);
        }

        private void Awake()
        {
            _viewportToWorldPoint = GetComponent<ViewportToWorldPoint>();
        }

        private void OnDisable()
        {
            selectionPanel.gameObject.SetActive(false);
        }

        private void Update()
        {
            bool inSelectingDefinition = IsInSelectingDefinitionProcess();
            
            selectionPanel.gameObject.SetActive(inSelectingDefinition);

            // When i start my rect selection
            if (IsStartingSelectingDefinitionProcess())
            {
                InitRectSelection();
            }
            
            // When i hold my selection
            if (inSelectingDefinition)
            {
                RectSelectionProcess();
            }

            if (IsEndingSelectingDefinitionProcess())
            {
                EndRectSelection();
            }
        }

        List<RectSelectionCandidate> AsCalculationCandidates(List<GameObject> candidates)
        {
            List<RectSelectionCandidate> _candidates = new List<RectSelectionCandidate>();
            
            switch (collectingStrategyEnum)
            {
                case CollectingStrategyEnum.FromColliders: 
                    _candidates = candidates
                        .Select(c => new WithColliderRectSelectionCandidate(c))
                        .Cast<RectSelectionCandidate>().ToList();
                    break;
                
                case CollectingStrategyEnum.FromRenderers:
                    _candidates = candidates
                        .Select(c => new WithRendererRectSelectionCandidate(c))
                        .Cast<RectSelectionCandidate>().ToList();
                    break;
                
                default:
                    throw new NotSupportedException("Not Supported Collecting Strategy");
                    break;
            }

            return _candidates;
        }

        void InitRectSelection()
        {
            _startDragMousePos = Input.mousePosition;
            selectionPanel.transform.position = Input.mousePosition;
        }

        void RectSelectionProcess()
        {
            Vector2 diff = (Vector2)Input.mousePosition - _startDragMousePos;

            // Debug.Log(" " + diff);
                
            float absDiffX = Mathf.Abs(diff.x);
                
            if (diff.y < 0)
            {
                selectionPanel.sizeDelta = new Vector2(absDiffX, -diff.y);
                selectionPanel.transform.position = _startDragMousePos + (diff / 2);
            }
            else
            {
                selectionPanel.sizeDelta = new Vector2(absDiffX, diff.y);
                selectionPanel.transform.position = _startDragMousePos + (diff / 2);
            }
        }

        void EndRectSelection()
        {
            OnInitRectSelectionCalculation?.Invoke();
        }

        public void CollectingElementsInProjectionCalculation(List<GameObject> candidates)
        {
            var calculationCandidates = AsCalculationCandidates(candidates);
            
            // When the rect selection is determined

            List<GeoPoint> geoPoints = new List<GeoPoint>();

            Vector2 currentMousePos = Input.mousePosition;

            int a = 0;
            float startZ = 1;

            foreach (Vector3 v in _viewportToWorldPoint.GetCameraProjectionBoundsSelection(_startDragMousePos,
                         currentMousePos, startZ))
            {
                geoPoints.Add(new GeoPoint(v));

                // var debugSphere = Instantiate(DebugBounds.Singleton.debugSphere, v, Camera.main.transform.rotation);
                // debugSphere.name = "CamPoint " + a;
                a++;
            }

            GeoPolygonProc geoPolygonProcess = new GeoPolygonProc(new GeoPolygon(geoPoints));
            
            List<int> candidatesInProjection = 
                SelectionCollector.CollectFromCandidates(geoPolygonProcess, calculationCandidates);
            
            RectSelectionOutput rectSelectionOutput = new RectSelectionOutput();
            rectSelectionOutput.CandidatesInProjections = candidatesInProjection;
            
            Vector2 diff = (Vector2)Input.mousePosition - _startDragMousePos;
            if (diff.magnitude < 3f)
            {
                candidatesInProjection.Clear();
            }
            
            OnSelectionDone?.Invoke(rectSelectionOutput);
            
            // SetGameObjectsColor(candidatesInProjection, Color.magenta);
        }
    }
}