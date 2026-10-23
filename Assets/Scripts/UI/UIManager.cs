using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI管理器
/// </summary>
public class UIManager : SingLetonAutoMono<UIManager>
{
    private readonly Dictionary<Type, UIPanel> _panels = new Dictionary<Type, UIPanel>();

    protected override void Awake()
    {
        base.Awake();
        // 收集所有面板（包括隐藏的），登记到字典
        foreach (var panel in GetComponentsInChildren<UIPanel>(true))
        {
            var type = panel.GetType();
            _panels[type] = panel;
        }
    }

    public T Open<T>() where T : UIPanel
    {
        if (_panels.TryGetValue(typeof(T), out var p)) { p.Open(); return (T)p; }
        return null;
    }

    public void Close<T>() where T : UIPanel
    {
        if (_panels.TryGetValue(typeof(T), out var p)) p.Close();
    }
}