@ECHO OFF
ECHO CONVERTING THE DOCX DOC INTO HTML FOR EASIER READINESS INTO GITHUB
ECHO PRE-REQUISITE: 
ECHO   - NEED TO INSTALL pandoc converter https://github.com/jgm/pandoc/releases
C:\Users\%username%\AppData\Local\Pandoc\pandoc -s -o wii-wars-by-VISEO-Technical_Documentation.html wii-wars-by-VISEO-Technical_DocOriginal.docx
PAUSE