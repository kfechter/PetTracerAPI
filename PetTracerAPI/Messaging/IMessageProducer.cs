using System;
namespace PetTracerAPI.Messaging
{
	public interface IMessageProducer
	{
        void SendMessage<T>(T message);
    }
}

