using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPopup : MonoBehaviour
{
    private Camera _camera;

    private void Start() {
        _camera = Camera.main;
    }

    private void LateUpdate() {
        transform.LookAt(new Vector3(transform.position.x, _camera.transform.position.y, _camera.transform.position.z));
        transform.Rotate(0, 180, 0);
    }
}
