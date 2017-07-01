#pragma once
#include "DllMacros.hpp"
#include <memory>

using namespace std;

namespace qds
{
	class DLL_libService Service
	{
	public:
		bool autoInit() { return autoInit_; }
		bool autoStart() { return autoStart_; }
		virtual bool init(shared_ptr<int> config);
	private:
		bool autoInit_;
		bool autoStart_;
		friend class ServiceManager;
	};

}
