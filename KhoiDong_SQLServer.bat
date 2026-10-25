@echo off
chcp 65001 >nul
:: Kiem tra quyen Administrator, neu chua co thi tu dong mo UAC xin quyen
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [THONG BAO] Dang yeu cau quyen Administrator de khoi dong dich vu SQL Server...
    powershell -Command "Start-Process cmd -ArgumentList '/c %~dpnx0' -Verb RunAs"
    exit /b
)

echo ====================================================================
echo      DANG KIEM TRA VA KHOI DONG DICH VU SQL SERVER (SQLEXPRESS)     
echo ====================================================================
echo.

sc config MSSQL$SQLEXPRESS start= auto >nul 2>&1
net start MSSQL$SQLEXPRESS

sc config SQLBrowser start= auto >nul 2>&1
net start SQLBrowser >nul 2>&1

echo.
echo ====================================================================
echo      HOAN TAT! Dich vu SQL Server SQLEXPRESS da chay binh thuong.   
echo ====================================================================
timeout /t 3
