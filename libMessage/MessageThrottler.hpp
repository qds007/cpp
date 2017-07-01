#pragma once
#include "IMessageCallback.hpp"
#include <time.h>
#include "PtrVector.hpp"

namespace qds
{
	class MessageThrottlerBase : public IMessageCallback
	{

	};

	template<class TMessage, class TDispatchCallback>
	class MessageThrottlerT :public MessageThrottlerBase
	{
	public:
		typedef TDispatchCallback DispatchCallback;
		virtual void onMessage(const Message& msg);
		void callback(DispatchCallback cb) { dispatchCallback_ = cb; }
		void dispatch();
	private:
		DispatchCallback dispatchCallback_;


		template<class TMessage, class TDispatchCallback>
		inline void MessageThrottlerT<TMessage, TDispatchCallback>::onMessage(const Message & msg)
		{
		}

		template<class TMessage, class TDispatchCallback>
		inline void MessageThrottlerT<TMessage, TDispatchCallback>::dispatch()
		{
		}
	};

	class MessageThrottler : public MessageThrottlerT<Message, void(PtrVector<Message*>)>
	{

	};
}
