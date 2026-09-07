$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$downloads = Join-Path $root 'work\downloads'
$tools = Join-Path $root 'outputs\QuickPanel-v0.3.3\tools\Everything'
New-Item -ItemType Directory -Force -Path $downloads,$tools | Out-Null
Invoke-WebRequest 'https://www.voidtools.com/ES-1.1.0.37.x64.zip' -OutFile (Join-Path $downloads 'es.zip')
Invoke-WebRequest 'https://www.voidtools.com/Everything-1.4.1.1032.x64.zip' -OutFile (Join-Path $downloads 'everything.zip')
Expand-Archive (Join-Path $downloads 'es.zip') (Join-Path $downloads 'es') -Force
Expand-Archive (Join-Path $downloads 'everything.zip') (Join-Path $downloads 'everything') -Force
Copy-Item (Join-Path $downloads 'es\es.exe') $tools -Force
Copy-Item (Join-Path $downloads 'everything\Everything.exe') $tools -Force
Copy-Item (Join-Path $downloads 'everything\Everything.lng') $tools -Force
Invoke-WebRequest 'https://www.voidtools.com/License.txt' -OutFile (Join-Path $tools 'Everything-LICENSE.txt')
Invoke-WebRequest 'https://raw.githubusercontent.com/voidtools/ES/main/LICENSE' -OutFile (Join-Path $tools 'ES-LICENSE.txt')
