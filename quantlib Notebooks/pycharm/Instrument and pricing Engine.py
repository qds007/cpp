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

u.setValue(95)
print('{0:.4f} {1:.4f} {2:.4f} {3:.4f} {4:.4f}'.format(option.NPV(),option.delta(), option.vega(),option.gamma(),option.theta()))

import numpy
import matplotlib.pyplot as plot
from IPython.display import display


plot.interactive(True)
f, ax=plot.subplots()
xs= numpy.linspace(80.0,120.0,400)
ys=[]
for x in xs:
    u.setValue(x);
    ys.append(option.NPV())
ax.set_title("option Value")
_=ax.plot(xs,ys)

#plot.show(block=True)

Settings.instance().evaluationDate=Date(14,September,2017)

ys=[]
for x in xs:
    u.setValue(x)
    ys.append(option.NPV())
ax.plot(xs,ys)
display(f)  # f=fig object

plot.show(block=True)

# dummy starting params
v0=0.04; kappa=0.1;theta=0.01;sigma=0.05; rho=-0.75
model =HestonModel(
    HestonProcess(YieldTermStructureHandle(rfCurve), YieldTermStructureHandle(FlatForward(0,TARGET(),0.0,Actual360())),
                  QuoteHandle(u), v0,kappa,theta,sigma,rho))

engine=AnalyticHestonEngine(model)
option.setPricingEngine(engine)

print(option.NPV())

engineMC=MCEuropeanEngine(process,"PseudoRandom", timeSteps=20, requiredSamples=250000)
option.setPricingEngine(engineMC)
option.isExpired=False
Settings.instance().evaluationDate=today

print('{0:.4f} '.format(option.NPV())) # MC cannot est greek, must use Fininte Difference