using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expand : MonoBehaviour{
    [SerializeField] bool raise;
    [SerializeField] AudioSource source;
    Vector3 endScale;

    private void Start() {
        endScale = transform.localScale;

        if (raise) {
            StartCoroutine(Raise());
            if(UI.soundsEnabled)
                source.Play();
        }
        else
            transform.localScale = endScale;
    }

    IEnumerator Raise() {
        float a = 0;

        while (a <= 1) {
            transform.localScale = Vector3.Lerp(Vector3.zero, endScale, a);
            yield return new WaitForFixedUpdate();
            a += 0.125f;
        }

        transform.localScale = endScale;

    }
}
