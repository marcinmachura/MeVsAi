10 SCREEN 9  ' Set EGA mode
20 CLS
30 COLOR 2, 0  ' Set green foreground, black background
40 LINE (0, 0)-(213, 399), 2, BF  ' Draw the green part of the flag
50 COLOR 15, 0  ' Set white foreground
60 LINE (214, 0)-(426, 399), 15, BF  ' Draw the white part of the flag
70 COLOR 4, 0  ' Set red foreground
80 LINE (427, 0)-(639, 399), 4, BF  ' Draw the red part of the flag
90 PRINT "Moroccan Flag"
100 PLAY "T120 O4 L8 C E G G F E D C D E F G A G F E D C"  ' Play a simplified version of the Moroccan anthem
110 END
