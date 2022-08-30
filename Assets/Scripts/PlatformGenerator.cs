using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlatformGenerator : MonoBehaviour
{
    
    [Range(1,36)] public int smoothness = 5;
    [Range(1, 360)] public int AngleToFill = 360;
    public float RadiusOfPlatform = 3;
    public Material MaterialToUse;
    private Vector3[] _points;
    private Vector2[] _uvs;
    private int[] _tris;
    private Mesh _mesh;
    private MeshRenderer _meshRenderer;

    private void CalculatePoints()
    {
        int amountOfPoints = AngleToFill / smoothness;
        _points = new Vector3[AngleToFill/smoothness + 2];
        _uvs = new Vector2[_points.Length];
        for (int i = 0; i <= amountOfPoints + 1; i++)
        {
            float angle = Mathf.Deg2Rad * (i * smoothness);
            _points[i] = new Vector3(Mathf.Cos(angle) * RadiusOfPlatform, 0, Mathf.Sin(angle) * RadiusOfPlatform);
            _uvs[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        _points[amountOfPoints + 1] = Vector3.zero;
        _uvs[amountOfPoints + 1] = new Vector2(0, 0);
    }

    private void CalculateTriangles()
    {
        int amountOfPoints = _points.Length;
        int amountOfTrianglePoints = (amountOfPoints - 1) * 3;
        _tris = new int[amountOfTrianglePoints];
        for (int i = 0; i < amountOfPoints - 1; i++)
        {
            int pointIndex = i * 3;
            _tris[pointIndex] = i;
            _tris[pointIndex + 1] = amountOfPoints - 1;
            _tris[pointIndex + 2] = i + 1;
        }
    }

    private void Awake()
    {
        _mesh = gameObject.AddComponent<MeshFilter>().mesh;
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _meshRenderer.sharedMaterial = MaterialToUse;
        
    }

    private void Update()
    {
        RenderObject();
    }

    private void RenderObject()
    {
        CalculatePoints();
        CalculateTriangles();
        _mesh.vertices = _points;
        _mesh.triangles = _tris;
        _mesh.uv = _uvs;
        _mesh.RecalculateNormals();
    }

    private void OnGUI()
    {
        foreach (Vector3 point in _points)
        {
            Debug.DrawRay(transform.position + point, transform.up, Color.green, 10f);
        }
    }
}
