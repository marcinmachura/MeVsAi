10 REM Draw Morocco flag and play anthem in GW-BASIC (EGA mode)
20 SCREEN 9: REM EGA 640x350, 16 colors
30 CLS
40 REM Draw red background
50 LINE (0,0)-(639,349),4,BF
60 REM Draw green star (simplified pentagram)
70 FOR I=0 TO 4
80   X1=320+80*COS(I*1.2566): Y1=175-80*SIN(I*1.2566)
90   X2=320+80*COS((I+2)*1.2566): Y2=175-80*SIN((I+2)*1.2566)
100  LINE (X1,Y1)-(X2,Y2),2
110 NEXT
120 REM Display text
130 COLOR 2: LOCATE 2, 28: PRINT "MOROCCO"
140 REM Play Moroccan anthem (Cherifian Anthem, simplified)
150 PLAY "T180 L8 O4 G G A B C5 D5 E5 D5 C5 B A G A B C5 D5 E5 D5 C5 B A G"
160 END
