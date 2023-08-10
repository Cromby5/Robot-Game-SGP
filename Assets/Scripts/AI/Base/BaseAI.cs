using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAI : MonoBehaviour
{
//    public List <BaseAIBehaviour> aiBehaviours;
    public abstract void CooperativeArbitration();

    private void Update()
    {
        CooperativeArbitration();
    }
}
