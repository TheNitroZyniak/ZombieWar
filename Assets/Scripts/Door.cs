using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour{
    public List<DoorPart> parts;
    [SerializeField] Health _health;

    public int doorCounter;

    private void Update() {

        if (GameManager.currentLevel < 10) {
            if (_health._currentHealth < 70 && doorCounter == 0) {
                doorCounter++;
                for (int i = 0; i < 8; i++) {
                    parts[i].ThrowParts();
                }

            }
            if (_health._currentHealth < 30 && doorCounter == 1) {
                doorCounter++;
                for (int i = 8; i < 16; i++) {
                    parts[i].ThrowParts();
                }
            }
            if (_health._currentHealth <= 0 && doorCounter == 2) {
                for (int i = 16; i < 24; i++) {
                    parts[i].ThrowParts();
                }
                doorCounter++;
                gameObject.SetActive(false);
            }
        } 
        else {
            if (_health._currentHealth < 70 && doorCounter == 0) {
                doorCounter++;
                for (int i = 0; i < 4; i++) {
                    parts[i].ThrowParts();
                }

            }
            if (_health._currentHealth < 30 && doorCounter == 1) {
                doorCounter++;
                for (int i = 4; i < 8; i++) {
                    parts[i].ThrowParts();
                }
            }
            if (_health._currentHealth <= 0 && doorCounter == 2) {
                for (int i = 8; i < 12; i++) {
                    parts[i].ThrowParts();
                }
                doorCounter++;
                gameObject.SetActive(false);
            }
        }


    }

}
