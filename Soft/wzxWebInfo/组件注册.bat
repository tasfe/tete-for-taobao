@echo off

echo ┌────────────────────┐
echo │　IIS 网站流量监控软件附属组件注册向导  │
echo └────────────────────┘

echo .
echo .

 
copy /y .\MSCMCCHS.DLL %SYSTEMROOT%\SYSTEM32\MSCMCCHS.DLL > nul
copy /y .\MSCOMCTL.OCX %SYSTEMROOT%\SYSTEM32\MSCOMCTL.OCX > nul
copy /y .\msvbvm60.dll %SYSTEMROOT%\SYSTEM32\msvbvm60.dll > nul
copy /y .\VB6CHS.DLL %SYSTEMROOT%\SYSTEM32\VB6CHS.DLL > nul

regsvr32 %SYSTEMROOT%\SYSTEM32\MSCOMCTL.OCX /s


echo .
echo .
echo .
echo OCX组件注册完成，请点“wzxWebInfo.exe”启动软件
echo .
echo 如果运行“wzxWebInfo.exe”出现“MSCOMCTL.OCX”错误，或其他错误，请使用“安装文件”里的 setup.exe 进行安装！
echo .
echo .
echo .
pause
exit