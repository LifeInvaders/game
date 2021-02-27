using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class RebindButton
{
    [SerializeField] Button rebindButton;
    [SerializeField] InputActionReference action;
    private Text buttonText;
    [SerializeField] int bindingIndex = -1;

    public void Start()
    {
        buttonText = rebindButton.gameObject.GetComponentInChildren<Text>(); 
        rebindButton.onClick.AddListener(RemapButton);
        ChangeText();
    }
    private void RemapButton()
    {
        var rebindingOperation = action.action.PerformInteractiveRebinding(bindingIndex)
            // To avoid accidental input from mouse motion
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => {ChangeText(); operation.Dispose();})
            .Start();
    }

    private void ChangeText()
    => buttonText.text = rebindButton.gameObject.name + ": " +
                         action.action.bindings[bindingIndex].ToDisplayString();
}
