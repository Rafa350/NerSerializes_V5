@echo off
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" Publish.proj /t:Publish /p:msbuildemitsolution=1 /m
pause
