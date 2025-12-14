using UnityEngine;

public class AmmoBox : MonoBehaviour, IInteractable
{
    [Header("References")]
    public WeaponManager _weaponManager;
    public WeaponManager.AmmoTypes ammoType;
    
    [Header("Variables")]
    public string itemName;
    public int amount;

    public string Name
    {
        get => itemName; 
        set => itemName = value;
    }
    
    public void Interact()
    {
        _weaponManager.AddAmmo(ammoType,amount);
        Destroy(gameObject);
    }
}
