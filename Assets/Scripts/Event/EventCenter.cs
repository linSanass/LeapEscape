using System;
using System.Collections.Generic;

public static class EventCenter
{
    private static readonly Dictionary<string, Delegate> _events = new Dictionary<string, Delegate>();

    /// <summary>
    /// ---- 订阅（无参）----
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cb"></param>
    public static void AddListener(string name, Action cb)
    {
        if (_events.TryGetValue(name, out var d)) _events[name] = Delegate.Combine(d, cb);
        else _events[name] = cb;
    }

    /// <summary>
    /// ---- 订阅（一个参数）----
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="cb"></param>
    public static void AddListener<T>(string name, Action<T> cb)
    {
        if (_events.TryGetValue(name, out var d)) _events[name] = Delegate.Combine(d, cb);
        else _events[name] = cb;
    }

    /// <summary>
    /// 两个参数
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="name"></param>
    /// <param name="cb"></param>
    public static void AddListener<T1, T2>(string name, Action<T1, T2> cb)
    {
        if (_events.TryGetValue(name, out var d)) _events[name] = Delegate.Combine(d, cb);
        else _events[name] = cb;
    }
    /// <summary>
    /// 三个参数
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <param name="name"></param>
    /// <param name="cb"></param>
    public static void AddListener<T1, T2, T3>(string name, Action<T1, T2, T3> cb)
    {
        if (_events.TryGetValue(name, out var d)) _events[name] = Delegate.Combine(d, cb);
        else _events[name] = cb; 
    }
    /// <summary>
    /// ---- 取消订阅（无参）----
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cb"></param>
    public static void RemoveListener(string name, Action cb)
    {
        if (!_events.TryGetValue(name, out var d)) return;
        var left = Delegate.Remove(d, cb);
        if (left == null) _events.Remove(name);
        else _events[name] = left;
    }

    /// <summary>
    /// ---- 取消订阅（一个参数）----
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="cb"></param>
    public static void RemoveListener<T>(string name, Action<T> cb)
    {
        if (!_events.TryGetValue(name, out var d)) return;
        var left = Delegate.Remove(d, cb);
        if (left == null) _events.Remove(name);
        else _events[name] = left;
    }

    /// <summary>
    /// ---- 派发（无参）----
    /// </summary>
    /// <param name="name"></param>
    public static void TriggerEvent(string name)
    {
        if (_events.TryGetValue(name, out var d)) (d as Action)?.Invoke();
    }

    /// <summary>
    /// ---- 派发（一个参数）----
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="arg"></param>
    public static void TriggerEvent<T>(string name, T arg)
    {
        if (_events.TryGetValue(name, out var d)) (d as Action<T>)?.Invoke(arg);
    }

    /// <summary>
    /// ---- 清空（切场景时用）----
    /// </summary>
    public static void Clear() => _events.Clear();
}