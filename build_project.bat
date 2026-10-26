@echo off
set "http_proxy="
set "https_proxy="
set "all_proxy="
"C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" QuanLyBanLaptop\QuanLyBanLaptop.csproj -t:Build -p:Configuration=Debug -p:UseSharedCompilation=false
