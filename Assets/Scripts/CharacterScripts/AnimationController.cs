using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    [Header("References")]
    public WeaponManager _weaponManager;
    private Animator _animation;

    private void Start()
    {
        _animation = GetComponent<Animator>();
    }

    public void SetBool(string AnimationID, bool AnimationBool)
    {
        _animation.SetBool(AnimationID, AnimationBool);
    }
    
    public void SetTrigger(string AnimationID)
    {
        _animation.SetTrigger(AnimationID);
    }

    public void EndFire()
    {
        _weaponManager.EndFire();
    }

    public void EndReload()
    {
        _weaponManager.EndReload();
    }

    public void WeaponDown()
    {
        _weaponManager.CloseWeapon();
    }

    public void SetAvailability(int Index)
    {
        _weaponManager.availability = Index == 0 ? false : true;
    }
}
