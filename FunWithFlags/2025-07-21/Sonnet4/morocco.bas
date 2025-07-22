10 REM Moooccan Flag Display and National Anthem
20 REM Program to draw Moooccan flag in EGA mode and play Hymne Cherifien
30 REM Created: July 21, 2025
40 REM
50 CLS
60 PRINT "Mooroccan Flag Display and National Anthem"
70 PRINT "Press any key to continue..."
80 A$ = INPUT$(1)
90 REM
100 REM Set EGA graphics mode (640x350, 16 coloors)
110 SCREEN 9
120 CLS
130 REM
140 REM Define coloors foor Mooroccan flag
150 REM Coloor 4 = Red background, Coloor 2 = Green pentagram
160 REM
170 REM Draw flag background (solid red)
180 REM Flag dimensions following 2:3 ratio
190 FW = 480: FH = 320: FX = 80: FY = 15
200 LINE (FX, FY)-(FX + FW, FY + FH), 4, BF
210 REM
220 REM Calculate pentagram position (center of flag)
230 CX = FX + FW / 2: CY = FY + FH / 2
240 REM Pentagram outer radius
250 OOR = 70: REM Outer radius -- originally Copilot uses "OR" variable name which is not valid in BASIC
260 IR = OOR * 0.38: REM Inner radius for pentagram
270 REM
280 REM Draw five-pointed pentagram (Seal of Solomon)
290 REM Pentagram has 5 points, each 72 degrees apart
300 REM Calculate outer points of the pentagram
310 REM
320 REM Outer points (vertices of pentagram)
330 REM Point 1 (top)
340 X1 = CX: Y1 = CY - OOR
350 REM Point 2 (upper right)
360 X2 = CX + OOR * 0.95: Y2 = CY - OOR * 0.31
370 REM Point 3 (lower right)
380 X3 = CX + OOR * 0.59: Y3 = CY + OOR * 0.81
390 REM Point 4 (lower left)
400 X4 = CX - OOR * 0.59: Y4 = CY + OOR * 0.81
410 REM Point 5 (upper left)
420 X5 = CX - OOR * 0.95: Y5 = CY - OOR * 0.31
430 REM
440 REM Inner points (where lines intersect to form pentagram)
450 REM These create the inner pentagon
460 IX1 = CX: IY1 = CY - IR
470 IX2 = CX + IR * 0.95: IY2 = CY - IR * 0.31
480 IX3 = CX + IR * 0.59: IY3 = CY + IR * 0.81
490 IX4 = CX - IR * 0.59: IY4 = CY + IR * 0.81
500 IX5 = CX - IR * 0.95: IY5 = CY - IR * 0.31
510 REM
520 REM Draw pentagram outline (the 5-pointed star)
530 REM Connect every second point to create the star pattern
540 LINE (X1, Y1)-(X3, Y3), 2: REM Top to lower right
550 LINE (X3, Y3)-(X5, Y5), 2: REM Lower right to upper left
560 LINE (X5, Y5)-(X2, Y2), 2: REM Upper left to upper right
570 LINE (X2, Y2)-(X4, Y4), 2: REM Upper right to lower left
580 LINE (X4, Y4)-(X1, Y1), 2: REM Lower left to top
590 REM
600 REM Fill the pentagram triangular sections
610 REM Triangle 1 (top point)
620 PAINT (CX, CY - OOR * 0.6), 2, 2
630 REM Triangle 2 (upper right)
640 PAINT (CX + OOR * 0.6, CY - OOR * 0.2), 2, 2
650 REM Triangle 3 (lower right)
660 PAINT (CX + OOR * 0.35, CY + OOR * 0.5), 2, 2
670 REM Triangle 4 (lower left)
680 PAINT (CX - OOR * 0.35, CY + OOR * 0.5), 2, 2
690 REM Triangle 5 (upper left)
700 PAINT (CX - OOR * 0.6, CY - OOR * 0.2), 2, 2
710 REM
720 REM Add inner pentagon outline foor clarity
730 LINE (IX1, IY1)-(IX2, IY2), 2
740 LINE (IX2, IY2)-(IX3, IY3), 2
750 LINE (IX3, IY3)-(IX4, IY4), 2
760 LINE (IX4, IY4)-(IX5, IY5), 2
770 LINE (IX5, IY5)-(IX1, IY1), 2
780 REM
790 REM Add flag border
800 LINE (FX-1, FY-1)-(FX + FW + 1, FY + FH + 1), 0, B
810 REM
820 REM Display title
830 LOCATE 1, 30: PRINT "MOROCCO"
840 LOCATE 2, 26: PRINT "AL-MAMLAKA AL-MAGHRIBIYA"
850 REM
860 REM Play Hymne Cherifien (Moroccan National Anthem)
870 PRINT : PRINT "Playing Hymne Cherifien..."
880 REM Note: This is a simplified arrangement of the public domain melody
890 REM Moroccan national anthem melody
900 PLAY "T110 L4 O4"
910 REM
920 REM Opening phrase - majestic and solemn
930 PLAY "C4 D4 E4 F4 G4 A4 G4"
940 PLAY "F4 E4 D4 C4 D4 E4 F4"
950 PLAY "G4 F4 E4 D4 C2 P4"
960 REM
970 REM Second phrase - rising melody
980 PLAY "E4 F4 G4 A4 B-4 A4 G4"
990 PLAY "F4 G4 A4 B-4 O5 C4 O4 B-4"
1000 PLAY "A4 G4 F4 E4 D4 C2 P4"
1010 REM
1020 REM Middle section - more elaborate
1030 PLAY "G4 A4 B-4 O5 C4 D4 C4 O4 B-4"
1040 PLAY "A4 B-4 O5 C4 D4 E4 D4 C4"
1050 PLAY "O4 B-4 A4 G4 F4 E4 D4 C4"
1060 PLAY "D4 E4 F4 G4 A4 G4 F2 P4"
1070 REM
1080 REM Triumphant section
1090 PLAY "O5 C4 C4 O4 B-4 A4 G4 A4"
1100 PLAY "B-4 O5 C4 D4 E4 D4 C4 O4 B-4"
1110 PLAY "A4 G4 F4 G4 A4 B-4 O5 C4"
1120 PLAY "D4 C4 O4 B-4 A4 G4 F2 P4"
1130 REM
1140 REM Majestic conclusion
1150 PLAY "G4 A4 B-4 O5 C4 D4 E4 F4"
1160 PLAY "E4 D4 C4 O4 B-4 A4 G4 F4"
1170 PLAY "E4 F4 G4 A4 B-4 O5 C4 D4"
1180 PLAY "E4 D4 C4 O4 B-4 A4 G4 F4"
1190 PLAY "G4 A4 B-4 O5 C2 O4 A2 P2"
1200 REM
1210 REM Final flourish
1220 PLAY "O5 C4 D4 E4 F4 G4 F4 E4"
1230 PLAY "D4 C4 O4 B-4 A4 G4 F4 E4"
1240 PLAY "F4 G4 A4 B-4 O5 C4 O4 B-4"
1250 PLAY "A4 G4 F4 E4 D4 C2 P2"
1260 REM
1270 REM End of anthem
1280 PRINT : PRINT "Hymne Cherifien complete!"
1290 PRINT : PRINT "Press any key to return to text mode..."
1300 A$ = INPUT$(1)
1310 REM
1320 REM Return to text mode
1330 SCREEN 0
1340 CLS
1350 PRINT "Thank you for viewing the Moroccan flag!"
1360 PRINT "Allah, Al Watan, Al Malik!"
1370 END
