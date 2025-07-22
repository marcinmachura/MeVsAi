10 SCREEN 9 ' 640x350 EGA
20 CLS
30 ' Draw French Flag
40 ' Blue part
50 LINE (0, 0)-(212, 349), 1, BF
60 ' White part
70 LINE (213, 0)-(425, 349), 15, BF
80 ' Red part
90 LINE (426, 0)-(639, 349), 4, BF
100 ' Play French Anthem "La Marseillaise"
110 PLAY "T120 L8 O4 G G G C L4 E G E C"
120 PLAY "T120 L8 G G G C L4 E G E C"
130 PLAY "T120 L8 E E E F G A G F E D"
140 PLAY "T120 L8 C D L4 E C"
150 PLAY "T120 L8 G G G C L4 E G E C"
160 ' Loop until a key is pressed
170 A$ = INKEY$
180 IF A$ = "" THEN 170
190 SCREEN 0 ' Back to text mode
200 END
