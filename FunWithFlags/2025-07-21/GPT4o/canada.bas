10 SCREEN 9  ' Set EGA mode
20 CLS
30 COLOR 15, 0  ' Set white foreground, black background
40 LINE (0, 0)-(213, 399), 15, BF  ' Draw the white center of the flag
50 COLOR 4, 0  ' Set red foreground
60 LINE (0, 0)-(213, 399), 4, BF  ' Draw the red vertical bars
70 LINE (426, 0)-(639, 399), 4, BF
80 PRINT "Canadian Flag"
90 PLAY "T120 O4 L8 C E G G F E D C D E F G A G F E D C"  ' Play a simplified version of the Canadian anthem
100 END
