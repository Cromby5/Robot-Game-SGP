using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AddDelegate : MonoBehaviour, ISerializationCallbackReceiver
{
    [Header("Delegate")]
    #region Popups
    // Button
    [SerializeField]
    private GameObject delegateGameObject;

    [SerializeField]
    [HideInInspector]
    private Component currentComponent;

    public static List<string> tempList;
    [HideInInspector]
    private List<string> popupList;

    public static List<string> tempList2;
    [HideInInspector]
    private List<string> popupList2;

    // Popup List 1
    [ListToPopup(typeof(AddDelegate), "tempList")]
    public string delegateComponent;

    // Popup List 2
    [ListToPopup(typeof(AddDelegate), "tempList2")]
    public string delegateMethod;
    private List<string> GetAllComponents()
    {
        if (delegateGameObject == null)
            return null;
        Component[] listOfComponents = delegateGameObject.GetComponents(typeof(Component));
        List<string> listOfComponentsString = new List<string>();

        for (int i = 0; i < listOfComponents.Length; i++)
        {
            listOfComponentsString.Add(listOfComponents[i].ToString());
        }
        return listOfComponentsString;
    }
    private List<string> GetAllMethods()
    {
        if (delegateGameObject == null)
            return null;
        // List of Methods
        var methods = new List<MethodInfo>();
        Component componentNeeded = GetCurrentComponent();
        if (componentNeeded == null)
            return null;
        // Add methods from component
        methods.AddRange(componentNeeded.GetType().GetTypeInfo().DeclaredMethods);
        List<string> listOfMethodsString = new List<string>();

        var methodsArray = methods.ToArray();
        for (int i = 0; i < methods.Count; i++)
        {
            listOfMethodsString.Add(methodsArray[i].ToString());
        }
        return listOfMethodsString;
    }

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize()
    {
        popupList = GetAllComponents();
        tempList = popupList;
        popupList2 = GetAllMethods();
        tempList2 = popupList2;
    }
    private Component GetCurrentComponent()
    {
        if (delegateGameObject == null)
            return null;
        // List of Methods
        var methods = new List<MethodInfo>();
        Component componentNeeded = null;
        // Temp Component
        var tempComponents = delegateGameObject.GetComponents<Component>();
        foreach (var component in tempComponents)
        {
            if (component.ToString() == delegateComponent)
            {
                currentComponent = component;
                componentNeeded = currentComponent;
                return componentNeeded;
            }
        }
        return null;
    }
    private MethodInfo GetCurrentMethod()
    {
        // List of Methods
        var methods = new List<MethodInfo>();

        methods.AddRange(currentComponent.GetType().GetTypeInfo().DeclaredMethods);
        List<string> listOfMethodsString = new List<string>();

        var methodsArray = methods.ToArray();
        for (int i = 0; i < methods.Count; i++)
        {
            if (delegateMethod == methodsArray[i].ToString())
            {
                return methods[i];
            }
        }
        return null;
    }

    public void CallDelegate()
    {
        MethodInfo methodToCall = GetCurrentMethod();
        if (methodToCall != null)
        {
            Object[] parameters = null;
            methodToCall.Invoke(currentComponent, parameters);
        }
    }
    #endregion
}
