10 SCREEN 9 ' Set EGA mode
20 CLS ' Clear the screen
30 ' Draw the flag of the USA
40 FOR I = 0 TO 12
50   LINE (0, I * 40)-(639, I * 40 + 20), 4, BF ' Red stripes
60 NEXT I
70 LINE (0, 0)-(239, 239), 1, BF ' Blue rectangle for stars
80 ' Play "The Star-Spangled Banner"
90 PLAY "T120 O4 L4 G G A G C5 B" ' First phrase
100 PLAY "L4 G G A G D5 C5" ' Second phrase
110 PLAY "L4 G G G E C B A" ' Third phrase
120 PLAY "L4 F F E C D C" ' Fourth phrase
130 END
