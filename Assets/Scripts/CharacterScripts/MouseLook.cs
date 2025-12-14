using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class MouseLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _target;
    public Transform _cameraParent;
    
    [Header("Look")]
    [SerializeField] [Range(1,10)] float sensitivity;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private float x;
    private float y;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MouseController();
    }

    void MouseController()
    {
        x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime * 15;
        y += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * 15;
        
        y=Mathf.Clamp(y,minY,maxY);
        
        _cameraParent.localRotation = Quaternion.Euler(-y, 0, 0);
        _target.Rotate(Vector3.up, x);
        
    }
}
