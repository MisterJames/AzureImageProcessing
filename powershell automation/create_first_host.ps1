
# get the zone, two required params
$rootzone = Get-AzureDnsZone -Name imagenomnom.com -ResourceGroupName AzureImageProcessing

# create the subdomain
$rs = New-AzureDnsRecordSet -Name www -Zone $rootzone -RecordType A -Ttl 300


