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

    private float X;
    private float Y;

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
        X += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime * 15;
        Y += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * 15;

        Y = Mathf.Clamp(Y, minY, maxY);
        
        _cameraParent.localRotation = Quaternion.Euler(-Y, 0, 0);
        _target.localRotation = Quaternion.Euler(0, X, 0);
    }

    public void AddRecoil(float x , float y)
    {
        X += x;
        Y += y;
    }
}
