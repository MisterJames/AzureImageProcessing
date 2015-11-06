

$zone = Get-AzureDnsZone -Name "imagenomnom.com" -ResourceGroupName AzureImageProcessing

Remove-AzureDnsRecordSet -Name "awverify" -RecordType CNAME -Zone $zone -Force