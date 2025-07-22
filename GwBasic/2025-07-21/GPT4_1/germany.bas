10 REM Draw Germany flag and play anthem in GW-BASIC (EGA mode)
20 SCREEN 9: REM EGA 640x350, 16 colors
30 CLS
40 REM Draw black top third
50 LINE (0,0)-(639,116),0,BF
60 REM Draw red middle third
70 LINE (0,117)-(639,232),4,BF
80 REM Draw yellow bottom third
90 LINE (0,233)-(639,349),14,BF
100 REM Display text
110 COLOR 15: LOCATE 2, 28: PRINT "GERMANY"
120 REM Play German anthem (Deutschlandlied, simplified)
130 PLAY "T180 L8 O4 G G D G G A B C5 B A G D G G A B C5 B A G"
140 END
