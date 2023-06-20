using System;
using System.Collections.Generic;
using System.Linq;
using _3dProjectionSelection.Scripts.Manager;
using _3dProjectionSelection.Scripts.Model;
using UnityEngine;

namespace _3dProjectionSelection.Scripts.Example
{
    public class ThreeDimensionalProjectionSelectionExample : MonoBehaviour
    {
        [SerializeField] private ThreeDProjectionSelectorManager projSelectorManager;
        
        public List<GameObject> candidates;
        
        private void Start()
        {
            projSelectorManager.OnInitRectSelectionCalculation += OnInitRectSelectionCalculation;
            projSelectorManager.OnSelectionDone += OnSelectionDone;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                projSelectorManager.enabled = !projSelectorManager.enabled;
            }
        }

        /// <summary>
        /// When Rect Selection area is settled, define the rect selection candidates.
        /// </summary>
        private void OnInitRectSelectionCalculation()
        {
            projSelectorManager.CollectingElementsInProjectionCalculation(candidates);
        }
        
        /// <summary>
        /// When the rect selection is done, do whatever you want with the result.
        /// </summary>
        /// <param name="rectSelectionResult"></param>
        private void OnSelectionDone(RectSelectionOutput rectSelectionResult)
        {
            SetGameObjectsColor(candidates, Color.gray);

            foreach (int indexOfCandidate in rectSelectionResult.CandidatesInProjections)
            {
                SetGameObjectColor(candidates[indexOfCandidate], Color.magenta);
            }
        }

        void SetGameObjectsColor(List<GameObject> list, Color c)
        {
            foreach (var go in list)
            {
                SetGameObjectColor(go, c);
            }
        }

        void SetGameObjectColor(GameObject go, Color c)
        {
            var meshRenderer = go.GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                meshRenderer.material.color = c;
            }
        }
    }
}