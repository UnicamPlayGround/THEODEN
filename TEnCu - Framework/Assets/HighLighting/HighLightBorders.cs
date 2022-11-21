using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighLightBorders : MonoBehaviour
{
    [SerializeField]
    private Material material;
    [SerializeField]
    private Material shader;

    private Renderer[] renderers;
    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

    [SerializeField, HideInInspector]
    private List<Mesh> bakeKeys = new List<Mesh>();

    [SerializeField, HideInInspector]
    private List<ListVector3> bakeValues = new List<ListVector3>();

    private class ListVector3
    {
        public List<Vector3> data;
    }
    public List<GameObject> highLightedObjects;

    private void Awake()
    {
        highLightedObjects = new List<GameObject>();

    }
    public void ApplyHighLight(GameObject gameObject)
    {
        if (!highLightedObjects.Contains(gameObject))
        {
            LoadSmoothNormals(gameObject);
            renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                var materials = renderer.sharedMaterials.ToList();
                materials.Add(material);
                materials.Add(shader);
                renderer.materials = materials.ToArray();
            }
            highLightedObjects.Add(gameObject);
        }

    }

    public void RemoveAllHighLight()
    {
        GameObject[] tmpHighLight = highLightedObjects.ToArray();
        foreach (GameObject hightLightObject in tmpHighLight)
        {
            RemoveHighLight(hightLightObject);
        }
    }
    
    public void RemoveHighLight(GameObject gameObject)
    {
        if (highLightedObjects.Contains(gameObject))
        {
            renderers = gameObject.GetComponentsInChildren<Renderer>();

            foreach (var renderer in renderers)
            {
                var materials = renderer.sharedMaterials.ToList();
                materials.Remove(material);
                materials.Remove(shader);
                renderer.materials = materials.ToArray();
            }
            highLightedObjects.Remove(gameObject);
        }
    }


    public void LoadSmoothNormals(GameObject gameObject)
    {
        // Retrieve or generate smooth normals
        foreach (var meshFilter in gameObject.GetComponentsInChildren<MeshFilter>())
        {

            // Skip if smooth normals have already been adopted
            if (!registeredMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            // Retrieve or generate smooth normals
            var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
            var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

            // Store smooth normals in UV3
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);

            // Combine submeshes
            var renderer = meshFilter.GetComponent<Renderer>();

            if (renderer != null)
            {
                CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
            }
        }
    }
    List<Vector3> SmoothNormals(Mesh mesh)
    {

        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        // Copy normals to a new list
        var smoothNormals = new List<Vector3>(mesh.normals);

        // Average normals for grouped vertices
        foreach (var group in groups)
        {

            // Skip single vertices
            if (group.Count() == 1)
            {
                continue;
            }

            // Calculate the average normal
            var smoothNormal = Vector3.zero;

            foreach (var pair in group)
            {
                smoothNormal += smoothNormals[pair.Value];
            }

            smoothNormal.Normalize();

            // Assign smooth normal to each vertex
            foreach (var pair in group)
            {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }

        return smoothNormals;
    }

    void CombineSubmeshes(Mesh mesh, Material[] materials)
    {

        // Skip meshes with a single submesh
        if (mesh.subMeshCount == 1)
        {
            return;
        }

        // Skip if submesh count exceeds material count
        if (mesh.subMeshCount > materials.Length)
        {
            return;
        }

        // Append combined submesh
        mesh.subMeshCount++;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
    }
}
