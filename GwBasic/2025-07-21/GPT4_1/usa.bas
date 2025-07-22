10 REM Draw USA flag and play anthem in GW-BASIC (EGA mode)
20 SCREEN 9: REM EGA 640x350, 16 colors
30 CLS
40 REM Draw 13 stripes
50 FOR I=0 TO 12
60   C=15: IF I MOD 2=1 THEN C=4
70   LINE (0,I*27)-(639,I*27+26),C,BF
80 NEXT
90 REM Draw blue canton
100 LINE (0,0)-(263,208),1,BF
110 REM Display text
120 COLOR 15: LOCATE 2, 28: PRINT "USA"
130 REM Play USA anthem (Star-Spangled Banner, simplified)
140 PLAY "T180 L8 O4 G G E G A B C5 B A G E G A B C5 B A G"
150 END
