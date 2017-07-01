#pragma once
#include "DllMacros.hpp"
#include "IMessageCallback.hpp"
#include "Message.hpp"
#include <functional>
using namespace std;

namespace qds
{
	class DLL_libMessage MessageCallback: public IMessageCallback, public function<void(const Message&)>
	{

	};
}
