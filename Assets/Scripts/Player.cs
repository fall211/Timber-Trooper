using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public int health = 100;
    public bool isAlive = true;
    [SerializeField] private TextMeshProUGUI healthText;



    private Animator animator;
    private PlayerController playerController;
    // Start is called before the first frame update

    private void LateUpdate(){
        health = Mathf.Clamp(health, 0, 100);
        healthText.text = $"Health: {health}";
    }
    private void Start(){
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    public void ResetPlayer(){
        animator.SetTrigger("Reset");
        isAlive = true;
        health = 100;
        animator.ResetTrigger("Die");
        playerController.MoveToStart();
        GameManager.Instance.HideResetButton();
    }

    public void KillPlayer(){
        animator.SetTrigger("Die");
        isAlive = false;
    }
    public void TakeDamage(int damage){
        health -= damage;
        if (health <= 0){
            KillPlayer();
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("KillZone")){
            KillPlayer();
        }
    }
    public void OnDieAnimationComplete(){
        GameManager.Instance.ShowResetButton();
    }
}
