using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
   [Header("References")]
   public PlayerController _playerController;
   public MouseLook _mouseLook;
   public CameraController _cameraController;
   private Camera _camera;

   [SerializeField] private Transform _weaponTransform;
   [SerializeField] private Transform _currentWeaponParent;

   [Header("State")] 
   public bool isFire;
   [SerializeField] private bool isReload;
   public bool availability;
   
   [Header("Animations")] 
   [SerializeField] private AnimationController _animationController;
   [SerializeField] private string Fire1;
   [SerializeField] private string Fire2;
   [SerializeField] private string Reload;
   [SerializeField] private string WeaponDown;
   [SerializeField] private string Aim;
   
   [Header("FireVariables")]
   [SerializeField] private int currentAmmo;
   [SerializeField] private int maxAmmo;
   [SerializeField] private float fireFreq;
   private float fireCounter;
   RaycastHit fireHit;
   [SerializeField] float fireRange;
   [SerializeField] private LayerMask IgnoreLayer;

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
   
   [Header("Muzzle Flash")]
   [SerializeField] private Transform weaponTip;
   [SerializeField] private GameObject muzzleFlash;
   [SerializeField] private ParticleSystem bulletShells;
   
   [Header("Bullet Holes & Particles")]
   [SerializeField] GameObject[]  bulletHoles;
   
   [Header("Indicators")]
   public TextMeshProUGUI currentAmmoText;
   public TextMeshProUGUI totalAmmoText;

   [Header("Aim")] 
   public bool aim;
   
   [SerializeField] private Vector3 originalPos;
   [SerializeField] private Vector3 aimPos;
   
   [SerializeField] private Quaternion originalRot;
   [SerializeField] private Quaternion aimRot;

   [SerializeField] private float aimSpeed;

   [SerializeField] private float originalFOV;
   [SerializeField] private float aimFOV;

   [Header("Bullet Scatter")] 
   [SerializeField] private Quaternion maxScatter;
   [SerializeField] private Quaternion minScatter;
   private Quaternion currentScatter;

   [Header("Recoil")] 
   [SerializeField] private Vector2 maxRecoil;
   [SerializeField] private Vector2 minRecoil;
   [SerializeField] Recoil _cameraRecoil;

   private void Start()
   {
      _camera = _cameraController.cameraTransform.GetComponent<Camera>();
   }

   private void Update()
   {
      Inputs();
      SetTotalAmmo();
      SetAim();
   }

   void Inputs()
   {
      _weaponTransform.localRotation = _mouseLook._cameraParent.localRotation;
      currentAmmoText.text = currentAmmo.ToString();
      totalAmmoText.text = totalAmmo.ToString();

      if (Input.GetMouseButtonDown(0) && !isReload && currentAmmo > 0 && Time.time > fireCounter && availability)
         StartFire();

      if ((Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0) && totalAmmo != 0 && currentAmmo != maxAmmo && !isFire)
         StartReload();

      if (Input.GetMouseButtonDown(1))
      {
         SetAimBool();
      }
   }

   #region Fire

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
             SetScatter() * _cameraController.cameraTransform.forward,
             out fireHit, fireRange, ~IgnoreLayer))
      {
         if (fireHit.transform.GetComponent<Rigidbody>() != null)
            fireHit.transform.GetComponent<Rigidbody>().AddForce(-fireHit.normal * 100);

         GameObject copyBulletHole = Instantiate(bulletHoles[Random.Range(0, bulletHoles.Length)], fireHit.point,
            Quaternion.LookRotation(fireHit.normal));
         
         copyBulletHole.transform.parent = fireHit.transform;

         Destroy(copyBulletHole, 15f);
      }
      
      CreateMuzzleFlash();
      SetRecoil();
      _cameraRecoil.SetTarget();
   }

   public void EndFire()
   {
      isFire = false;
      _animationController.SetBool(Fire1, isFire);
      _animationController.SetBool(Fire2, isFire);
   }

   void CreateMuzzleFlash()
   {
      GameObject muzzleFlashCopy = Instantiate(muzzleFlash, weaponTip.position, weaponTip.rotation , weaponTip);
      Destroy(muzzleFlashCopy,5);
      
      bulletShells.Play();
   }

   Quaternion SetScatter()
   {
      if (_playerController.isWalking)
      {
         currentScatter = Quaternion.Euler(Random.Range(-maxScatter.eulerAngles.x, maxScatter.eulerAngles.x),
            Random.Range(-maxScatter.eulerAngles.y, maxScatter.eulerAngles.y),
            Random.Range(-maxScatter.eulerAngles.z, maxScatter.eulerAngles.z));
      }
      else if (aim)
      {
         currentScatter = Quaternion.Euler(0, 0, 0);
      }
      else
      {
         currentScatter = Quaternion.Euler(Random.Range(-minScatter.eulerAngles.x, minScatter.eulerAngles.x),
            Random.Range(-minScatter.eulerAngles.y, minScatter.eulerAngles.y),
            Random.Range(-minScatter.eulerAngles.z, minScatter.eulerAngles.z));
      }

      return currentScatter;
   }

   void SetRecoil()
   {
      float X = Random.Range(maxRecoil.x, minRecoil.x);
      float Y = Random.Range(maxRecoil.y, minRecoil.y);

      _mouseLook.AddRecoil(X, Y);
   }
   
   #endregion

   #region Reload

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

   #endregion

   #region Aim

   void SetAimBool()
   {
      aim = !aim;
   }

   void SetAim()
   {
      if (aim)
      {
         _currentWeaponParent.localPosition =
            Vector3.Lerp(_currentWeaponParent.localPosition, aimPos, aimSpeed * Time.deltaTime);
         _currentWeaponParent.localRotation =
            Quaternion.Lerp(_currentWeaponParent.localRotation, aimRot, aimSpeed * Time.deltaTime);
         _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, aimFOV, aimSpeed * Time.deltaTime);
      }
      else
      {
         _currentWeaponParent.localPosition =
            Vector3.Lerp(_currentWeaponParent.localPosition, originalPos, aimSpeed * Time.deltaTime);
         _currentWeaponParent.localRotation =
            Quaternion.Lerp(_currentWeaponParent.localRotation, originalRot, aimSpeed * Time.deltaTime);
         _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, originalFOV, aimSpeed * Time.deltaTime);
      }
      _animationController.SetBool(Aim, aim);
   }

   #endregion

   public void CloseWeapon()
   {
      
   }

   public void AddAmmo(WeaponManager.AmmoTypes ammoType, int amount)
   {
      if (type == AmmoTypes._5_56)
      {
         _5_56 += amount;
      }
      else if (type == AmmoTypes._7_62)
      {
         _7_62 += amount;
      }
      else if (type == AmmoTypes._9mm)
      {
         _9mm += amount;
      }
      else if (type == AmmoTypes._45cal)
      {
         _45cal += amount;
      }
      else if (type == AmmoTypes._12ga)
      {
         _12ga += amount;
      }
   }
}
