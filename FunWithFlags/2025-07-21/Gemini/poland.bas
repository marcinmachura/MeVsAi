10 SCREEN 9 ' 640x350 EGA
20 CLS
30 ' Draw Polish Flag
40 ' White part
50 LINE (0, 0)-(639, 174), 15, BF
60 ' Red part
70 LINE (0, 175)-(639, 349), 4, BF
80 ' Play Polish Anthem "Mazurek Dąbrowskiego"
90 PLAY "T180 L4 O4 G. A8 G G G. A8 G G F. G8 A A A. B-8 A G"
100 PLAY "T180 L4 O4 G. A8 G G G. A8 G G F. G8 A A A. B-8 A G"
110 PLAY "T180 L4 O5 C. C8 C C B-. A8 G A B- A. G8 F E"
120 PLAY "T180 L4 O4 G. A8 G G G. A8 G G F. G8 A A A. B-8 A G"
130 ' Loop until a key is pressed
140 A$ = INKEY$
150 IF A$ = "" THEN 140
160 SCREEN 0 ' Back to text mode
170 END
