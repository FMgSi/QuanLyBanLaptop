param([string]$Server = '.\SQLEXPRESS')
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio/Installer/vswhere.exe'
$msbuild = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
if (-not $msbuild) { throw 'Install Visual Studio with .NET desktop development and .NET Framework 4.7.2 targeting pack.' }
& $msbuild "$root/QuanLyBanLaptop/QuanLyBanLaptop.csproj" -t:Build -p:Configuration=Debug -p:UseSharedCompilation=false -verbosity:minimal
if ($LASTEXITCODE) { throw 'Build failed.' }
$bin = Join-Path $root 'QuanLyBanLaptop/bin/Debug'
$csc = Join-Path (Split-Path $msbuild) 'Roslyn/csc.exe'
foreach ($test in @('BusinessTests','SmokeTests')) {
    & $csc /nologo /target:exe "/out:$bin/$test.exe" "/reference:$bin/QuanLyBanLaptop.exe" /reference:System.dll /reference:System.Core.dll /reference:System.Data.dll /reference:System.Data.DataSetExtensions.dll /reference:System.Drawing.dll /reference:System.Windows.Forms.dll /reference:System.Windows.Forms.DataVisualization.dll "$PSScriptRoot/$test.cs"
    if ($LASTEXITCODE) { throw "Compile $test failed." }
}
Add-Type -AssemblyName System.Data
$name = 'QuanLyBanLaptop_Test_' + [Guid]::NewGuid().ToString('N')
$builder = New-Object System.Data.SqlClient.SqlConnectionStringBuilder
$builder['Data Source']=$Server; $builder['Initial Catalog']='master'; $builder['Integrated Security']=$true; $builder['TrustServerCertificate']=$true
$cn = New-Object System.Data.SqlClient.SqlConnection $builder.ConnectionString
$cn.Open()
$command=$cn.CreateCommand(); $command.CommandText="CREATE DATABASE [$name]"; [void]$command.ExecuteNonQuery()
try {
    $cn.ChangeDatabase($name)
    foreach ($file in @('01_structure.sql','02_operations.sql','03_demo.sql')) {
        $text=Get-Content "$root/Database/$file" -Raw -Encoding UTF8
        foreach ($batch in [regex]::Split($text,'(?im)^\s*GO\s*\r?$')) {
            if (-not $batch.Trim()) { continue }
            $command=$cn.CreateCommand(); $command.CommandText=$batch; $command.CommandTimeout=120
            try { [void]$command.ExecuteNonQuery() } finally { $command.Dispose() }
        }
        Write-Output "PASS: $file"
    }
    $builder['Initial Catalog']=$name
    $env:LAPTOP_TEST_CONNECTION=$builder.ConnectionString
    foreach ($test in @('BusinessTests','SmokeTests')) {
        & "$bin/$test.exe"
        if ($LASTEXITCODE) { throw "$test failed." }
    }
    $command=$cn.CreateCommand()
    $command.CommandText=[regex]::Replace((Get-Content "$root/Database/04_verify.sql" -Raw -Encoding UTF8),'(?im)^\s*GO\s*\r?$','')
    $table=New-Object System.Data.DataTable
    $adapter=New-Object System.Data.SqlClient.SqlDataAdapter $command
    [void]$adapter.Fill($table)
    if ($table.Rows.Count) { $table | Format-Table; throw 'Reconciliation failed.' }
    Write-Output 'PASS: reconciliation after business operations'
}
finally {
    $cn.Close(); $cn.Dispose()
    [System.Data.SqlClient.SqlConnection]::ClearAllPools()
    $builder['Initial Catalog']='master'
    $cleanup=New-Object System.Data.SqlClient.SqlConnection $builder.ConnectionString
    $cleanup.Open()
    # Only the randomly named database created by this invocation is removed.
    $command=$cleanup.CreateCommand(); $command.CommandText="ALTER DATABASE [$name] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [$name]"
    [void]$command.ExecuteNonQuery(); $cleanup.Dispose()
    Remove-Item Env:LAPTOP_TEST_CONNECTION -ErrorAction SilentlyContinue
}
