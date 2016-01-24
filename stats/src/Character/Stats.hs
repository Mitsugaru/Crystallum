module Character.Stats where


type StatRange = (Int, Int)

data Stats = Stats {
    virtue :: Int
  , virtueRange :: StatRange
  , resolve :: Int 
  , resolveRange :: StatRange
  , spirit :: Int
  , spiritRange :: StatRange
  , deftness :: Int
  , deftnessRange :: StatRange
  , vitality :: Int
  , vitalityRange :: StatRange
} deriving (Eq, Ord, Show)

