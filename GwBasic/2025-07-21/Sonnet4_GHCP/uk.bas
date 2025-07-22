10 REM UK FLAG AND ANTHEM PROGRAM
20 REM Sets EGA mode and draws Union Jack flag
30 REM Plays God Save the King
40 REM
50 SCREEN 9: CLS
60 REM EGA mode 640x350, 16 colors
70 REM
80 REM Define colors
90 REM 0=Black, 1=Blue, 4=Red, 15=White
100 REM
110 REM Fill background with blue
120 FOR Y = 0 TO 349
130 LINE (0, Y)-(639, Y), 1
140 NEXT Y
150 REM
160 REM Draw white diagonal crosses (St. Andrew's cross)
170 REM Main diagonals
180 FOR I = 0 TO 639 STEP 2
190 PSET (I, I * 349 / 639), 15
200 PSET (I, 349 - I * 349 / 639), 15
210 NEXT I
220 REM
230 REM Thicken the diagonal lines
240 FOR I = 0 TO 639 STEP 2
250 Y1 = I * 349 / 639
260 Y2 = 349 - I * 349 / 639
270 FOR J = -3 TO 3
280 IF Y1 + J >= 0 AND Y1 + J <= 349 THEN PSET (I, Y1 + J), 15
290 IF Y2 + J >= 0 AND Y2 + J <= 349 THEN PSET (I, Y2 + J), 15
300 NEXT J
310 NEXT I
320 REM
330 REM Draw red diagonal crosses on top of white
340 FOR I = 0 TO 639 STEP 2
350 Y1 = I * 349 / 639
360 Y2 = 349 - I * 349 / 639
370 PSET (I, Y1), 4
380 PSET (I, Y2), 4
390 NEXT I
400 REM
410 REM Draw white vertical and horizontal crosses (St. George's cross base)
420 REM Vertical white stripe
430 FOR Y = 0 TO 349
440 FOR X = 300 TO 339
450 PSET (X, Y), 15
460 NEXT X
470 NEXT Y
480 REM
490 REM Horizontal white stripe  
500 FOR X = 0 TO 639
510 FOR Y = 155 TO 194
520 PSET (X, Y), 15
530 NEXT Y
540 NEXT X
550 REM
560 REM Draw red St. George's cross (smaller, centered)
570 REM Vertical red stripe
580 FOR Y = 0 TO 349
590 FOR X = 310 TO 329
600 PSET (X, Y), 4
610 NEXT X
620 NEXT Y
630 REM
640 REM Horizontal red stripe
650 FOR X = 0 TO 639
660 FOR Y = 165 TO 184
670 PSET (X, Y), 4
680 NEXT Y
690 NEXT X
700 REM
710 REM Display title
720 LOCATE 1, 1
730 COLOR 15, 1
740 PRINT "UNITED KINGDOM - Union Jack Flag"
750 LOCATE 2, 1
760 PRINT "Press any key to play God Save the King..."
770 REM
780 REM Wait for keypress
790 A$ = INPUT$(1)
800 REM
810 REM Play God Save the King (UK National Anthem)
820 LOCATE 24, 1
830 PRINT "Playing: God Save the King..."
840 REM
850 REM Main melody of God Save the King
860 PLAY "T120"
870 PLAY "O4 G2 G4 A4 G4 C5 B4 B2"
880 PLAY "G2 G4 A4 G4 D5 C5 C2"
890 PLAY "D5 D5 C5 B4 C5 A4 G4 A4"
900 PLAY "B4 B4 A4 G4 A4 F#4 G2"
910 REM
920 REM Second verse
930 PLAY "O4 G2 G4 A4 G4 C5 B4 B2"
940 PLAY "G2 G4 A4 G4 D5 C5 C2"
950 PLAY "G5 F#5 E5 D5 E5 C5 B4 A4"
960 PLAY "G4 C5 C5 B4 A4 A4 G2"
970 REM
980 REM Final flourish
990 PLAY "O5 G4 G4 F#4 G4 C4 C2"
1000 REM
1010 LOCATE 25, 1
1020 PRINT "Anthem complete! Press any key to exit..."
1030 A$ = INPUT$(1)
1040 SCREEN 0: CLS
1050 END
