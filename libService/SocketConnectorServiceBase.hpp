#pragma once
#include "Service.hpp"
#include "ConfigParam.hpp"
#include "Runnable.hpp"
#include <memory>

using namespace std;
namespace  qds
{
	class SocketConnectorServiceBase : public Service, public Runnable
	{
	public:
		virtual bool init(shared_ptr<ConfigParam> config);
	};
}
