using System;
using UnityEngine;

public class PlayerInputEvent : MonoBehaviour
{
    public static event Action PlayerInteract;
    public static void OnPlayerInteract() => PlayerInteract?.Invoke();

    public static event Action FreezePlayer;
    public static void OnFreezePlayer() => FreezePlayer?.Invoke();

    public static event Action UnFreezePlayer;
    public static void OnUnFreezePlayer() => UnFreezePlayer?.Invoke();
}
