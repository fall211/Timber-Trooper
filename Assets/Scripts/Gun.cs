using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 10f;
    public int damage = 1;
    public float fireRate = 0.4f;
    private float gunCooldown = 0f;
    private Player player;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();   
        audioSource = GetComponent<AudioSource>();
    }

    private void SpawnBullet(){
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.transform.forward * bulletSpeed, ForceMode.Impulse);
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isAlive) return;
        if (gunCooldown > 0) gunCooldown -= Time.deltaTime;
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && gunCooldown <= 0){
            SpawnBullet();
            gunCooldown = fireRate;
        }

    }
}
