using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<Player>().TakeDamage(-10);
            Destroy(gameObject);
        }
    }
}
