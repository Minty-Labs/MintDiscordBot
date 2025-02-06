@echo off
setlocal enabledelayedexpansion

set num=0

:loop
if %num% GTR 10 (
    pause
    exit
)

.\Twscrape.py
set /a num+=1

timeout /t 3 /nobreak >nul

goto loop
