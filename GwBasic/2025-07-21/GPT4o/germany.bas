10 SCREEN 9  ' Set EGA mode
20 CLS
30 COLOR 0, 0  ' Set black foreground, black background
40 LINE (0, 0)-(639, 133), 0, BF  ' Draw the black part of the flag
50 COLOR 4, 0  ' Set red foreground
60 LINE (0, 134)-(639, 266), 4, BF  ' Draw the red part of the flag
70 COLOR 14, 0  ' Set yellow foreground
80 LINE (0, 267)-(639, 399), 14, BF  ' Draw the yellow part of the flag
90 PRINT "German Flag"
100 PLAY "T120 O4 L8 E G A B O5 C B A G F E D C"  ' Play a simplified version of the German anthem
110 END
