using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class UIScreenRepository : MonoBehaviour
{
    private Dictionary<Type, UIScreen> _screens;

    private static UIScreenRepository _instance;

    private void Awake()
    {
        if (_instance != null )
        {
            Destroy(this);
            return;
        }

        _instance = this;
        _screens = new Dictionary<Type, UIScreen>();

        foreach(UIScreen screen in FindObjectsOfType<UIScreen>(true))
            _screens.Add(screen.GetType(), screen);
    }

    public static TScreen GetScreen<TScreen>() where TScreen : UIScreen
    {
        if (_instance is null)
            throw new NullReferenceException($"Instance is null");

        if (_instance._screens.TryGetValue(typeof(TScreen), out UIScreen screen))
            return (TScreen)screen;

        return default;
    }
}
