10 REM Draw UK flag and play anthem in GW-BASIC (EGA mode)
20 SCREEN 9: REM EGA 640x350, 16 colors
30 CLS
40 REM Draw blue background
50 LINE (0,0)-(639,349), 1, BF
60 REM Draw white diagonals
70 LINE (0,0)-(639,349), 15
80 LINE (639,0)-(0,349), 15
90 REM Draw red diagonals
100 LINE (40,0)-(639,309), 4
110 LINE (599,0)-(0,309), 4
120 REM Draw white cross
130 LINE (213,0)-(425,349), 15, BF
140 LINE (0,140)-(639,210), 15, BF
150 REM Draw red cross
160 LINE (255,0)-(383,349), 4, BF
170 LINE (0,162)-(639,188), 4, BF
180 REM Display text
190 COLOR 15: LOCATE 2, 28: PRINT "UNITED KINGDOM"
200 REM Play UK anthem (God Save the King, simplified)
210 PLAY "T80 L8 O4 G G A G C5 B A G F# G A B C5 B A G A B G"
220 END
