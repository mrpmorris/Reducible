cls
@echo **** 0.6.0 : UPDATED THE VERSION NUMBER IN THE PROJECT *AND* BATCH FILE? ****
pause

cls
@call Test.bat

@echo
@echo
@echo
@echo ======================

set /p ShouldPublish=Publish 0.6.0 [yes]?
@if "%ShouldPublish%" == "yes" (
	@echo PUBLISHING
	dotnet nuget push .\Source\Morris.Reducible\bin\Release\Reducible.0.6.0.nupkg -k %MORRIS.NUGET.KEY% -s https://api.nuget.org/v3/index.json
)

