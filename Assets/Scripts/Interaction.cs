using System;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
   [Header("References")]
   public DynamicCrossHair _dynamicCrossHair;
   
   [SerializeField] private Transform _camera;
   [SerializeField] private float interactionDistance;
   
   RaycastHit interactionHit;

   [Header("ItemName")] 
   [SerializeField] private GameObject itemNameObject;
   [SerializeField] TextMeshProUGUI itemNameText;

   private void Update()
   {
      if (Physics.Raycast(_camera.position, _camera.forward, out interactionHit, interactionDistance))
      {
         if (interactionHit.transform.GetComponent<IInteractable>() != null)
         {
            _dynamicCrossHair.available = false;
            itemNameObject.SetActive(true);
            itemNameText.text = interactionHit.transform.name;
            
            if (Input.GetKeyDown(KeyCode.E))
            {
               interactionHit.transform.GetComponent<IInteractable>().Interact();
            }
         }
         else
         {
            _dynamicCrossHair.available = true;
            itemNameObject.SetActive(false);
         }
      }
      else
      {
         _dynamicCrossHair.available = true;
         itemNameObject.SetActive(false);
      }
   }
}
