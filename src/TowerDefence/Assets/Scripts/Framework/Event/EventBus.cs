using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class EventBus
{

    public static void SubscribeEvent(IEventReceiver receiver, CommonEvent e)
    {
        e.Subscribe(receiver);
    }

    public static void SendEvent(IEventSender sender, CommonEvent e)
    {
        e.CurrentSender = sender;
        foreach (IEventReceiver receiver in e.ReceiverList)
        {
            receiver.HandleEvent(e);
        }
    }
}


