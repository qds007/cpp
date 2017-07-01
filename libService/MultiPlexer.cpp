#include "Multiplexer.hpp"

namespace qds
{
	template<class T>
	void MultiPlexer<T>::handle(const T & msg)
	{
		for (THandler<T> it:handlers_)
		{
			it->handle(msg)
		}
	}

	template<class T>
	bool MultiPlexer<T>::subscribeHandler(THandler<T>* h)
	{
		handlers_.push_back(h);
		return true;
	}
}