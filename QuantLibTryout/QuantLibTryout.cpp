#include "stdafx.h"
#include <ql/stochasticprocess.hpp>
#include <ql/processes/blackscholesprocess.hpp>
#include <ql/quotes/simplequote.hpp>

using namespace QuantLib;
using namespace boost;

int main()
{
	boost::shared_ptr<SimpleQuote> spot_ptr(new SimpleQuote(100));
	Handle<Quote> spot;
	Handle<YieldTermStructure> div, rf;
	
	Handle<BlackVolTermStructure> vol;
	boost::shared_ptr<BlackScholesMertonProcess> bsmProcess(
		new BlackScholesMertonProcess(spot, div, rf, vol));

    return 0;
}

