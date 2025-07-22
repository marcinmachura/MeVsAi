10 REM American Flag Display and National Anthem
20 REM Program to draw American flag in EGA mode and play The Star-Spangled Banner
30 REM Created: July 21, 2025
40 REM
50 CLS
60 PRINT "American Flag Display and National Anthem"
70 PRINT "Press any key to continue..."
80 A$ = INPUT$(1)
90 REM
100 REM Set EGA graphics mode (640x350, 16 colors)
110 SCREEN 9
120 CLS
130 REM
140 REM Define colors for American flag
150 REM Color 4 = Red, Color 15 = White, Color 1 = Blue
160 REM
170 REM Draw flag background (13 stripes)
180 REM Calculate stripe dimensions
190 FW = 520: FH = 270: FX = 60: FY = 40
200 SH = FH / 13: REM Stripe height
210 REM
220 REM Draw 13 alternating red and white stripes
230 FOR I = 0 TO 12
240   Y1 = FY + I * SH
250   Y2 = Y1 + SH
260   IF I MOD 2 = 0 THEN SC = 4 ELSE SC = 15: REM Red or white
270   LINE (FX, Y1)-(FX + FW, Y2), SC, BF
280 NEXT I
290 REM
300 REM Draw blue canton (union) for stars
310 CW = FW * 0.76 / 2: REM Canton width (about 38% of flag width)
320 CH = SH * 7: REM Canton height (7 stripes)
330 LINE (FX, FY)-(FX + CW, FY + CH), 1, BF
340 REM
350 REM Draw stars (simplified as small rectangles)
360 REM 50 stars arranged in 9 rows (alternating 6 and 5 stars)
370 SW = 8: SHT = 6: REM Star width and height
380 FOR ROW = 0 TO 8
390   IF ROW MOD 2 = 0 THEN STARS = 6 ELSE STARS = 5
400   SY = FY + 8 + ROW * (CH - 16) / 8
410   IF STARS = 6 THEN SX = FX + 12 ELSE SX = FX + 24
420   SSTEP = (CW - 24) / (STARS - 1)
430   FOR COL = 0 TO STARS - 1
440     X1 = SX + COL * SSTEP - SW/2
450     Y1 = SY - SHT/2
460     LINE (X1, Y1)-(X1 + SW, Y1 + SHT), 15, BF
470   NEXT COL
480 NEXT ROW
490 REM
500 REM Add flag border
510 LINE (FX-1, FY-1)-(FX + FW + 1, FY + FH + 1), 0, B
520 REM
530 REM Display title
540 LOCATE 1, 28: PRINT "UNITED STATES OF AMERICA"
550 LOCATE 2, 32: PRINT "LAND OF THE FREE"
560 REM
570 REM Play The Star-Spangled Banner (simplified melody)
580 PRINT : PRINT "Playing The Star-Spangled Banner..."
590 REM Note: This is a simplified arrangement of the public domain melody
600 PLAY "T100 L4 O4"
610 REM "Oh say can you see"
620 PLAY "G8 E8 C4 E4 G4"
630 REM "by the dawn's early light"
640 PLAY "O5 C4 O4 G4 E4 C4"
650 REM "What so proudly we hailed"
660 PLAY "G8 G8 G4 A4 B4"
670 REM "at the twilight's last gleaming"
680 PLAY "O5 C4 C4 O4 B4 A4 G4"
690 REM
700 REM "Whose broad stripes and bright stars"
710 PLAY "G8 E8 C4 E4 G4"
720 REM "through the perilous fight"
730 PLAY "O5 C4 O4 G4 E4 C4"
740 REM "O'er the ramparts we watched"
750 PLAY "G4 A4 B4 O5 C4"
760 REM "were so gallantly streaming"
770 PLAY "D4 D4 C4 O4 B4 A4 G4"
780 REM
790 REM "And the rocket's red glare"
800 PLAY "O5 E4 E4 E4 D4"
810 REM "the bombs bursting in air"
820 PLAY "C4 O4 A4 B4 O5 C4"
830 REM "Gave proof through the night"
840 PLAY "O4 A4 B4 O5 C4 D4"
850 REM "that our flag was still there"
860 PLAY "E4 E4 D4 C4 O4 B4 A4"
870 REM
880 REM "Oh say does that star-spangled"
890 PLAY "G4 E4 C4 E4 G4 A4 B4"
900 REM "banner yet wave"
910 PLAY "O5 C4 C4 O4 B4 A4"
920 REM "O'er the land of the free"
930 PLAY "G4 A4 B4 O5 C4 D4 E4"
940 REM "and the home of the brave"
950 PLAY "G4 E4 C4 O4 G4 C2"
960 REM
970 REM End of anthem
980 PRINT : PRINT "The Star-Spangled Banner complete!"
990 PRINT : PRINT "Press any key to return to text mode..."
1000 A$ = INPUT$(1)
1010 REM
1020 REM Return to text mode
1030 SCREEN 0
1040 CLS
1050 PRINT "Thank you for viewing the American flag!"
1060 PRINT "God Bless America!"
1070 END
