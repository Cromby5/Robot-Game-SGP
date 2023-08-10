using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        Button = 0,
        PressurePlate = 1,
        Lever = 2
    }

    bool active = false;

    [SerializeField] private InteractableType interactableType;
    private void OnTriggerEnter(Collider other)
    {
        if (active)
            return;
        switch (interactableType)
        {
            case InteractableType.Button:
                ButtonEffect(other);
                break;
            case InteractableType.PressurePlate:
                PressurePlateEffect(other);
                break;
        }
    }

    void ButtonEffect(Collider hit)
    {
        if (hit.name == "Bullet(Clone)")
        {
            active = true;
            StartCoroutine(ActivateButton());
        }
    }
    void PressurePlateEffect(Collider hit)
    {
        if (hit.tag.Equals("Player"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            Debug.Log("Player On PressurePlate");
        }
    }
    IEnumerator ActivateButton()
    {
        Debug.Log("Button Activated");
        float startScale = transform.localScale.z;
        float totalTime = .5f;
        for (float time = 0; time < totalTime; time+= Time.deltaTime)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(startScale, startScale * .25f, time / totalTime));
            yield return new WaitForFixedUpdate();
        }
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, startScale * .25f);
    }
}
