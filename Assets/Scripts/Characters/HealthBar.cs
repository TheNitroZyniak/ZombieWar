using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour{
    [SerializeField] Image _healthBarFilling;
    [SerializeField] Health _health;
    //[SerializeField] PlayerController _healthPL;

    private Camera _camera;

    private void Awake() {
        _health.HealthChanged += OnHealthChanged;
        //_camera = Camera.main;
    }

    private void OnDestroy() {
        _health.HealthChanged -= OnHealthChanged;
    }

    void OnHealthChanged(float valueAsPercantage) {
        _healthBarFilling.fillAmount = valueAsPercantage;
    }

    private void LateUpdate() {
        transform.LookAt(new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));
        transform.Rotate(0, 180, 0);
    }
}
