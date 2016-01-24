module Main where

import Simulation.Simulation

import Control.Monad
import qualified Graphics.Gnuplot.Advanced as GP
import System.Random.MWC

main :: IO ()
main = do 
  let party1Total = 6
  let party1Level = 100
  let party2Total = 6
  let party2Level = 100
  let iters = 100000
  putStrLn "Running Random Sim"
  figure <- withSystemRandom (runSim party1Total party1Level
    party2Total party2Level iters)
  sequence_ $
    GP.plotDefault figure :
    []
