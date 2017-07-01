#pragma once

//#include "stdafx.h"
#include "DllMacros.hpp"
#include "MessageCallback.hpp"

void  main() {}

namespace qds
{
	template<class T>
	class  DLL_libMessage THandler : public MessageCallback
	{
	public:
		virtual void handle(const T& msg) = 0;
		virtual void onMessage(const T& msg) = 0;
	};
}