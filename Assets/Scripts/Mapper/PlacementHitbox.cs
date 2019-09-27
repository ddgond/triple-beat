using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mapper
{
    public class PlacementHitbox : MonoBehaviour
    {
        public NoteColor color;

        private MeshRenderer meshRenderer;

        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.Equals(gameObject))
            {
                meshRenderer.enabled = true;
            }
            else
            {
                meshRenderer.enabled = false;
            }
        }
    }
}