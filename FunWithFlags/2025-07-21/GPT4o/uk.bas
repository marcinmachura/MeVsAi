10 SCREEN 9  ' Set EGA mode
20 CLS
30 COLOR 1, 0  ' Set blue foreground, black background
40 LINE (0, 0)-(639, 399), 1, BF  ' Draw the blue background of the flag
50 COLOR 15, 0  ' Set white foreground
60 LINE (270, 0)-(370, 399), 15, BF  ' Draw the vertical white cross
70 LINE (0, 170)-(639, 230), 15, BF  ' Draw the horizontal white cross
80 COLOR 4, 0  ' Set red foreground
90 LINE (290, 0)-(350, 399), 4, BF  ' Draw the vertical red cross
100 LINE (0, 190)-(639, 210), 4, BF  ' Draw the horizontal red cross
110 PRINT "UK Flag"
120 PLAY "T120 O4 L8 G G A F G E D C D E F G A G F E D C"  ' Play a simplified version of the UK anthem
130 END
