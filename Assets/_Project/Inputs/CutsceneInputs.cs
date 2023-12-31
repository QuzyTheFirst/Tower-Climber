//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/_Project/Inputs/CutsceneInputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @CutsceneInputs: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @CutsceneInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CutsceneInputs"",
    ""maps"": [
        {
            ""name"": ""Map"",
            ""id"": ""a96d69ff-a161-4721-a4ac-d957632afec5"",
            ""actions"": [
                {
                    ""name"": ""Skip"",
                    ""type"": ""Button"",
                    ""id"": ""223742d4-2979-4519-bd7f-912eff3acee5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""aef80ae5-0123-498e-b5b3-62d4dd3be9ad"",
                    ""path"": ""<Touchscreen>/Press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mobiles"",
                    ""action"": ""Skip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mobiles"",
            ""bindingGroup"": ""Mobiles"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Map
        m_Map = asset.FindActionMap("Map", throwIfNotFound: true);
        m_Map_Skip = m_Map.FindAction("Skip", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Map
    private readonly InputActionMap m_Map;
    private List<IMapActions> m_MapActionsCallbackInterfaces = new List<IMapActions>();
    private readonly InputAction m_Map_Skip;
    public struct MapActions
    {
        private @CutsceneInputs m_Wrapper;
        public MapActions(@CutsceneInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Skip => m_Wrapper.m_Map_Skip;
        public InputActionMap Get() { return m_Wrapper.m_Map; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MapActions set) { return set.Get(); }
        public void AddCallbacks(IMapActions instance)
        {
            if (instance == null || m_Wrapper.m_MapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MapActionsCallbackInterfaces.Add(instance);
            @Skip.started += instance.OnSkip;
            @Skip.performed += instance.OnSkip;
            @Skip.canceled += instance.OnSkip;
        }

        private void UnregisterCallbacks(IMapActions instance)
        {
            @Skip.started -= instance.OnSkip;
            @Skip.performed -= instance.OnSkip;
            @Skip.canceled -= instance.OnSkip;
        }

        public void RemoveCallbacks(IMapActions instance)
        {
            if (m_Wrapper.m_MapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMapActions instance)
        {
            foreach (var item in m_Wrapper.m_MapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MapActions @Map => new MapActions(this);
    private int m_MobilesSchemeIndex = -1;
    public InputControlScheme MobilesScheme
    {
        get
        {
            if (m_MobilesSchemeIndex == -1) m_MobilesSchemeIndex = asset.FindControlSchemeIndex("Mobiles");
            return asset.controlSchemes[m_MobilesSchemeIndex];
        }
    }
    public interface IMapActions
    {
        void OnSkip(InputAction.CallbackContext context);
    }
}
