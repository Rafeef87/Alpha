@echo off
echo --------------------------------------
echo 🔐 Rensar secrets från Git-historik
echo --------------------------------------

:: Kolla om git-filter-repo finns
where git-filter-repo >nul 2>&1
IF %ERRORLEVEL% NEQ 0 (
    echo ❗ git-filter-repo saknas. Installerar via choco...
    choco install git-filter-repo -y
)

:: Kör filter-repo för att ta bort känslig fil
git filter-repo --path WebApp/appsettings.json --invert-paths

:: Återskapa tom appsettings.json
echo { > WebApp\appsettings.json
echo   "Logging": {}, >> WebApp\appsettings.json
echo   "AllowedHosts": "*" >> WebApp\appsettings.json
echo } >> WebApp\appsettings.json

:: Lägg till och committa
git add WebApp/appsettings.json
git commit -m "Add safe appsettings.json without secrets"

:: Force-push till origin/master
git push origin master --force

echo --------------------------------------
echo ✅ Klar! Secrets är borttagna och pushad.
echo --------------------------------------
pause
