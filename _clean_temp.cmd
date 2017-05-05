forfiles.exe /p . /s /m *.csproj /c "cmd /c del /q /f @file"
forfiles.exe /p . /s /m *.unityproj /c "cmd /c del /q /f @file"
forfiles.exe /p . /s /m *.sln /c "cmd /c del /q /f @file"
forfiles.exe /p . /s /m *.userprefs /c "cmd /c del /q /f @file"
forfiles.exe /p . /s /m *.pidb /c "cmd /c del /q /f @file"
forfiles.exe /p . /s /m Thumbs.db /c "cmd /c del /q /f @file"
forfiles.exe /p . /s /m ehthumbs.db /c "cmd /c del /q /f @file"
forfiles.exe /p . /s /m Desktop.ini /c "cmd /c del /q /f @file"
forfiles.exe /p . /s /m .DS_Store /c "cmd /c del /q /f @file"
forfiles.exe /p . /s /m *.bak /c "cmd /c del /q /f @file"
pause



