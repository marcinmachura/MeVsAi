10 REM vietnam.bas - Draws the flag of Vietnam and plays the anthem
20 SCREEN 9 ' EGA 640x350, 16 colors
30 REM Draw red background
40 COLOR 4
50 FOR Y = 0 TO 349
60     LINE (0, Y)-(639, Y), 4
70 NEXT
80 REM Draw yellow star (centered)
90 REM Star coordinates (approximate, scaled for EGA)
100 CX = 320: CY = 175: R = 70
110 FOR I = 0 TO 4
120     ANG1 = (I * 72 - 90) * 3.14159 / 180
130     ANG2 = ((I * 72 + 36) - 90) * 3.14159 / 180
140     X1 = CX + R * COS(ANG1)
150     Y1 = CY + R * SIN(ANG1)
160     X2 = CX + R / 2.5 * COS(ANG2)
170     Y2 = CY + R / 2.5 * SIN(ANG2)
180     X3 = CX + R * COS(ANG1 + 72 * 3.14159 / 180)
190     Y3 = CY + R * SIN(ANG1 + 72 * 3.14159 / 180)
200     LINE (CX, CY)-(X1, Y1), 14
210     LINE (X1, Y1)-(X2, Y2), 14
220     LINE (X2, Y2)-(X3, Y3), 14
230     LINE (X3, Y3)-(CX, CY), 14
240     PAINT (CX + (R / 2) * COS(ANG1 + 36 * 3.14159 / 180), CY + (R / 2) * SIN(ANG1 + 36 * 3.14159 / 180)), 14, 14
250 NEXT
260 REM Play anthem (simple melody, not full anthem)
270 PLAY "T200 L8 O4 C D E F G G F E D C"
280 END
