using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed= 5f ;
    [SerializeField] private LayerMask solidObjectsLayer;

    [SerializeField] private float encounterRate = 0.1f; // 10% chance per step
    private GameManager gameManager;

    public bool isMoving;
    private Vector2 input;
    private Animator animator;

    private void Awake(){
        animator = GetComponent<Animator>();
        gameManager = FindFirstObjectByType<GameManager>(); 
        if (gameManager == null) Debug.LogError("GameManager not found in scene!");
    }

    private void Update(){
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //remove diagonal movement
            if (input.x != 0 ) input.y = 0;

            if (input != Vector2.zero){
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos)){
                    StartCoroutine(Move(targetPos));
                } 
            }
        }
        animator.SetBool("isMoving", isMoving);
    }

    //Coroutine for smooth, tile-by-tile movement
    IEnumerator Move(Vector3 targetPos) {

        isMoving = true;
        //if there's a difference between the current and target position
        //player will move towards target position
        //loop will only stop once they're equal
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon){
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null; 
        }
        transform.position = targetPos;
        isMoving = false;

        CheckForRandomEncounter();
    }

    //Handles collisions 
    //(Player doesn't move forward when it's colliding with a solid object)
    private bool IsWalkable(Vector3 targetPos){
        if ((Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer)) != null){
            return false;
        }
        return true;
    }
    private void CheckForRandomEncounter()
    {
        // Only check for encounter if the player is in the 'long grass' or walkable area
        // For simplicity, we check a general chance every step
        if (Random.value < encounterRate) // Random.value is between 0.0 and 1.0
        {
            Debug.Log("WILD POKEMON ENCOUNTERED!");
            // Tell the GameManager to start the battle sequence
            gameManager.StartBattleSequence(); 
        }
    }
}