using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _3dProjectionSelection.Scripts.Model
{
    public class WithColliderRectSelectionCandidate : RectSelectionCandidate
    {
        public WithColliderRectSelectionCandidate(GameObject go) : base(go)
        {
        }
        protected override List<Bounds> GetBounds()
        {
            List<Bounds> bounds = new List<Bounds>();
            var colliders = go.GetComponentsInChildren<Collider>();
            colliders = colliders.Concat(go.GetComponents<Collider>()).ToArray();
            foreach (var collider in colliders)
            {
                bounds.Add(collider.bounds);
            }

            return bounds;
        }

        
    }
}