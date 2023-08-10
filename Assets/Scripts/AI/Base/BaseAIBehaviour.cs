using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAIBehaviour : MonoBehaviour
{
    /* Action called on every frame
     * Returns true if Action was successful
     */
    public abstract bool Action();
}
