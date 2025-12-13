using System;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCrossHair : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController _playerController;
    
    [Header("Crosshair Settings")]
    [SerializeField] private float maxSize;
    [SerializeField] private float minSize;
    [SerializeField] private float currentSize;
    [SerializeField] private float speed;
    
    [Header("UI")]
    [SerializeField] private RectTransform crossHair;

    private void Update()
    {
        Inputs();
        SetSize();
    }

    void Inputs()
    {
        if (!_playerController.isWalking && !_playerController.isRunning)
        {
            SetMin();
        }
        else if (_playerController.isWalking)
        {
            SetMax();
        }

        if (_playerController.isRunning)
        {
            SetDeActive();
        }
        else
        {
            SetActive();
        }
    }

    void SetSize()
    {
        crossHair.sizeDelta = new Vector2(currentSize, currentSize);
    }
    
    void SetMin()
    {
        currentSize = Mathf.Lerp(currentSize, minSize, speed * Time.deltaTime);
    }

    void SetMax()
    {
        currentSize = Mathf.Lerp(currentSize, maxSize, speed * Time.deltaTime); 
    }

    void SetActive()
    {
        crossHair.gameObject.SetActive(true);
    } 
    
    void SetDeActive()
    {
        crossHair.gameObject.SetActive(false);
    }
    
}
