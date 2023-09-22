using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour{

    [SerializeField] int position;
    [SerializeField] GameObject tutorialObject;
    
    private void Start() {
        if (GameManager.tutorial == position) {
            tutorialObject.SetActive(true);
        } 
        else
            tutorialObject.SetActive(false);
    }

    void Update(){
        if (GameManager.tutorial == position) {
            tutorialObject.SetActive(true);
        } 
        else
            tutorialObject.SetActive(false);

    }
}
