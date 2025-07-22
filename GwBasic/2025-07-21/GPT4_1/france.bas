10 REM Draw France flag and play anthem in GW-BASIC (EGA mode)
20 SCREEN 9: REM EGA 640x350, 16 colors
30 CLS
40 REM Draw blue left third
50 LINE (0,0)-(213,349), 1, BF
60 REM Draw white middle third
70 LINE (214,0)-(425,349), 15, BF
80 REM Draw red right third
90 LINE (426,0)-(639,349), 4, BF
100 REM Display text
110 COLOR 15: LOCATE 2, 28: PRINT "FRANCE"
120 REM Play French anthem (La Marseillaise, simplified)
130 PLAY "T180 L8 O4 G G A B C5 C5 D5 C5 B A G A B C5 D5 D5 E5 D5 C5 B A B C5 D5 C5 B A G"
140 END
