using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColliderCtrl : MonoBehaviour
{
    SkinnedMeshRenderer meshRenderer;
    MeshCollider collider;

    float delay = 0.1f;

    private void Awake()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        collider = GetComponent<MeshCollider>();
        
        StartCoroutine(UpdateCollider());
    }

    IEnumerator UpdateCollider()
    {
        while (true)
        {
            Mesh colliderMesh = new Mesh();
            meshRenderer.BakeMesh(colliderMesh);
            collider.sharedMesh = null;
            collider.sharedMesh = colliderMesh;

            yield return new WaitForSeconds(delay);
        }
    }
}
