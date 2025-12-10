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

   [Header("ReloadVariables")] 
   [SerializeField] private int totalAmmo;
   
   [Header("AmmoTypes")]
   [SerializeField] private AmmoTypes type;

   public enum AmmoTypes
   {
      _5_56,
      _7_62,
      _9mm,
      _45cal,
      _12ga
   }

   [SerializeField] private int _5_56;
   [SerializeField] private int _7_62;
   [SerializeField] private int _9mm;
   [SerializeField] private int _45cal;
   [SerializeField] private int _12ga;
   
   private void Update()
   {
      Inputs();
      SetTotalAmmo();
   }

   void Inputs()
   {
      _weaponTransform.localRotation = _mouseLook._cameraParent.localRotation;

      if (Input.GetMouseButtonDown(0) && !isReload && currentAmmo > 0 && Time.time > fireCounter && availability)
         StartFire();

      if ((Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0) && totalAmmo != 0 && currentAmmo != maxAmmo && !isFire)
         StartReload();
   }

   void StartFire()
   {
      isFire = true;

      if (currentAmmo <= 1)
         _animationController.SetBool(Fire2, isFire);
      else
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
      _animationController.SetBool(Fire2, isFire);
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
      
      int amount = SetReloadAmount(totalAmmo);
      
      currentAmmo += amount;
      
      if (type == AmmoTypes._5_56)
         _5_56 -= amount;
      
      else if (type == AmmoTypes._7_62)
         _7_62 -= amount;
      
      else if (type == AmmoTypes._9mm)
         _9mm -= amount;
      
      else if (type == AmmoTypes._45cal)
         _45cal -= amount;
      
      else if (type == AmmoTypes._12ga)
         _12ga -= amount;
   }

   void SetTotalAmmo()
   {
      if (type == AmmoTypes._5_56)
         totalAmmo = _5_56;
      
      else if (type == AmmoTypes._7_62)
         totalAmmo = _7_62;
      
      else if (type == AmmoTypes._9mm)
         totalAmmo = _9mm;
      
      else if (type == AmmoTypes._45cal)
         totalAmmo = _45cal;
      
      else if (type == AmmoTypes._12ga)
         totalAmmo = _12ga;
   }

   int SetReloadAmount(int inverntoryAmount)
   {
      int amountNeeded = maxAmmo - currentAmmo;

      if (amountNeeded < inverntoryAmount)
      {
         return amountNeeded;
      }
      else
      {
         return inverntoryAmount;
      }
   }

   public void CloseWeapon()
   {
      
   }
   
}
