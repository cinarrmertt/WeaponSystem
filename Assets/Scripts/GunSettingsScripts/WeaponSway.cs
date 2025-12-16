using System;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public WeaponManager _weaponManager;
    
    [SerializeField] private Transform weapon;
    [SerializeField] private float slerpSpeed;
    [SerializeField] private float intensity;
    [SerializeField] private float aimIntensity;
    private void Update()
    {
        Sway();
    }

    void Sway()
    {
        float x = Input.GetAxis("Mouse X") * TotalIntensity();
        float y = Input.GetAxis("Mouse Y") * TotalIntensity();
        
        Quaternion xRot = Quaternion.AngleAxis(-y, Vector3.right);
        Quaternion yRot = Quaternion.AngleAxis(x, Vector3.up);

        Quaternion rotation = xRot * yRot;
        
        weapon.localRotation = Quaternion.Slerp(weapon.localRotation, rotation, slerpSpeed * Time.deltaTime);
    }

    float TotalIntensity()
    {
        if (_weaponManager.aim)
        {
            return aimIntensity;
        }
        else
        {
            return intensity;
        }
    }
}
