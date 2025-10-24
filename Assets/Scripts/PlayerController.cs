using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;

    public bool isMoving;
    private Vector2 input;

    private void Update(){
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //remove diagonal movement
            if (input.x != 0 ) input.y = 0;
            if (input != Vector2.zero){
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos)){
                    StartCoroutine(Move(targetPos));
                } 
            }
        }
    }

    IEnumerator Move(Vector3 targetPos){

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
    }

    //Handles collisions 
    //(Player doesn't move forward when it's colliding with a solid object)
    private bool IsWalkable(Vector3 targetPos){
        if ((Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer)) != null){
            return false;
        }
        return true;
    }
}
