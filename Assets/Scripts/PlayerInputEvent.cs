using System;
using UnityEngine;

public class PlayerInputEvent : MonoBehaviour
{
    public static event Action PlayerInteract;
    public static void OnPlayerInteract() => PlayerInteract?.Invoke();

    public static event Action EnterDialog;
    public static void OnEnterDialog() => EnterDialog?.Invoke();

    public static event Action ExitDialog;
    public static void OnExitDialog() => ExitDialog?.Invoke();

    public static event Action CloseUI;
    public static void OnCloseUI() => CloseUI?.Invoke();
}
