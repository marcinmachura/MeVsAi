10 REM Draw Poland flag and play anthem in GW-BASIC (EGA mode)
20 SCREEN 9: REM EGA 640x350, 16 colors
30 CLS
40 REM Draw white top half
50 LINE (0,0)-(639,174), 15, BF
60 REM Draw red bottom half
70 LINE (0,175)-(639,349), 4, BF
80 REM Display text
90 COLOR 15: LOCATE 2, 28: PRINT "POLAND"
100 REM Play Polish anthem (simplified)
110 PLAY "T180 L8 O4 C D E F G G A G F E D C D E F G G A G F E D C"
120 END
