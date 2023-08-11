using System.Collections;
using UnityEngine;
public class IButton : AddDelegate
{
    [Header("Interactable")]

    bool active = false;

    // Collision Effects
    private void OnTriggerEnter(Collider other)
    {
        if (active)
            return;
        ButtonEffect(other);

        CallDelegate();
    }

    void ButtonEffect(Collider hit)
    {
        if (hit.name == "Bullet(Clone)")
        {
            active = true;
            StartCoroutine(ActivateButton());
        }
    }
    IEnumerator ActivateButton()
    {
        Debug.Log("Button Activated");
        float startScale = transform.localScale.z;
        float totalTime = .5f;
        for (float time = 0; time < totalTime; time += Time.deltaTime)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(startScale, startScale * .25f, time / totalTime));
            yield return new WaitForFixedUpdate();
        }
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, startScale * .25f);
    }
}