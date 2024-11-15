using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Animator animator;
    private Vector3 direction;
    public static PlayerMovement playerMovement;
    private float lastRotationY = 0f;
    private void Awake()
    {
        if (playerMovement == null)
        {
            playerMovement = this;
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            StopMovementAndAnimation();
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontal, vertical);
        AnimateMovement(direction);

        if (Input.GetMouseButtonDown(0) && !Tool_Inventory.tool_inventory.IsMouseOverUI())
        {
            HandleToolUsage();
        }

        ResetAnimationStates();
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (!animator.GetBool("isDigging"))
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void AnimateMovement(Vector3 direction)
    {
        bool isMoving = direction.x != 0 || direction.y != 0;

        animator.SetBool("isDigging", false);
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            UpdateRotation(direction);
            AudioManager.audioManager.Footsteps(true);
        }
        else
        {
            AudioManager.audioManager.Footsteps(false);
        }
    }
    private void UpdateRotation(Vector3 direction)
    {
        if (direction.x == 0)
        {
            transform.rotation = Quaternion.Euler(0, lastRotationY, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, direction.x < 0 ? 180 : 0, 0);
        }

        lastRotationY = transform.rotation.eulerAngles.y;
    }
    private void HandleToolUsage()
    {
        if (animator.GetBool("isMoving"))
        {
            return;
        }
        Tool_Inventory.tool_inventory.DeactivateAllTools();
        switch (Tool_Inventory.tool_inventory.currentToolIndex)
        {
            case 0:
                SetAnimationState("isAttacking", true);
                break;
            case 1:
                SetAnimationState("isAxeHitting", true);
                break;
            case 2:
                SetAnimationState("isMining", true);
                break;
            default:
                break;
        }
    }

    private void ResetAnimationStates()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("sword_hit") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95)
        {
            SetAnimationState("isAttacking", false);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("axe_hit") &&
                 animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95)
        {
            SetAnimationState("isAxeHitting", false);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("pickaxe_hit") &&
                 animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95)
        {
            SetAnimationState("isMining", false);
        }
    }

    private void SetAnimationState(string boolName, bool state)
    {
        animator.SetBool(boolName, state);
    }
    private void StopMovementAndAnimation()
    {
        direction = Vector3.zero;  
        animator.SetBool("isMoving", false);  
        AudioManager.audioManager.Footsteps(false);  
    }
}
