@echo off
setlocal
set HTTP_PROXY=
set http_proxy=
set HTTPS_PROXY=
set https_proxy=

echo ==============================================
echo  Dang bien dich QuanLyBanLaptop...
echo ==============================================

"C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" "%~dp0QuanLyBanLaptop\QuanLyBanLaptop.csproj" /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /nologo

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ==============================================
    echo  BIEN DICH THANH CONG! - 0 Error
    echo  File chay tai: %~dp0QuanLyBanLaptop\bin\Debug\QuanLyBanLaptop.exe
    echo ==============================================
) else (
    echo.
    echo [LOI] Bien dich that bai! Ma loi: %ERRORLEVEL%
)

endlocal
