using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldCleaner : MonoBehaviour
{
    private enum ZoneType { Box, Sphere }

    [Header("Configuration")]
    [SerializeField] private ZoneType zoneType;
    [SerializeField] private Vector3 scale;
    [SerializeField] private float radius;

    public void ClearEnemiesInZone()
    {
        Collider[] colliders = new Collider[0];

        if (zoneType == ZoneType.Box)
        {
            colliders
                = Physics.OverlapBox(transform.position, scale / 2f, transform.rotation, Enemy.enemyLayer);

        }
        else if (zoneType == ZoneType.Sphere)
        {
            colliders
                = Physics.OverlapSphere(transform.position, radius, Enemy.enemyLayer);
        }

        foreach (var collider in colliders)
            Destroy(collider.gameObject);
    }

    private void OnDrawGizmos()
    {
        if(zoneType == ZoneType.Sphere)
        {
            Gizmos.color = new Color(0f, 0f, 1f, .3f);
            Gizmos.DrawSphere(transform.position, radius);
        }
        else if (zoneType == ZoneType.Box)
        {
            Gizmos.color = new Color(0f, 0f, 1f, .3f);
            Gizmos.DrawCube(transform.position, scale);
        }

    }
}
