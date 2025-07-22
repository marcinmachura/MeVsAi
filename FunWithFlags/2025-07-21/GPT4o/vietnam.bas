10 SCREEN 9  ' Set EGA mode
20 CLS
30 COLOR 4, 0  ' Set red foreground, black background
40 LINE (0, 0)-(639, 399), 4, BF  ' Draw the red background of the flag
50 COLOR 15, 0  ' Set white foreground
60 LINE (270, 0)-(370, 399), 15, BF  ' Draw the vertical white stripe
70 LINE (0, 170)-(639, 230), 15, BF  ' Draw the horizontal white stripe
80 PRINT "Vietnam Flag"
90 PLAY "T120 O4 L8 C E G G F E D C D E F G A G F E D C"  ' Play a simplified version of the Vietnam anthem
100 END
