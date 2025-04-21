using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/DrinkServed")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "DrinkServed", message: "Bartender served a drink", category: "Events", id: "6d86c2770d24e806ea04fc52629dc34a")]
public partial class DrinkServed : EventChannelBase
{
    public delegate void DrinkServedEventHandler();
    public event DrinkServedEventHandler Event; 

    public void SendEventMessage()
    {
        Event?.Invoke();
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        Event?.Invoke();
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        DrinkServedEventHandler del = () =>
        {
            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as DrinkServedEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as DrinkServedEventHandler;
    }
}

