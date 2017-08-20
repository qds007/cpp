#pragma once
//#include <DllMacros.hpp>
#include <string>
#include "Message.hpp"

using namespace std;
class Message;
namespace qds
{
	class  SocketProcessor
	{
	public:
		virtual bool Connect(string addr);
		void sendAsync(const string& msg);
	protected:
		void readMessage(string & buffer);

	};
}