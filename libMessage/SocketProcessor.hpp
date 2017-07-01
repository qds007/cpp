#pragma once
#include <DllMacros.hpp>
#include <string>
#include "Message.hpp"

using namespace std;
namespace qds
{
	class DLL_libMessage SocketProcessor
	{
	public:
		virtual bool Connect(string addr);
		void sendAsync(const Message& msg);
	protected:
		void readMessage(string & buffer);

	};
}