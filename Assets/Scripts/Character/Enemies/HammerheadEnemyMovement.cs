using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerheadEnemyMovement : BasicEnemyMovement
{

    protected override IEnumerator ConeAttackRoutine()
    {
        isAttacking = true;

        // Show the attack indicator
        if (attackIndicator != null)
        {
            attackIndicator.SetActive(true);

            if (animator != null)
            {
                animator.Play("SharkAttackOpenMouth");
            }
        }

        // Wait for the indicator duration
        yield return new WaitForSeconds(indicatorDuration);

        // Perform the attack
        if (attackIndicator != null)
        {
            AudioManager.instance.PlaySoundSFX(AudioManager.instance.hammerheadSFX);
            attackIndicator.SetActive(false);
        }

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, coneRange, playerLayer);
        foreach (Collider2D hit in hitPlayers)
        {
            Vector2 toTarget = hit.transform.position - transform.position;
            float angle = Vector2.Angle(savedDirection, toTarget);
            if (angle <= coneAngle / 2)
            {
                hit.gameObject.GetComponentInParent<CharacterStatManager>().TakeDamage(damageAmount);
            }
        }

        yield return new WaitForSeconds(0.5f); // Short delay before starting cooldown

        // Start cooldown
        StartCoroutine(AttackCooldownRoutine());

        isAttacking = false;

        // Smoothly rotate back toward the player after attack ends
        StartCoroutine(SmoothTurnTowardsPlayer());
    }
}
