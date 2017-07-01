# pip install oandapyV20
# https://www.continuum.io/downloads Anaconda Python distribution
#https://www.oreilly.com/learning/algorithmic-trading-in-less-than-100-lines-of-python-code
import configparser
import oandapyV20 as opy

print ("oanda")

config = configparser.ConfigParser() 
config.read('oanda.cfg')

oanda = opy.API(environment='practice',access_token=config['oanda']['access_token']) 
