using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAnimator : MonoBehaviour
{
    //References
    Animator animator;
    PlayerMovement playerMovement;
    SpriteRenderer spriteRender;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.moveDirection.x != 0 || playerMovement.moveDirection.y != 0)
        {
            animator.SetBool("Move", true);
            SpriteDirectionCheck();
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }

    void SpriteDirectionCheck()
    {
        if (playerMovement.lastHorizontalVector < 0)
        {
            spriteRender.flipX = true;
        }
        else
        {
            spriteRender.flipX = false;
        }
    }
}
