using System;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Transform head;
    [SerializeField] private Transform headParent;

    [Header("Variables")] 
    [SerializeField] private float bobFreq;
    [SerializeField] private float horizontalMagnitude;
    [SerializeField] private float verticalMagnitude;
    [SerializeField] private float lerpSpeed;

    private float walkingTime;
    private Vector3 targetVector;

    private void Update()
    {
        SetHeadBob();
    }

    void SetHeadBob()
    {
        if (!_playerController.isWalking && !_playerController.isRunning)
        {
            walkingTime = 0f;
        }
        else
        {
            walkingTime += Time.deltaTime;
        }

        targetVector = headParent.position + SetOffset(walkingTime);
        head.position = Vector3.Lerp(head.position, targetVector, lerpSpeed * Time.deltaTime);
        if ((head.position - targetVector).magnitude < 0.001f)
        {
            head.position = targetVector;
        }
    }

    Vector3 SetOffset(float time)
    {
        float horizontalOffset = 0;
        float verticalOffset = 0;
        Vector3 offset = Vector3.zero;

        if (time > 0f)
        {
            horizontalOffset = Mathf.Cos(time * bobFreq * _playerController.TotalSpeed()) * horizontalMagnitude;
            verticalOffset = Mathf.Cos(time * bobFreq * 2 * _playerController.TotalSpeed()) * verticalMagnitude;

            offset = headParent.right * horizontalOffset + headParent.up * verticalOffset;
        }
        return offset;
    }
}
