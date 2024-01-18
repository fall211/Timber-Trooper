using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player")){
            Gun gun = other.gameObject.GetComponentInChildren<Gun>();
            gun.BoostGun();
            Destroy(gameObject);
        }
    }
}
