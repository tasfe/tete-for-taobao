echo 请按任意键开始安装客户管理平台的后台服务. . .
echo.
pause
echo.
net stop KbaoService
echo 清理原有服务项. . .
%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\installutil /U QiJiaService.exe >>InstallService.log
echo.
echo 清理完毕，开始安装后台服务. . .
echo.

echo.
echo 操作结束，请在 InstallService.log 中查看具体的操作结果。
echo.
pause
