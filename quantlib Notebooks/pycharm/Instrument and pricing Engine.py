from QuantLib import *

today = Date(19,August,2017)
Settings.instance().evaluationDate=today

option =EuropeanOption(PlainVanillaPayoff(Option.Call,100.0),EuropeanExercise(Date(24,September,2017)))

r= SimpleQuote(0.01)
sigma= SimpleQuote(0.2)
u=SimpleQuote(100)

rfCurve = FlatForward(0,TARGET(), QuoteHandle(r),Actual360())
vol = BlackConstantVol(0,TARGET(),QuoteHandle(sigma),Actual360())

process = BlackScholesProcess(QuoteHandle(u),YieldTermStructureHandle(rfCurve),BlackVolTermStructureHandle(vol) )

engine=AnalyticEuropeanEngine(process)

option.setPricingEngine(engine)

print('{0:.4f} {1:.4f} {2:.4f} {3:.4f} {4:.4f}'.format(option.NPV(),option.delta(), option.vega(),option.gamma(),option.theta()))
