10 SCREEN 9 ' 640x350 EGA
20 CLS
30 ' Draw Canadian Flag
40 ' Left red part
50 LINE (0, 0)-(159, 349), 4, BF
60 ' Right red part
70 LINE (480, 0)-(639, 349), 4, BF
80 ' Center white part
90 LINE (160, 0)-(479, 349), 15, BF
100 ' Draw Maple Leaf using DRAW command
110 ' Set color to red for the leaf
120 COLOR 4
130 ' Move to starting position (bottom of the stem) and draw the leaf
140 DRAW "BM320,250"
150 DRAW "U30 E15 F15 R30 U20 E15 G15 U30 L20 H15 G15 U20 L30 D20 G15 H15 L30 D20 H15 E15 D30 R20 F15 E15 D20 R30 U30 F15 H15"
160 ' Fill the leaf
170 PAINT (320, 180), 4, 4
180 ' Play Canadian Anthem "O Canada"
190 PLAY "T100 L4 O4 F F F G A. A8 G F E F. G8 A B-. B-8 A G"
200 PLAY "T100 L4 O4 F A C5 C5 B- A G. F8 F"
210 ' Loop until a key is pressed
220 A$ = INKEY$
230 IF A$ = "" THEN 220
240 SCREEN 0 ' Back to text mode
250 END
