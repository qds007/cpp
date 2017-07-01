#pragma once
#include "SocketConnectorServiceT.hpp"
#include "Message.hpp"
#include "IMessageCallback.hpp"


namespace qds
{
	class SocketConnectorService : public SocketConnectorServiceT<int>, public IMessageCallback
	{
	public:
		void dispatch(const Message& msg);
		void onMessage(const Message& msg);
		bool send(const Message& msg) { return true; }
	};
}