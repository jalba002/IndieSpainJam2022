using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.VFX;

public class ArcSetter : MonoBehaviour
{
    [SerializeField] private VisualEffect vfx;

    [SerializeField] private Vector3 targetPosition;

    [SerializeField] private Vector3 upward;
    private void Update()
    {
        Vector3 direction = targetPosition - vfx.transform.position;
        vfx.SetFloat("Distance", direction.magnitude);
        vfx.SetVector3("Direction", Quaternion.LookRotation(direction.normalized, upward).eulerAngles);
    }

    public void SetTarget(Vector3 t)
    {
        targetPosition = t;
    }
}
