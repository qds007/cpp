#pragma once
#include "defines.hpp"

class AffineModel
{
public:
	virtual Real DiscountFactor(Time time) const = 0;
};
