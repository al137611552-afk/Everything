$ErrorActionPreference = 'Stop'
$framework = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319'
$root = Split-Path $PSScriptRoot -Parent
$source = Join-Path $root 'src'
$destination = Join-Path $root 'outputs\QuickPanel-v0.3.3'
New-Item -ItemType Directory -Force -Path $destination | Out-Null
$refs = @('System.dll','System.Core.dll','System.Web.Extensions.dll','System.Security.dll','System.Net.Http.dll','System.Drawing.dll','System.Windows.Forms.dll','System.Xaml.dll','WPF\WindowsBase.dll','WPF\PresentationCore.dll','WPF\PresentationFramework.dll') | ForEach-Object { '/reference:' + (Join-Path $framework $_) }
$sources = Get-ChildItem -LiteralPath $source -Filter '*.cs' | Select-Object -ExpandProperty FullName
& (Join-Path $framework 'csc.exe') /nologo /target:winexe /platform:x64 /optimize+ "/win32manifest:$source\app.manifest" "/out:$destination\QuickPanel.exe" $refs $sources
if ($LASTEXITCODE -ne 0) { throw 'Compilation failed' }
Copy-Item -LiteralPath (Join-Path $source 'Main.xaml') -Destination $destination -Force
