using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
   [Header("References")]
   public MouseLook _mouseLook;
   [SerializeField] private Transform _weaponTransform;
   public CameraController _cameraController;

   [Header("State")]
   [SerializeField] private bool isFire;
   [SerializeField] private bool isReload;
   public bool availability;
   
   [Header("Animations")] 
   [SerializeField] private AnimationController _animationController;
   [SerializeField] private string Fire1;
   [SerializeField] private string Fire2;
   [SerializeField] private string Reload;
   [SerializeField] private string WeaponDown;
   
   [Header("FireVariables")]
   [SerializeField] private int currentAmmo;
   [SerializeField] private int maxAmmo;

   [SerializeField] private float fireFreq;
   private float fireCounter;
   
   RaycastHit fireHit;
   [SerializeField] float fireRange;

   private void Update()
   {
      Inputs();
   }

   void Inputs()
   {
      _weaponTransform.localRotation = _mouseLook._cameraParent.localRotation;

      if (Input.GetMouseButtonDown(0) && !isReload && currentAmmo > 0 && Time.time > fireCounter && availability)
         StartFire();

      if (Input.GetKeyDown(KeyCode.R))
         StartReload();
   }

   void StartFire()
   {
      isFire = true;
      _animationController.SetBool(Fire1, isFire);
      currentAmmo--;
      fireCounter = Time.time + fireFreq;

      if (Physics.Raycast(_cameraController.cameraTransform.position, 
             _cameraController.cameraTransform.forward, 
             out fireHit, fireRange))
      {
         if (fireHit.transform.GetComponent<Rigidbody>() != null)
            fireHit.transform.GetComponent<Rigidbody>().AddForce(-fireHit.normal * 100);
      }
   }

   public void EndFire()
   {
      isFire = false;
      _animationController.SetBool(Fire1, isFire);
   }

   void StartReload()
   {
      isReload = true;
      _animationController.SetBool(Reload, isReload);
   }

   public void EndReload()
   {
      isReload = false;
      _animationController.SetBool(Reload, isReload);
   }

   public void CloseWeapon()
   {
      
   }
   
}
