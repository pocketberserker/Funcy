$BaseFolder = Split-Path -Parent -Path $PSScriptRoot
Start-Process $(Join-Path $BaseFolder \packages\Persimmon.Console.1.0.0-beta6\tools\Persimmon.Console.exe) $(Join-Path $BaseFolder \Funcy.Test\bin\Debug\Funcy.Test.dll) -NoNewWindow -Wait
