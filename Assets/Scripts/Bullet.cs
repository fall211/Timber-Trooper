using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerController player;
    private Gun gun;
    private void Start(){
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        gun = player.gun.GetComponent<Gun>();
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Enemy")){
            other.GetComponent<EnemyAI>().TakeDamage(gun.damage);
            DestroyBullet();
        }
    }
    public void DestroyBullet(){
        Destroy(gameObject);
    }

    private void OnBecameInvisible(){
        DestroyBullet();
    }
}
