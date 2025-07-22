10 REM Polish Flag and Anthem Program
20 REM Sets EGA graphics mode and draws Polish flag
30 REM Plays Polish national anthem melody
40 REM
50 SCREEN 9: REM EGA 640x350, 16 colors
60 CLS
70 REM
80 REM Draw Polish Flag (white top, red bottom)
90 REM
100 REM Set colors: 15=white, 4=red
110 REM
120 REM Draw white stripe (top half)
130 FOR Y = 50 TO 174
140   FOR X = 200 TO 440
150     PSET (X, Y), 15
160   NEXT X
170 NEXT Y
180 REM
190 REM Draw red stripe (bottom half)
200 FOR Y = 175 TO 299
210   FOR X = 200 TO 440
220     PSET (X, Y), 4
230   NEXT X
240 NEXT Y
250 REM
260 REM Draw flag border
270 LINE (200, 50)-(440, 299), 0, B
280 REM
290 REM Add title
300 LOCATE 3, 25
310 PRINT "POLSKA - POLAND"
320 REM
330 REM Play Polish National Anthem (Mazurek Dąbrowskiego)
340 REM First phrase of the melody
350 LOCATE 25, 20
360 PRINT "Playing Polish National Anthem..."
370 REM
380 REM Main melody line (simplified version)
390 PLAY "T120 O4 L4"
400 PLAY "G G A B- > C2 < B-4 A G"
410 PLAY "F G A2 G4 F E D2"
420 PLAY "G G A B- > C2 < B-4 A G"
430 PLAY "A B- > C2 D4 < B- A G2"
440 REM
450 REM Second phrase
460 PLAY "G G A B- > C2 < B-4 A G"
470 PLAY "F G A2 G4 F E D2"
480 PLAY "> D D E F G2 F4 E D"
490 PLAY "C D E2 F4 D C < B-2"
500 REM
510 REM Final phrase
520 PLAY "G G A B- > C2 < B-4 A G"
530 PLAY "A B- > C2 D4 < B- A G2"
540 REM
550 LOCATE 23, 15
560 PRINT "Press any key to continue..."
570 A$ = INPUT$(1)
580 SCREEN 0: REM Return to text mode
590 CLS
600 PRINT "Program completed."
610 END