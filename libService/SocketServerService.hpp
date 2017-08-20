#pragma once
//#include "DLLMacros.hpp"
#include <memory>
#include "Service.hpp"
//#include "Runnable.hpp"
//#include "ConfigParam.hpp"
//#include "Message.hpp"

using namespace std;

class Service;
namespace qds
{
	class DLL_libService SocketServerService : public Service, public Runnable
	{
	public:
		virtual bool init(shared_ptr<ConfigParam> config);
		bool start();
		virtual void stop();
		void listen();
		void sendAsync(const int& msg);

	protected:
		void go();
	};
}

