#pragma once
#include <vector>

using namespace std;

namespace qds
{
	template<class T>
	class PtrVector
	{
	public:
		PtrVector(const PtrVector&) = delete;
		const vector<T*>& impl() const { return vec_; };
		vector<T*>& impl() { return vec_; }
	private:
		vector<T*> vec_
	};
}

