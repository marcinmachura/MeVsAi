10 REM French Flag Display and National Anthem
20 REM Program to draw French flag in EGA mode and play La Marseillaise
30 REM Created: July 21, 2025
40 REM
50 CLS
60 PRINT "French Flag Display and National Anthem"
70 PRINT "Press any key to continue..."
80 A$ = INPUT$(1)
90 REM
100 REM Set EGA graphics mode (640x350, 16 colors)
110 SCREEN 9
120 CLS
130 REM
140 REM Define colors for French flag
150 REM Color 1 = Blue, Color 15 = White, Color 4 = Red
160 REM
170 REM Draw French tricolor flag (3 vertical sections)
180 REM Left blue section
190 LINE (50, 50)-(233, 250), 1, BF
200 REM Middle white section
210 LINE (233, 50)-(416, 250), 15, BF
220 REM Right red section
230 LINE (416, 50)-(590, 250), 4, BF
240 REM
250 REM Add flag border
260 LINE (49, 49)-(591, 251), 0, B
270 REM
280 REM Display title
290 LOCATE 1, 28: PRINT "FRANCE"
300 LOCATE 2, 22: PRINT "LIBERTE, EGALITE, FRATERNITE!"
310 REM
320 REM Play La Marseillaise (simplified melody)
330 PRINT : PRINT "Playing La Marseillaise..."
340 REM Note: This is a simplified arrangement of the public domain melody
350 REM Using basic melody structure
360 PLAY "T100 L4 O4"
370 REM
380 REM Opening phrase "Allons enfants de la Patrie"
390 PLAY "G4 G4 G4 A4 B-4 B-4"
400 PLAY "A4 A4 A4 B-4 C5 C5"
410 PLAY "B-4 A4 G4 A4 B-2"
420 REM
430 REM "Le jour de gloire est arrive"
440 PLAY "G4 G4 G4 A4 B-4 B-4"
450 PLAY "A4 A4 A4 B-4 C5 C5"
460 PLAY "B-4 A4 G4 F4 G2"
470 REM
480 REM "Contre nous de la tyrannie"
490 PLAY "D5 D5 D5 C5 B-4 A4"
500 PLAY "G4 A4 B-4 C5 D5 D5"
510 PLAY "C5 B-4 A4 G4 A2"
520 REM
530 REM "L'etendard sanglant est leve"
540 PLAY "D5 D5 D5 C5 B-4 A4"
550 PLAY "G4 A4 B-4 C5 D5 D5"
560 PLAY "C5 B-4 A4 G4 G2"
570 REM
580 REM Chorus "Aux armes, citoyens!"
590 PLAY "P4 G4 A4 B-4 C5 D5"
600 PLAY "E-5 D5 C5 B-4 A4 G4"
610 PLAY "A4 B-4 C5 D5 E-5 F5"
620 PLAY "G5 F5 E-5 D5 C5 B-4"
630 REM
640 REM "Formez vos bataillons"
650 PLAY "C5 C5 C5 B-4 A4 G4"
660 PLAY "F4 G4 A4 B-4 C5 D5"
670 PLAY "E-5 D5 C5 B-4 A4 G4"
680 REM
690 REM "Marchons, marchons!"
700 PLAY "G4 A4 B-4 C5 D5 E-5"
710 PLAY "F5 E-5 D5 C5 B-4 A4"
720 PLAY "G4 A4 B-4 C5 G4 G4"
730 REM
740 REM Final phrase "Qu'un sang impur abreuve nos sillons"
750 PLAY "B-4 B-4 B-4 A4 G4 F4"
760 PLAY "E-4 F4 G4 A4 B-4 C5"
770 PLAY "D5 C5 B-4 A4 G4 F4"
780 PLAY "G4 A4 B-4 C5 D5 E-5"
790 PLAY "F5 E-5 D5 C5 B-4 A4"
800 PLAY "G2 G2 G1"
810 REM
820 REM End of anthem
830 PRINT : PRINT "La Marseillaise complete!"
840 PRINT : PRINT "Press any key to return to text mode..."
850 A$ = INPUT$(1)
860 REM
870 REM Return to text mode
880 SCREEN 0
890 CLS
900 PRINT "Merci for viewing the French flag!"
910 PRINT "Vive la France!"
920 PRINT "Program completed."
930 END
