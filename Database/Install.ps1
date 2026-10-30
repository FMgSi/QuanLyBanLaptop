param(
    [string]$Server = '.\SQLEXPRESS',
    [Parameter(Mandatory=$true)][ValidatePattern('^[A-Za-z][A-Za-z0-9_]{0,127}$')][string]$Database,
    [switch]$Demo
)
$ErrorActionPreference = 'Stop'
if ($Database -in @('master','model','msdb','tempdb')) { throw 'Choose an application database, not a system database.' }
Add-Type -AssemblyName System.Data
$settings = New-Object System.Data.SqlClient.SqlConnectionStringBuilder
$settings['Data Source']=$Server
$settings['Initial Catalog']='master'
$settings['Integrated Security']=$true
$settings['TrustServerCertificate']=$true
$connection = New-Object System.Data.SqlClient.SqlConnection $settings.ConnectionString
function Invoke-Batch([string]$sql) {
    $command=$connection.CreateCommand()
    $command.CommandText=$sql
    $command.CommandTimeout=120
    try { return $command.ExecuteNonQuery() } finally { $command.Dispose() }
}
function Invoke-Script([string]$file) {
    $text=Get-Content (Join-Path $PSScriptRoot $file) -Raw -Encoding UTF8
    foreach ($batch in [regex]::Split($text,'(?im)^\s*GO\s*\r?$')) {
        if ($batch.Trim()) { [void](Invoke-Batch $batch) }
    }
    Write-Output "Applied $file"
}
try {
    $connection.Open()
    [void](Invoke-Batch "IF DB_ID(N'$Database') IS NULL CREATE DATABASE [$Database];")
    $connection.ChangeDatabase($Database)
    [void](Invoke-Batch "IF EXISTS(SELECT 1 FROM sys.objects WHERE is_ms_shipped=0) THROW 52000,N'Database is not empty. Installation stopped; existing data is unchanged.',1;")
    [void](Invoke-Batch 'BEGIN TRANSACTION;')
    try {
        Invoke-Script '01_structure.sql'
        Invoke-Script '02_operations.sql'
        [void](Invoke-Batch 'COMMIT;')
        if ($Demo) { Invoke-Script '03_demo.sql' }
    }
    catch {
        [void](Invoke-Batch 'IF @@TRANCOUNT>0 ROLLBACK;')
        throw
    }
    Write-Output "Installed $Database. Set the database name in QuanLyBanLaptop/App.config."
}
finally { $connection.Dispose() }
