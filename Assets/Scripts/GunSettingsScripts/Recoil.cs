using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Recoil : MonoBehaviour
{
    [SerializeField] Transform Object;

    [Header("")] 
    [SerializeField] private Vector3 maxTargetPos;
    [SerializeField] private Vector3 minTargetPos;

    [SerializeField] private Quaternion maxTargetRot;
    [SerializeField] private Quaternion minTargetRot;
    
    private Vector3 targetPos; 
    private Vector3 originalPos;

    private Quaternion targetRot;
    private Quaternion originalRot;
    
    [Header("")]
    private Vector3 slideVector;
    private Quaternion slideRot;

    [Header("")] 
    [SerializeField] private float slideSpeed;
    [SerializeField] private float lerpSpeed;

    [SerializeField] private bool lerp;

    private void Start()
    {
        originalPos = Object.localPosition;
        originalRot = Object.localRotation;
    }

    private void Update()
    {
        if (lerp)
        {
            slideVector = Vector3.MoveTowards(slideVector, targetPos, slideSpeed * Time.deltaTime);
            slideRot = Quaternion.RotateTowards(slideRot, targetRot, slideSpeed * Time.deltaTime * 7);
            
            if (slideVector == targetPos)
            {
                lerp = false;
            }
        }
        else
        {
            slideVector = Vector3.MoveTowards(slideVector, originalPos, slideSpeed * Time.deltaTime);
            slideRot = Quaternion.RotateTowards(slideRot, originalRot, slideSpeed * Time.deltaTime * 7);
        }

        Object.localPosition = Vector3.Lerp(Object.localPosition, slideVector, lerpSpeed * Time.deltaTime);
        Object.localRotation = Quaternion.Lerp(Object.localRotation, slideRot, lerpSpeed * Time.deltaTime * 7);
    }

    public void SetTarget()
    {
        targetPos = originalPos + new Vector3(Random.Range(minTargetPos.x, maxTargetPos.x),
            Random.Range(minTargetPos.y, maxTargetPos.y), Random.Range(minTargetPos.z, maxTargetPos.z));
        targetRot = originalRot * Quaternion.Euler(Random.Range(minTargetRot.eulerAngles.x, maxTargetRot.eulerAngles.x),
            Random.Range(minTargetRot.eulerAngles.y, maxTargetRot.eulerAngles.y),
            Random.Range(minTargetRot.eulerAngles.z, maxTargetRot.eulerAngles.z));
        lerp = true;
    }
}
