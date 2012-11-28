echo 请按任意键开始安装客户管理平台的后台服务. . .
echo.
pause
echo.

echo ，开始安装后台服务. . .
echo.
%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\installutil  QiJiaService.exe
echo 服务安装完毕，启动服务. . .
net start KbaoService>> InstallService.log
echo.
echo 操作结束，请在 InstallService.log 中查看具体的操作结果。
echo.
pause
