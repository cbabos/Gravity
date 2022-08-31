using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    #region MaterialsToUse

    public Material platformMaterial;

    #endregion

    private Vector2 _mouseInitialPosition;
    private Quaternion _lastRotation;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
        GameObject newPlatform = PlatformGenerator.GeneratePlatform(platformMaterial).GameObject();
        newPlatform.transform.SetParent(transform);
    }

    private void FixedUpdate()
    {
        HandleGesture();
    }

    private Vector2 GetMousePosition()
    {
        return _mainCamera.ScreenToViewportPoint(Input.mousePosition);
    }
    void HandleGesture()
    {
        if (Input.GetMouseButton(0))
        {
            if (_mouseInitialPosition == Vector2.zero)
            {
                _mouseInitialPosition = GetMousePosition();
                _lastRotation = transform.rotation;
            }
        }

        if (!Input.GetMouseButton(0))
        {
            _mouseInitialPosition = Vector2.zero;
        }

        if (_mouseInitialPosition != Vector2.zero)
        {
            transform.rotation = _lastRotation;
            float sensitivity = 360f;
            float difference = _mouseInitialPosition.x - GetMousePosition().x;
            transform.RotateAround(transform.position, transform.up, difference * sensitivity );
        }
    }

}
