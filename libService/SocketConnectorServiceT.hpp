#pragma once
#include "SocketConnectorServiceBase.hpp"
#include <memory>

using namespace std;

namespace qds
{
	template<class SocketProcessorType>
	class SocketConnectorServiceT : public SocketConnectorServiceBase
	{
	public:
		virtual bool init(shared_ptr<ConfigParam> config);
	};
	template<class SocketProcessorType>
	inline bool SocketConnectorServiceT<SocketProcessorType>::init(shared_ptr<ConfigParam> config)
	{
		return false;
	}
}
