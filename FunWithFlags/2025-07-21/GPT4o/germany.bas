10 SCREEN 9 ' Set EGA mode
20 CLS ' Clear the screen
30 ' Draw the flag of Germany
40 LINE (0, 0)-(639, 159), 0, BF ' Black stripe
50 LINE (0, 160)-(639, 319), 14, BF ' Red stripe
60 LINE (0, 320)-(639, 479), 6, BF ' Yellow stripe
70 ' Play the German anthem
80 PLAY "T120 O4 L4 E E F G G F E D C C D E E D D" ' First phrase
90 PLAY "L4 E E F G G F E D C C D E D C C" ' Second phrase
100 PLAY "L4 D D E C D E F E C D E F G A B" ' Third phrase
110 PLAY "L4 C5 C5 B A G F E D C C D E D C C" ' Fourth phrase
120 END
