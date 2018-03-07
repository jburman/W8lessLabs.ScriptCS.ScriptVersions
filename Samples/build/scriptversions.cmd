SET webpath="..\..\..\ClassicMVC"

CD ..\packages\W8lessLabs.ScriptCS.ScriptVersions.1.0.0\tools
scriptcs scriptversions.csx -- "%webpath%\scriptversions.json" "js:%webpath%\scripts:*.min.js" "css:%webpath%\css:*.min.css"
