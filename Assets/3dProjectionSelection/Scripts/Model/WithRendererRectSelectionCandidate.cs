using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _3dProjectionSelection.Scripts.Model
{
    public class WithRendererRectSelectionCandidate : RectSelectionCandidate
    {
        public WithRendererRectSelectionCandidate(GameObject go) : base(go)
        {
        }
        protected override List<Bounds> GetBounds()
        {
            List<Renderer> renderers = new List<Renderer>();
            renderers = go.GetComponentsInChildren<Renderer>().ToList();
            renderers = renderers.Concat(go.GetComponents<Renderer>()).ToList();
            
            List<Bounds> bounds = new List<Bounds>();
            
            foreach (var renderer in renderers)
            {
                bounds.Add(renderer.bounds);
            }
            
            return bounds;
        }

        
    }
}