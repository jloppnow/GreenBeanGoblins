using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    [SerializeField]
    LayerMask layers;

    [SerializeField]
    float radius = 0.0f;

    [SerializeField]
    List<GridNode> connectingNodes = null;

    public void FindNearbyNodes(float radius, Vector3 plane, float radiusLimit = float.MaxValue)
    {
        Collider[] nodes = Physics.OverlapBox(transform.position, plane * radius, transform.rotation, layers);
        foreach (Collider node in nodes)
        {
            if (node.gameObject != gameObject)
            {
                float dist = Vector3.Distance(transform.position, node.transform.position);
                if (dist <= radiusLimit)
                    connectingNodes.Add(node.GetComponent<GridNode>());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.green;
        foreach(GridNode node in connectingNodes)
        {
            Gizmos.DrawLine(transform.position + (Vector3.down * radius), node.transform.position + (Vector3.up * radius));
        }
    }
}
