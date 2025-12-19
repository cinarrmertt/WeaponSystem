using UnityEngine;

public class Weapons : MonoBehaviour,IInteractable
{
    public string weaponName;
    
    public string Name
    {
        get => weaponName;
        set => weaponName = value;
    }
    public void Interact()
    {
        WeaponManager.instance.PickWeapon(weaponName);
        Destroy(gameObject);
    }
}
