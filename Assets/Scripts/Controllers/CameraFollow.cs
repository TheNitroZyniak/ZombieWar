using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour{

    [SerializeField] Transform player;

    void Update(){
        transform.position = player.position + new Vector3(0, 24, -17);
    }
}
