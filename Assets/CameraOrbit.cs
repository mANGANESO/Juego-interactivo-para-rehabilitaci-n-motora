using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform follow; //punto a seguir
    public float distance;
    void Start()
    {
        
    }

    // Se ejecutan las ordenes despues que el jugador de ordenes de moverse
    void LateUpdate()
    {
        transform.position = follow.position + new Vector3(0,0,-distance);
    }
}
