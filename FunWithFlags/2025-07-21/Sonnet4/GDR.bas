10 SCREEN 9 ' Set EGA mode
20 CLS ' Clear the screen
30 ' Draw the flag of GDR (German Democratic Republic)
40 ' The GDR flag had black, red, and gold horizontal stripes
50 ' with the state emblem in the center
60 LINE (0, 0)-(639, 159), 0, BF ' Black stripe (top)
70 LINE (0, 160)-(639, 319), 4, BF ' Red stripe (middle)
80 LINE (0, 320)-(639, 479), 14, BF ' Gold/Yellow stripe (bottom)
90 ' Draw simplified state emblem (hammer and compass in wreath)
100 ' Draw a simple circular emblem representation
110 CIRCLE (320, 240), 60, 15 ' White circle for emblem background
120 PAINT (320, 240), 15, 15 ' Fill the circle white
130 ' Draw simplified hammer and compass symbol
140 LINE (300, 220)-(340, 220), 0 ' Horizontal line for hammer
150 LINE (300, 220)-(310, 210), 0 ' Hammer handle
160 LINE (330, 220)-(340, 210), 0 ' Compass leg
170 LINE (330, 220)-(340, 230), 0 ' Compass leg
180 CIRCLE (320, 225), 15, 0 ' Circle for compass
190 ' Draw wheat wreath outline
200 CIRCLE (320, 240), 55, 14 ' Golden wreath outline
210 ' Play GDR anthem melody (simplified version)
220 PLAY "T100 O4 L4 C D E F G A G F E D C C"
230 PLAY "L4 E F G A B- A G F E F G F E D C"
240 PLAY "L4 G A B- C5 D5 C5 B- A G A B- A G F"
250 PLAY "L4 C D E F G F E D C C C C"
260 END
