#pragma once
//#include "DllMacros.hpp"

namespace qds
{
	class Message;
	class DLL_libMessage IMessageCallback
	{
	public:
		virtual void onMessage(const Message& msg) = 0;
	};
}
