module Character.Party where

import Character.Character

import qualified Data.Text as T
import qualified Data.Sequence as Seq
import System.Random.MWC


-- | A party is just a sequence of characters. 
--
type Party = Seq.Seq Character

-- | Construct a random party.
--
-- This builds a party with all the same names and at all the same level. At
-- some point we'll be changing this to allow different names per party
-- member and varying levels. 
--
buildParty :: T.Text
           -> Int
           -> Int
           -> GenIO
           -> IO Party
buildParty teamName numMembers level gen = Seq.replicateM numMembers $ 
      newCharacter teamName level gen


-- | Update a character within a party.
--
-- If the character isn't in the party, just return the unchanged party.
--
updateParty :: (Character -> Character)
            -> Character
            -> Party
            -> Party
updateParty charUpdate char party =
  case index of
    Just ix -> Seq.adjust charUpdate ix party
    Nothing -> party
  where index = Seq.elemIndexL char party


-- | Get the next fastest character in the party.
--
-- This returns the next fastest character in the party that is eligible
-- to perform an attack (which means that this character has not yet
-- taken their turn). 
getNextFastest :: Party
               -> Maybe Character
getNextFastest charList 
  | null fastChars = Nothing
  | otherwise = Just $ Seq.index fastChars 0
  where 
    eligChars = Seq.filter (not . turnOver) charList
    maxSpeed = foldr (max . getDeftness) 0 eligChars
    fastChars = Seq.filter (\x -> getDeftness x == maxSpeed) charList


-- | A party has wiped when all of their health values are zero or lower.
isPartyWiped :: Party
             -> Bool
isPartyWiped = Seq.null . getAliveMembers


-- | Return a sequence of party members who are still alive. 
getAliveMembers :: Party
                -> Party
getAliveMembers = Seq.filter ((> 0) . health)
