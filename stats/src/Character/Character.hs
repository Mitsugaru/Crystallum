module Character.Character where

import Character.Stats

import Control.Monad
import qualified Data.Text as T
import Data.Sequence (Seq(..))
import Data.Tuple
import Statistics.Distribution
import Control.Monad.ST
import System.Random.MWC

data Character = Character {
    name :: T.Text
  , stats :: Stats
  , health :: Int
  , turnOver :: Bool
} deriving (Show)

instance Eq Character where
  (==) a b = (name a == name b) && (stats a == stats b)

instance Ord Character where
  compare a b = compare (name a) (name b) 
  

getDeftness :: Character 
            -> Int
getDeftness = deftness . stats

getVirtue :: Character
          -> Int
getVirtue = virtue . stats

getSpirit :: Character 
          -> Int
getSpirit = spirit . stats

getResolve :: Character
           -> Int
getResolve = resolve . stats

dummyChar = Character {
    name = T.empty
  , stats = Stats {
        virtue = 0
      , virtueRange = (0, 0)
      , resolve = 0
      , resolveRange = (0, 0)
      , spirit = 0
      , spiritRange = (0, 0)
      , deftness = 0
      , deftnessRange = (0, 0)
      , vitality = 0
      , vitalityRange = (0, 0)
      }
  , health = 0
  , turnOver = True
  }


sortTuple :: Ord a => (a, a) -> (a, a)
sortTuple (a, b) 
  | a >= b = (a, b)
  | otherwise = (b, a)


newCharacter :: T.Text
             -> Int
             -> GenIO
             -> IO Character
newCharacter charName level rand = do
  let range = ((1, 10), (1, 10)) :: ((Int, Int), (Int, Int))
  virtueRng  <- fmap sortTuple $ uniformR range rand 
  resolveRng <- fmap sortTuple $ uniformR range rand 
  spiritRng  <- fmap sortTuple $ uniformR range rand 
  deftRng    <- fmap sortTuple $ uniformR range rand
  vitRng     <- fmap sortTuple $ uniformR range rand

  virt <- fmap sum $ replicateM level $ uniformR virtueRng rand 
  res  <- fmap sum $ replicateM level $ uniformR resolveRng rand
  spir <- fmap sum $ replicateM level $ uniformR spiritRng rand
  deft <- fmap sum $ replicateM level $ uniformR deftRng rand
  vit  <- fmap sum $ replicateM level $ uniformR vitRng rand

  let stat = Stats {
      virtue = vit
    , virtueRange = virtueRng
    , resolve = res
    , resolveRange = resolveRng
    , spirit = spir
    , spiritRange = spiritRng
    , deftness = deft
    , deftnessRange = deftRng
    , vitality = vit
    , vitalityRange = vitRng
  }

  return Character {
      name = charName
    , stats = stat
    , health = vit
    , turnOver = False
  }

takeDamage :: Int
           -> Character
           -> Character
takeDamage dam char = char {health = (health char) - dam}


endTurn :: Character 
        -> Character
endTurn char = char {turnOver = True}


resetTurn :: Character
          -> Character
resetTurn char = char {turnOver = False}
