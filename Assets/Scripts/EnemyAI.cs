using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;
    private Rigidbody rb;
    private Animator animator;
    private AudioSource audioSource;
    private float speed = 2.5f;
    public bool isWalking = false;
    public bool isActive = false;

    private float attackIFrames = 0.5f;
    private int damage = 1;
    private int health = 3;
    private bool emissive = false;
    private float emissiveTimer = 0f;

    private void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        RandomizeStats();
    }

    // Update is called once per frame
    private void Update(){
        if (emissive){
            emissiveTimer -= Time.deltaTime;
            if (emissiveTimer <= 0){
                emissive = false;
                var material = GetComponentInChildren<SkinnedMeshRenderer>().material;
                material.DisableKeyword("_EMISSION");
            }
        }
        if (Vector3.Distance(transform.position, player.transform.position) > 25f){
            KillEnemy(false);
        }

    }
    private void FixedUpdate()
    {
        if (attackIFrames > 0) attackIFrames -= Time.fixedDeltaTime;
        if (!isWalking){
            return;
        }

        var direction = player.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        rb.MovePosition(transform.position + speed * Time.fixedDeltaTime * direction);

        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void RandomizeStats(){
        var size = Random.Range(0.8f, 1.2f);
        transform.localScale = Vector3.one * size;
        damage = Mathf.RoundToInt(4 * size);

    }

    public void StartWalking(){
        isWalking = true;
        isActive = true;
    }

    public void KillEnemy(bool givePoints = true){
        if (!isActive) return;

        isWalking = false;
            audioSource.Play();
        animator.SetTrigger("Die");
        if (givePoints) GameManager.Instance.AddScore(1);
        GameManager.Instance.enemies.Remove(gameObject);
        Destroy(gameObject, 6f);
    }

    public void TakeDamage(int dmg){
        if (isActive) EnableEmission();
        health -= dmg;
        if (health <= 0){
            KillEnemy();
            isActive = false;
        }
    }

    private void EnableEmission(){
        if (emissive) return;
        emissive = true;
        emissiveTimer = 0.125f;
        var material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        material.EnableKeyword("_EMISSION");
    }

    private void OnTriggerStay(Collider other){
        if (!isActive) return;
        if (other.CompareTag("Player")){
            if (attackIFrames <= 0) {
                player.GetComponent<Player>().TakeDamage(damage);
                attackIFrames = 0.5f;
            }
        }
    }

}
