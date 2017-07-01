#pragma once
#include <memory>

using namespace std;

namespace qds
{

#define DECL_CLASS(QdsClass) \
public: \
	typedef shared_ptr<QdsClass> ShPtr;\
	typedef shared_ptr<const QdsClass> CShPtr;\
	typedef auto_ptr<QdsClass> Pa;\


}
