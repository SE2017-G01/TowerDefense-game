using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CommonEvent :Singleton<CommonEvent>
{
    private readonly List<IEventReceiver> _mReceiverList = new List<IEventReceiver>();

    public IEventSender CurrentSender { get; set; }
    public List<IEventReceiver> ReceiverList
    {
        get { return _mReceiverList; }
    }

    public void Subscribe(IEventReceiver receiver)
    {
        if(!ReceiverList.Contains(receiver))
            ReceiverList.Add(receiver);
    }
}
