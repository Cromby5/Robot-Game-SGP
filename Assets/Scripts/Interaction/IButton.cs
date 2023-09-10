using System;
using System.Collections;
using UnityEngine;
public class IButton : AddDelegate
{
    [Header("Interactable")]

    [SerializeField] bool deactivateAfterTime = false;
    [SerializeField] float deactivationTime = 7.5f;

    bool active = false;

    float startScale;
    private void Awake()
    {
        startScale = transform.localScale.z;
    }
    // Collision Effects
    private void OnTriggerEnter(Collider other)
    {
        if (active)
            return;
        ButtonEffect(other);
    }

    void ButtonEffect(Collider hit)
    {
        if (hit.name == "Bullet(Clone)" || hit.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            active = true;
            StartCoroutine(ActivateButton());
        }
    }

    IEnumerator ActivateButton()
    {
        float currentScale = transform.localScale.z;
        float finalScale = startScale * .25f;

        float totalTime = .5f;
        for (float time = 0; time < totalTime; time += Time.deltaTime)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(currentScale, finalScale, time / totalTime));
            yield return new WaitForFixedUpdate();
        }
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, finalScale);

        CallDelegate();

        if (deactivateAfterTime)
            StartCoroutine(DeActivateButton(deactivationTime));
    }

    IEnumerator DeActivateButton(float delayTime)
    {
        float currentScale = transform.localScale.z;
        float finalScale = startScale;

        float totalTime = delayTime;
        for (float time = 0; time < totalTime; time += Time.deltaTime)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(currentScale, finalScale, time / totalTime));
            yield return new WaitForFixedUpdate();
        }
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, finalScale);

        CallDelegate();
        active = false;
    }
}