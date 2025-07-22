10 REM Vietnamese Flag Display and National Anthem
20 REM Program to draw Vietnamese flag in EGA mode and play Tien Quan Ca
30 REM Created: July 21, 2025
40 REM
50 CLS
60 PRINT "Vietnamese Flag Display and National Anthem"
70 PRINT "Press any key to continue..."
80 A$ = INPUT$(1)
90 REM
100 REM Set EGA graphics mode (640x350, 16 colors)
110 SCREEN 9
120 CLS
130 REM
140 REM Define colors for Vietnamese flag
150 REM Color 4 = Red background, Color 14 = Yellow star
160 REM
170 REM Draw flag background (solid red)
180 REM Flag dimensions following 2:3 ratio
190 FW = 480: FH = 320: FX = 80: FY = 15
200 LINE (FX, FY)-(FX + FW, FY + FH), 4, BF
210 REM
220 REM Calculate star position (center of flag)
230 CX = FX + FW / 2: CY = FY + FH / 2
240 REM Star radius
250 SR = 60
260 REM
270 REM Draw five-pointed star (yellow)
280 REM Calculate star points using trigonometry approximation
290 REM Star has 5 points, each 72 degrees apart
300 REM Using simplified coordinate calculations
310 REM
320 REM Top point
330 X1 = CX: Y1 = CY - SR
340 REM Upper right point  
350 X2 = CX + SR * 0.95: Y2 = CY - SR * 0.31
360 REM Lower right point
370 X3 = CX + SR * 0.59: Y3 = CY + SR * 0.81
380 REM Lower left point
390 X4 = CX - SR * 0.59: Y4 = CY + SR * 0.81
400 REM Upper left point
410 X5 = CX - SR * 0.95: Y5 = CY - SR * 0.31
420 REM
430 REM Draw star by connecting points to form triangles
440 REM Inner pentagon first
450 IX1 = CX: IY1 = CY - SR * 0.38
460 IX2 = CX + SR * 0.36: IY2 = CY - SR * 0.12
470 IX3 = CX + SR * 0.22: IY3 = CY + SR * 0.31
480 IX4 = CX - SR * 0.22: IY4 = CY + SR * 0.31
490 IX5 = CX - SR * 0.36: IY5 = CY - SR * 0.12
500 REM
510 REM Draw star triangles
520 REM Triangle 1 (top)
530 LINE (X1, Y1)-(IX2, IY2), 14: LINE (IX2, IY2)-(IX5, IY5), 14: LINE (IX5, IY5)-(X1, Y1), 14
540 PAINT (CX, CY - SR * 0.2), 14, 14
550 REM Triangle 2 (upper right)
560 LINE (X2, Y2)-(IX3, IY3), 14: LINE (IX3, IY3)-(IX1, IY1), 14: LINE (IX1, IY1)-(X2, Y2), 14
570 PAINT (CX + SR * 0.4, CY), 14, 14
580 REM Triangle 3 (lower right)
590 LINE (X3, Y3)-(IX4, IY4), 14: LINE (IX4, IY4)-(IX2, IY2), 14: LINE (IX2, IY2)-(X3, Y3), 14
600 PAINT (CX + SR * 0.2, CY + SR * 0.4), 14, 14
610 REM Triangle 4 (lower left)
620 LINE (X4, Y4)-(IX5, IY5), 14: LINE (IX5, IY5)-(IX3, IY3), 14: LINE (IX3, IY3)-(X4, Y4), 14
630 PAINT (CX - SR * 0.2, CY + SR * 0.4), 14, 14
640 REM Triangle 5 (upper left)
650 LINE (X5, Y5)-(IX1, IY1), 14: LINE (IX1, IY1)-(IX4, IY4), 14: LINE (IX4, IY4)-(X5, Y5), 14
660 PAINT (CX - SR * 0.4, CY), 14, 14
670 REM
680 REM Add flag border
690 LINE (FX-1, FY-1)-(FX + FW + 1, FY + FH + 1), 0, B
700 REM
710 REM Display title
720 LOCATE 1, 30: PRINT "VIETNAM"
730 LOCATE 2, 26: PRINT "CONG HOA XA HOI CHU NGHIA"
740 REM
750 REM Play Tien Quan Ca (Vietnamese National Anthem)
760 PRINT : PRINT "Playing Tien Quan Ca..."
770 REM Note: This is a simplified arrangement of the public domain melody
780 REM Vietnamese national anthem melody
790 PLAY "T120 L4 O4"
800 REM
810 REM Opening phrase of Tien Quan Ca
820 PLAY "C4 D4 E4 F4 G4 A4"
830 PLAY "G4 F4 E4 D4 C4 D4"
840 PLAY "E4 F4 G4 A4 G4 F4"
850 PLAY "E4 D4 C2 P4"
860 REM
870 REM Second phrase
880 PLAY "E4 F4 G4 A4 B-4 A4"
890 PLAY "G4 F4 E4 F4 G4 A4"
900 PLAY "B-4 A4 G4 F4 E4 F4"
910 PLAY "G4 F4 E4 D4 C2 P4"
920 REM
930 REM Rising melodic section
940 PLAY "G4 A4 B-4 O5 C4 D4 C4"
950 PLAY "O4 B-4 A4 G4 F4 E4 F4"
960 PLAY "G4 A4 B-4 O5 C4 O4 B-4 A4"
970 PLAY "G4 F4 E4 D4 C2 P4"
980 REM
990 REM Triumphant conclusion
1000 PLAY "O5 C4 C4 O4 B-4 A4 G4 A4"
1010 PLAY "B-4 O5 C4 D4 C4 O4 B-4 A4"
1020 PLAY "G4 A4 B-4 O5 C4 D4 E4"
1030 PLAY "F4 E4 D4 C4 O4 B-4 A4"
1040 PLAY "G4 F4 E4 D4 C2"
1050 REM
1060 REM Final patriotic flourish
1070 PLAY "O5 C4 D4 E4 F4 G4 F4"
1080 PLAY "E4 D4 C4 O4 G4 A4 B-4"
1090 PLAY "O5 C4 O4 B-4 A4 G4 F4 E4"
1100 PLAY "D4 C4 D4 E4 F4 G4 A4"
1110 PLAY "G4 F4 E4 D4 C2 P2"
1120 REM
1130 REM End of anthem
1140 PRINT : PRINT "Tien Quan Ca complete!"
1150 PRINT : PRINT "Press any key to return to text mode..."
1160 A$ = INPUT$(1)
1170 REM
1180 REM Return to text mode
1190 SCREEN 0
1200 CLS
1210 PRINT "Thank you for viewing the Vietnamese flag!"
1220 PRINT "Chuc mung nam moi! (Happy New Year!)"
1230 END
