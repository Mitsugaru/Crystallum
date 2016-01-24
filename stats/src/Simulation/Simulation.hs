module Simulation.Simulation where

import Character.Character
import Character.Party
import Character.Stats
import Utils.Plot

import Control.Monad
import Control.Monad.Loops
import Control.Monad.ST
import Data.Foldable
import qualified Data.Map as Map
import Data.Maybe
import qualified Data.Sequence as Seq
import qualified Data.Text as T
import Graphics.Gnuplot.Advanced
import System.Random.MWC

import qualified Graphics.Gnuplot.Frame as Frame
import qualified Graphics.Gnuplot.Plot.TwoDimensional as Plot2D
import qualified Graphics.Gnuplot.Graph.TwoDimensional as Graph2D

data Results = Results {
    startingChars1 :: Party
  , startingChars2 :: Party
  , finalChars1 :: Party
  , finalChars2 :: Party
  , minDamage :: Map.Map Character Int
  , maxDamage :: Map.Map Character Int
  , totalTurns :: Int
  , turnsPerChar :: Map.Map Character Int
  , combatRounds :: [CombatRound]
} deriving (Show)

data CombatRound = CombatRound { 
    attacker :: Character
  , defender :: Character
  , damageDealt :: Int
} deriving (Show)

initResults :: Party
            -> Party
            -> Results
initResults party1 party2 = Results {
        startingChars1 = party1
      , startingChars2 = party2
      , finalChars1 = party1
      , finalChars2 = party2
      , minDamage = Map.empty
      , maxDamage = Map.empty
      , totalTurns = 0
      , turnsPerChar = Map.empty
      , combatRounds = []
    }

runSim :: Int -- ^ Number of party members in group 1.
       -> Int -- ^ Level of all team members in party 1.
       -> Int -- ^ Number of party members in group 2.
       -> Int -- ^ Level of all party members in group 2.
       -> Int -- ^ Number of combat scenarios to run.
       -> GenIO -- ^ Random number generator.
       -> IO (Frame.T (Graph2D.T Int Int)) -- ^ Figure displaying the results.
runSim party1Cnt party1Lvl party2Cnt party2Lvl iters gen = do
    -- Build two parties randomly.
    party1 <- buildParty (T.pack "Team Badass") party1Cnt party1Lvl gen
    party2 <- buildParty (T.pack "Team Meanbutt") party2Cnt party2Lvl gen

    let startingResults = initResults party1 party2

    -- Run a combat scenario. Combat ends when one party has been killed.
    finalResults <- iterateUntilM (\x -> (isPartyWiped . finalChars1) x ||
      (isPartyWiped . finalChars2) x) (combat gen) startingResults 
    
    -- Here is where we will be doing data analysis on the results we
    -- got from the combat simulation. For now though, we'll just be 
    -- displaying the stats. 
    let virtueVals = fmap (virtue . stats) party1
    let resolveVals = fmap (resolve . stats) party1
    let spiritVals = fmap (spirit . stats) party1
    let deftnessVals = fmap (deftness . stats) party1
    let vitalityVals = fmap (vitality . stats) party1


    return $ histogram2d $ [ ("Virtue", virtueVals) 
      , ("Resolve", resolveVals) 
      , ("Spirit", spiritVals) 
      , ("Deftness", deftnessVals) 
      , ("Vitality", vitalityVals) ]


-- | Simulate a combat scenario.
--
-- This runs through a single step of combat given two parties in the 
-- Results data structure. Combat executes as follows:
--
-- * Each party checks for the fastest player that hasn't yet taken their
--   turn. 
-- * The fastest players are compared. Whichever has the current fastest 
--   player attacks a person on the opposing team randomly.
--
combat :: GenIO
       -> Results
       -> IO Results
combat gen results = do
  -- Get the current state of the party.
  let p1 = finalChars1 results
  let p2 = finalChars2 results
  
  let (party1, party2) = if (all turnOver p1 && all turnOver p2) 
      then 
        (fmap resetTurn p1, fmap resetTurn p2)
      else 
        (p1, p2)

  -- Grab the two fastest eligible characters from each party and execute
  -- a combat round with the fastest of the two.
  let next1 = fromMaybe dummyChar $ getNextFastest party1
  let next2 = fromMaybe dummyChar $ getNextFastest party2
  comRound <- 
    if (getDeftness next1 >= getDeftness next2)
      then doCombatRound next1 party2 gen
      else doCombatRound next2 party1 gen

  -- Update the parties based on the result of the combat round. One 
  -- party just had a random member take damage and the other party just had
  -- a member end their turn.
  let defendChar = defender comRound
  let attackChar = attacker comRound
  let (party1', party2') = if (next1 == attackChar) 
      then 
        (updateParty endTurn attackChar party1,
          updateParty (takeDamage (damageDealt comRound)) defendChar party2)
      else
        (updateParty (takeDamage (damageDealt comRound)) defendChar party1,
          updateParty endTurn attackChar party2)
   
  -- Log the results of this combat round.
  return $ results { 
      combatRounds = combatRounds results ++ [comRound]
    , totalTurns = totalTurns results + 1
    , turnsPerChar = Map.adjust (+ 1) (attacker comRound) (turnsPerChar results)
    , finalChars1 = party1'
    , finalChars2 = party2'
   }

   
-- | Runs through a single combat round.
--
doCombatRound :: Character 
              -> Party 
              -> GenIO
              -> IO CombatRound
doCombatRound att party gen = do
  -- Get all of the alive party members on the defending team and select
  -- one at random.
  let eligDefenders = getAliveMembers party
  index <- uniformR (0, length eligDefenders - 1) gen

  -- Damage is calculated randomly, with a multiplier and an offset applied.
  -- Generate these here so the attack function doesn't need the random 
  -- number generator.
  multiplier <- uniformR (1, 5) gen
  additive <- uniformR (1, 10) gen

  -- Execute the attack.
  return $ attack att (Seq.index eligDefenders index) multiplier additive
  
  
-- | Attack a single character.
--
-- Note that defense is not yet calculated into the damage. There are 
-- only the multipliers and offsets for offense.
--
attack :: Character 
       -> Character
       -> Int
       -> Int
       -> CombatRound
attack att def multiplier additive = 
  let
    virtueAtt = getVirtue att 
    spiritAtt = getSpirit att
    resolveDef = getResolve def
    spiritDef = getSpirit def
    damage = if virtueAtt > spiritAtt
               then virtueAtt * multiplier + additive
               else spiritAtt * multiplier + additive
  in
    CombatRound { 
        attacker = att
      , defender = def
      , damageDealt = damage
    }


