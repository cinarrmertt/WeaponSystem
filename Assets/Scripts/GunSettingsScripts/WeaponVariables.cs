using UnityEngine;

public class WeaponVariables : MonoBehaviour
{
    public string weaponID;
    public Transform weaponParent;
    
    [Header("Animations")] 
    public AnimationController animationController;

    [Header("FireVariables")] 
    public int bulletsAndOnce;
    public int currentAmmo;
    public float fireFreq;
    public float fireRange;
    
    [Header("ReloadVariables")] 
    public int maxAmmo;
    public WeaponManager.AmmoTypes type;
    
    [Header("Muzzle Flash")]
    public Transform weaponTip;
    public GameObject muzzleFlash;
    public ParticleSystem bulletShells;
    
    [Header("Aim")] 
    public Vector3 originalPos;
    public Vector3 aimPos;
   
    public Quaternion originalRot;
    public Quaternion aimRot;

    public float aimSpeed;

    public float originalFOV;
    public float aimFOV;
    
    [Header("Bullet Scatter")] 
    public Quaternion maxScatter;
    public Quaternion minScatter;
    public Quaternion aimScatter;
    
    [Header("Recoil")] 
    public Vector2 maxRecoil;
    public Vector2 minRecoil;
    public Recoil weaponRecoil;

    [Header("Drop Weapon")] 
    public GameObject pickableWeapon;
}
