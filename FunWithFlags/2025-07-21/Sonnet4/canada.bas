10 REM Canadian Flag Display and National Anthem
20 REM Program to draw Canadian flag in EGA mode and play O Canada
30 REM Created: July 21, 2025
40 REM
50 CLS
60 PRINT "Canadian Flag Display and National Anthem"
70 PRINT "Press any key to continue..."
80 A$ = INPUT$(1)
90 REM
100 REM Set EGA graphics mode (640x350, 16 colors)
110 SCREEN 9
120 CLS
130 REM
140 REM Define colors for Canadian flag
150 REM Color 4 = Red, Color 15 = White
160 REM
170 REM Draw flag background (3 vertical sections)
180 REM Left red section
190 LINE (50, 50)-(213, 250), 4, BF
200 REM Middle white section
210 LINE (213, 50)-(426, 250), 15, BF
220 REM Right red section
230 LINE (426, 50)-(590, 250), 4, BF
240 REM
250 REM Draw maple leaf in center (simplified version)
260 REM Maple leaf stem
270 LINE (319, 200)-(321, 220), 4, BF
280 REM
290 REM Maple leaf body (simplified geometric representation)
300 REM Main body of leaf
310 LINE (295, 120)-(345, 180), 4, BF
320 REM Upper points
330 LINE (305, 100)-(335, 120), 4, BF
340 LINE (315, 80)-(325, 100), 4, BF
350 REM Side points
360 LINE (280, 140)-(295, 160), 4, BF
370 LINE (345, 140)-(360, 160), 4, BF
380 REM Lower points
390 LINE (300, 180)-(320, 200), 4, BF
400 LINE (320, 180)-(340, 200), 4, BF
410 REM
420 REM Add flag border
430 LINE (49, 49)-(591, 251), 0, B
440 REM
450 REM Display title
460 LOCATE 1, 28: PRINT "CANADA"
470 LOCATE 2, 25: PRINT "EH! TRUE NORTH!"
480 REM
490 REM Play O Canada (simplified melody)
500 PRINT : PRINT "Playing O Canada..."
510 REM Note: This is a simplified arrangement avoiding copyrighted lyrics
520 REM Using basic melody structure in public domain
530 PLAY "T120 L4 O4"
540 REM Opening phrase
550 PLAY "G2 G8 A8 B-2 B-8 C8 D2"
560 PLAY "E-2 E-8 D8 C2 B-8 C8 D2"
570 PLAY "G2 G8 A8 B-2 B-8 C8 D2"
580 PLAY "E-2 D8 C8 B-2 P2"
590 REM
600 REM Second phrase
610 PLAY "B-2 B-8 C8 D2 D8 E-8 F2"
620 PLAY "G2 F8 E-8 D2 C8 D8 E-2"
630 PLAY "F2 E-8 D8 C2 B-8 C8 D2"
640 PLAY "G2 F8 E-8 D2 P2"
650 REM
660 REM Final phrase
670 PLAY "G2 A8 B-8 C4 D4 E-2"
680 PLAY "D2 C8 B-8 C2 P2"
690 PLAY "B-2 C8 D8 E-2 F8 G8 A2"
700 PLAY "B-2 A8 G8 A4 G4 G2"
710 REM
720 REM End of anthem
730 PRINT : PRINT "O Canada complete!"
740 PRINT : PRINT "Press any key to return to text mode..."
750 A$ = INPUT$(1)
760 REM
770 REM Return to text mode
780 SCREEN 0
790 CLS
800 PRINT "Thank you for viewing the Canadian flag!"
810 PRINT "Program completed."
820 END
