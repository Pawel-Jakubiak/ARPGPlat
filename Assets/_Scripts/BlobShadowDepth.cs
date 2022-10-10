using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlobShadowDepth : MonoBehaviour
{
    public LayerMask layerMasks;
    public float sphereRadius;
    public float maxDistance;

    public float test;

    private DecalProjector projector;

    private void Awake()
    {
        projector = GetComponent<DecalProjector>();
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Physics.SphereCast(transform.position + Vector3.up, sphereRadius, Vector3.down, out hit, maxDistance, layerMasks);

        float distance = hit.distance == 0 ? 15f : hit.distance;
        float depth = Mathf.Clamp(distance * 3f, 2f, 15f);
        float pivotZ = .5f * depth;

        projector.size = new Vector3(projector.size.x, projector.size.y, depth);
        projector.pivot = new Vector3(projector.pivot.x, projector.pivot.y, pivotZ);
    }
}
