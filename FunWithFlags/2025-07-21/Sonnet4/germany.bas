10 REM German Flag Display and National Anthem
20 REM Program to draw German flag in EGA mode and play Deutschlandlied
30 REM Created: July 21, 2025
40 REM
50 CLS
60 PRINT "German Flag Display and National Anthem"
70 PRINT "Press any key to continue..."
80 A$ = INPUT$(1)
90 REM
100 REM Set EGA graphics mode (640x350, 16 colors)
110 SCREEN 9
120 CLS
130 REM
140 REM Define colors for German flag
150 REM Color 0 = Black, Color 4 = Red, Color 14 = Yellow/Gold
160 REM
170 REM Draw German tricolor flag (3 horizontal stripes)
180 REM Flag dimensions following 3:5 ratio
190 FW = 500: FH = 300: FX = 70: FY = 25
200 SH = FH / 3: REM Stripe height
210 REM
220 REM Top black stripe
230 LINE (FX, FY)-(FX + FW, FY + SH), 0, BF
240 REM Middle red stripe
250 LINE (FX, FY + SH)-(FX + FW, FY + 2 * SH), 4, BF
260 REM Bottom gold/yellow stripe
270 LINE (FX, FY + 2 * SH)-(FX + FW, FY + FH), 14, BF
280 REM
290 REM Add flag border
300 LINE (FX-1, FY-1)-(FX + FW + 1, FY + FH + 1), 15, B
310 REM
320 REM Display title
330 LOCATE 1, 28: PRINT "DEUTSCHLAND"
340 LOCATE 2, 24: PRINT "BUNDESREPUBLIK DEUTSCHLAND"
350 REM
360 REM Play Deutschlandlied (German National Anthem)
370 PRINT : PRINT "Playing Deutschlandlied..."
380 REM Note: This is a simplified arrangement of the public domain melody
390 REM Based on Haydn's melody "Gott erhalte Franz den Kaiser"
400 PLAY "T100 L4 O4"
410 REM
420 REM Opening phrase "Einigkeit und Recht und Freiheit"
430 PLAY "G4 G4 A4 F4 B-4 B-4"
440 PLAY "A4 A4 B-4 G4 C5 C5"
450 PLAY "B-4 A4 G4 A4 F2 P4"
460 REM
470 REM "Fuer das deutsche Vaterland"
480 PLAY "G4 G4 A4 F4 B-4 B-4"
490 PLAY "A4 A4 B-4 G4 C5 C5"
500 PLAY "B-4 A4 G4 F4 G2 P4"
510 REM
520 REM "Danach lasst uns alle streben"
530 PLAY "D5 D5 C5 B-4 A4 A4"
540 PLAY "B-4 B-4 A4 G4 F4 F4"
550 PLAY "G4 A4 B-4 C5 D5 C5"
560 PLAY "B-4 A4 G4 F4 G2 P4"
570 REM
580 REM "Bruederlich mit Herz und Hand"
590 PLAY "D5 D5 C5 B-4 A4 A4"
600 PLAY "B-4 B-4 A4 G4 F4 F4"
610 PLAY "G4 A4 B-4 C5 D5 C5"
620 PLAY "B-4 A4 G4 F4 G2 P4"
630 REM
640 REM Triumphant refrain "Einigkeit und Recht und Freiheit"
650 PLAY "G4 A4 B-4 C5 D5 D5"
660 PLAY "C5 B-4 A4 B-4 C5 D5"
670 PLAY "E-5 D5 C5 B-4 A4 B-4"
680 PLAY "C5 B-4 A4 G4 F2 P4"
690 REM
700 REM "Sind des Glueckes Unterpfand"
710 PLAY "G4 A4 B-4 C5 D5 D5"
720 PLAY "C5 B-4 A4 B-4 C5 D5"
730 PLAY "E-5 D5 C5 B-4 A4 B-4"
740 PLAY "C5 B-4 A4 F4 G2 P4"
750 REM
760 REM Majestic conclusion
770 PLAY "B-4 C5 D5 E-5 F5 F5"
780 PLAY "E-5 D5 C5 D5 E-5 F5"
790 PLAY "G5 F5 E-5 D5 C5 D5"
800 PLAY "E-5 D5 C5 B-4 A4 G4"
810 PLAY "F4 G4 A4 B-4 C5 B-4"
820 PLAY "A4 G4 F4 E-4 D4 C4"
830 PLAY "B-3 C4 D4 E-4 F4 G4"
840 PLAY "A4 B-4 C5 D5 C5 B-4"
850 PLAY "A4 G4 F4 G2 P2"
860 REM
870 REM Final patriotic flourish
880 PLAY "O5 C4 D4 E-4 F4 G4 F4"
890 PLAY "E-4 D4 C4 O4 B-4 A4 G4"
890 PLAY "A4 B-4 O5 C4 D4 E-4 F4"
900 PLAY "G4 F4 E-4 D4 C4 O4 B-4"
910 PLAY "A4 G4 F4 E-4 D4 C4"
920 PLAY "D4 E-4 F4 G4 A4 B-4"
930 PLAY "O5 C4 O4 B-4 A4 G4 F4 G2"
940 REM
950 REM End of anthem
960 PRINT : PRINT "Deutschlandlied complete!"
970 PRINT : PRINT "Press any key to return to text mode..."
980 A$ = INPUT$(1)
990 REM
1000 REM Return to text mode
1010 SCREEN 0
1020 CLS
1030 PRINT "Thank you for viewing the German flag!"
1040 PRINT "Einigkeit und Recht und Freiheit!"
1050 END
