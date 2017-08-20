#pragma once
#include "defines.hpp"
class Array;
class Matrix;

class StochasticProcess
{
public:
	class discretization {
		virtual Array drift(const StochasticProcess& s, Time t0, Time dt, Array& x0) const = 0;
		virtual Array difussion(const StochasticProcess& s, Time t0, Time dt, Array& x0) const = 0;
		virtual Array covariance(const StochasticProcess& s, Time t0, Time dt, Array& x0) const = 0;
	};

};