module Utils.Plot where

import Data.Foldable
import qualified Data.Sequence as Seq
import qualified Graphics.Gnuplot.Advanced as GP
import qualified Graphics.Gnuplot.Frame as Frame
import qualified Graphics.Gnuplot.Plot.TwoDimensional as Plot2D
import qualified Graphics.Gnuplot.Graph.TwoDimensional as Graph2D
import qualified Graphics.Gnuplot.Frame.Option as Option
import qualified Graphics.Gnuplot.Frame.OptionSet as Opts
import qualified Graphics.Gnuplot.Frame.OptionSet.Style as OptsStyle
import qualified Graphics.Gnuplot.Frame.OptionSet.Histogram as Histogram
import qualified Graphics.Gnuplot.LineSpecification as LineSpec

histogram2d :: [(String, Seq.Seq Int)] 
            -> Frame.T (Graph2D.T Int Int)
histogram2d input =
  let
    minVal = minimum $ map (minimum . snd) input
    maxVal = maximum $ map (maximum . snd) input
  in
    Frame.cons (
      Opts.title "Stats" $
      Opts.yRange2d (minVal - 1, maxVal) $
      Histogram.clusteredGap 2 $
      OptsStyle.fillBorderLineType (-1) $
      OptsStyle.fillSolid $
      Opts.keyOutside $
      Opts.deflt) $
    foldMap (\(title, dat) ->
      fmap (Graph2D.lineSpec (LineSpec.title title LineSpec.deflt)) $
      Plot2D.list Graph2D.histograms $ toList dat) input
