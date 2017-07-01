#pragma once
#include <vector>
#include "Handler.hpp"
#include "IMessageCallback.hpp"
#include <DllMacros.hpp>

namespace qds
{
	template<class T>
	class  MultiPlexer : public THandler<T>
	{
		MultiPlexer() {}
		~MultiPlexer() {}
		void handle(const T& msg);
		virtual bool subscribeHandler(THandler<T>* h);
	private:
		vector<THandler<T>*> handlers_;
	};
}
