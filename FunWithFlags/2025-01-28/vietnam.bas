10 REM Set up the screen mode
20 SCREEN 12 ' 640x480 resolution, 16 colors
30 REM Define colors
40 RED = 4 ' Red color code
50 YELLOW = 14 ' Yellow color code
60 REM Draw the red background
70 CLS ' Clear the screen
80 COLOR RED ' Set the color to red
90 LINE (0, 0)-(639, 479),, BF ' Draw a filled rectangle for the background
100 REM Calculate the center of the flag
110 CENTERX = 320
120 CENTERY = 240
130 REM Calculate the radius of the star
140 RADIUS = 50
150 REM Draw the yellow star
160 COLOR YELLOW ' Set the color to yellow
170 FOR I = 0 TO 4 ' Loop through the five points of the star
180     ANGLE = I * 144 ' Calculate the angle for each point
190     X = CENTERX + RADIUS * COS(RAD(ANGLE)) ' Calculate the x-coordinate of the point
200     Y = CENTERY + RADIUS * SIN(RAD(ANGLE)) ' Calculate the y-coordinate of the point
210     IF I = 0 THEN ' If it's the first point, start a new line
220         LINE (X, Y)-(X, Y)
230     ELSE ' Otherwise, draw a line to the previous point
240         LINE -(X, Y)
250     END IF
260 NEXT I
270 LINE -(CENTERX + RADIUS, CENTERY) ' Close the star by drawing a line back to the first point
280 END