using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnArea : MonoBehaviour
{
    [Range(1f, 15f)]
    public float spawnRange;

    [Range(1f, 100f)]
    public int spawnDensity;

    /// <summary>
    /// Draws Red Spheres around spawn area
    /// </summary>
    void OnDrawGizmos()
    {
        Color spawnColor = Color.red;
        spawnColor.a = Utilities.MapToInterval(spawnDensity,1,50,0.1f,0.75f);
        Gizmos.color = spawnColor;
        Gizmos.DrawSphere(transform.position, spawnRange);
    }

}