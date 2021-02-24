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
        if (bindingIndex == -1)
            rebindButton.onClick.AddListener(RemapButton);
        else rebindButton.onClick.AddListener(RemapButtonAxis);
        ChangeText();
    }

    private void RemapButton()
    {
        var rebindingOperation = action.action.PerformInteractiveRebinding()
            // To avoid accidental input from mouse motion
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => {ChangeText(); operation.Dispose();})
            .Start();
    }

    private void RemapButtonAxis()
    {
        var rebindingOperation = action.action.PerformInteractiveRebinding(bindingIndex)
            // To avoid accidental input from mouse motion
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => {ChangeText(); operation.Dispose();})
            .Start();
    }

    private void ChangeText()
    {
        string print;
        if (bindingIndex == -1)
            print = action.action.name + ": " + action.action.GetBindingDisplayString();
        else
            print = action.action.bindings[bindingIndex].name + ": " +
                    action.action.bindings[bindingIndex].ToDisplayString();
        buttonText.text = print;
    }
}
