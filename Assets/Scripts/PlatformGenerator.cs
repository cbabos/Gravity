using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlatformGenerator : MonoBehaviour
{
    
    [Range(1,10)] public int SmoothnessLevel = 2;
    [Range(1, 360)] public int AngleToFill = 360;
    public float RadiusOfPlatform = 3;
    public Material MaterialToUse;
    private float _thickness = .1f;
    private Vector3[] _points;
    private Vector2[] _uvs;
    private List<int> _tris;
    private Mesh _mesh;
    private MeshRenderer _meshRenderer;

    public static PlatformGenerator GeneratePlatform(Material platformMaterial, int angleToFill = 330)
    {
        GameObject go = new GameObject();
        PlatformGenerator platformGenerator = go.AddComponent<PlatformGenerator>();
        platformGenerator.MaterialToUse = platformMaterial;
        platformGenerator.AngleToFill = angleToFill;
        return platformGenerator;
    }
    
    private void CalculatePoints()
    {
        int amountOfPoints = CalculateEdgeCount() + 2;
        _points = new Vector3[amountOfPoints];
        _uvs = new Vector2[amountOfPoints];
        _tris = new List<int>();
        CalculateFloorFace(0f);
    }

    private int CalculateEdgeCount()
    {
        int smoothness = CalculateSmoothness();
        return Mathf.CeilToInt((float) AngleToFill / smoothness);
    }

    private int CalculateSmoothness()
    {
        return 6 * SmoothnessLevel;
    }

    private Vector3 CalculatePoint(float angle, float radius)
    {
        return new Vector3(Mathf.Sin(angle) * radius, 0, -Mathf.Cos(angle) * radius);
    }
    
    private Vector2 CalculatePointUV(float angle)
    {
        return new Vector2(Mathf.Sin(angle), -Mathf.Cos(angle));
    }

    private void AddPieSlice(int startIndex, int originIndex)
    {
        _tris.Add(originIndex);
        _tris.Add(startIndex + 1);
        _tris.Add(startIndex);
    }
    private void CalculateFloorFace(float y)
    {
        _tris.Clear();
        int amountOfPoints = CalculateEdgeCount();
        for (int i = 0; i <= amountOfPoints; i++)
        {
            float angle = Mathf.Deg2Rad * (i * Mathf.Ceil((float) AngleToFill / (amountOfPoints)));
            _points[i] = CalculatePoint(angle, RadiusOfPlatform);
            _uvs[i] = CalculatePointUV(angle);
            AddPieSlice(i, amountOfPoints + 1);
        }

        _points[amountOfPoints] = CalculatePoint(Mathf.Deg2Rad * AngleToFill, RadiusOfPlatform);
        _points[amountOfPoints + 1] = Vector3.zero + Vector3.up * -.5f;
        _uvs[amountOfPoints + 1] = new Vector2(0, 0);
    }
    private void Start()
    {
        
        _mesh = gameObject.AddComponent<MeshFilter>().mesh;
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _meshRenderer.sharedMaterial = MaterialToUse;
        RenderObject();
    }

    private void RenderObject()
    {
        CalculatePoints();
        _mesh.vertices = _points;
        _mesh.triangles = _tris.ToArray();
        _mesh.uv = _uvs;
        _mesh.RecalculateNormals();
    }

    private void _OnGUI()
    {
        foreach (Vector3 point in _points)
        {
            Debug.DrawRay(transform.position + point, transform.up, Color.green, 10f);
        }
    }
}
