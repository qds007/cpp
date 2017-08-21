from QuantLib import *

today=Date_todaysDate()
option=BarrierOption(Barrier.UpIn,120,0.02,
                     PlainVanillaPayoff(Option.Call,100),
                     EuropeanExercise(today+90))

u=SimpleQuote(100)
r=SimpleQuote(0.01)
sigma=SimpleQuote(0.20)

rfCurve=FlatForward(0,TARGET(),QuoteHandle(r),Actual360())
vol=BlackConstantVol(0, TARGET(),QuoteHandle(sigma),Actual360())

process=BlackScholesProcess(QuoteHandle(u),YieldTermStructureHandle(rfCurve),BlackVolTermStructureHandle(vol))
engine=AnalyticBarrierEngine(process)
option.setPricingEngine(engine)
Settings.instance().evaluationDate=today

h=0.01; u0=u.value()

p0=option.NPV()
u.setValue(u0+h)
p_plus =option.NPV()

u.setValue(u0-h)
p_minus=option.NPV()

delta =(p_plus-p_minus)/(2*h)
gamma =(p_plus-2*p0+p_minus)/(h*h)

h1=0.0001

sigma0=sigma.value();p0=option.NPV()
sigma.setValue(sigma0+h1);p_plus=option.NPV()
vega=(p_plus-p0)/h1
sigma.setValue(sigma0)

r0=r.value();p0=option.NPV()
r.setValue(r0+h1);p_plus=option.NPV()
rho=(p_plus-p0)/h1
r.setValue(r0)

p0=option.NPV()
Settings.instance().evaluationDate=today+10; p_plus=option.NPV();
print(p0,p_plus)
h2=10/365
theta=(p_plus-p0)/h2
Settings.instance().evaluationDate=today
print("{0:f} {1:f} {2:f} {3:f} {4:f}".format(delta,gamma,vega,rho,theta))

