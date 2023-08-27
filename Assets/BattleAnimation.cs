using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimation : MonoBehaviour
{
    public delegate void BattleEvent();
    public event BattleEvent OnDamageTaken;
    public event BattleEvent OnAttackMade;
    
    public float giggleAmount = 0.1f;
    public float stepBackAmount = 0.5f; 
    public float animationTime = 0.5f; 

    private Vector3 originalPosition;
    
    private void Start()
    {
        originalPosition = transform.position;
        OnDamageTaken += TriggerGiggle;
        OnAttackMade += TriggerStepBack;
    }

    private void OnDestroy()
    {
        OnDamageTaken -= TriggerGiggle;
        OnAttackMade -= TriggerStepBack;
    }

    public void DamageTaken()
    {
        OnDamageTaken?.Invoke();
    }

    public void MadeAttack()
    {
        OnAttackMade?.Invoke();
    }

    private void TriggerGiggle()
    {
        StartCoroutine(GiggleEffect());
    }

    private void TriggerStepBack()
    {
        StartCoroutine(StepBackEffect());
    }

    private IEnumerator GiggleEffect()
    {
        float timer = 0;
        while (timer < animationTime)
        {
            float offset = Mathf.Sin(timer * Mathf.PI * 2 / animationTime) * giggleAmount;
            transform.position = originalPosition + new Vector3(offset, 0, 0);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
    }

    private IEnumerator StepBackEffect()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = originalPosition - new Vector3(stepBackAmount, 0, 0);

        float timer = 0;
        while (timer < animationTime)
        {
            float t = timer / animationTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
    }
}
