10 SCREEN 9  ' Set EGA mode
20 CLS
30 COLOR 15, 0  ' Set white foreground, black background
40 LINE (0, 0)-(639, 199), 15, BF  ' Draw the white part of the flag
50 COLOR 4, 0  ' Set red foreground
60 LINE (0, 200)-(639, 399), 4, BF  ' Draw the red part of the flag
70 PRINT "Polish Flag"
80 PLAY "T120 O4 L8 C E G C O5 C G E C"  ' Play a simplified version of the Polish anthem
90 END
