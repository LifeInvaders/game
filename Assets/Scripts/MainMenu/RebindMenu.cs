using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindMenu : MonoBehaviour
{
    [SerializeField] private RebindButton[] rebindButtons;
    [SerializeField] private InputActionAsset inputAsset;

    void Awake()
    {
        LoadOverrides();
        foreach (RebindButton rB in rebindButtons)
            rB.Start();
    }
    
    public void SaveOverrides()
    {
        var overrides = new Dictionary<Guid, string>();
        foreach (var map in inputAsset.actionMaps)
        foreach (var binding in map.bindings)
        {
            if (!string.IsNullOrEmpty(binding.overridePath))
                overrides[binding.id] = binding.overridePath;
        }
        PlayerDatabase.Instance.controls = overrides;
    }

    public void LoadOverrides()
    {
        foreach (var map in inputAsset.actionMaps)
        {
            var bindings = map.bindings;
            for (var i = 0; i < bindings.Count; ++i)
            {
                if (PlayerDatabase.Instance.controls.TryGetValue(bindings[i].id, out var overridePath))
                    map.ApplyBindingOverride(i, new InputBinding { overridePath = overridePath });
            }
        }
    }
}
