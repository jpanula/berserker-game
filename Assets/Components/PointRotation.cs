using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointRotation : MonoBehaviour
{
    [SerializeField] private GameObject pointGameObject;
    void Update()
    {
        var angle = Vector3.Angle(Vector3.right,
            transform.position - pointGameObject.transform.position);
        if (transform.parent.localPosition.y < 0)
        {
            angle = 360 - angle;
        }
        Debug.Log(angle);
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
