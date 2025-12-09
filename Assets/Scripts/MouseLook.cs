using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class MouseLook : MonoBehaviour
{
    public static MouseLook Instance; //diğer scriptler üzerinden erişim (Sadece tek bir objede kullanılır)

    private void Awake()
    {
        Instance = this;
    }
    
    [Header("References")]
    [SerializeField] Transform _target;
    [SerializeField] Transform _cameraParent;
    
    [Header("Variables")]
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

    private void FixedUpdate()
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
