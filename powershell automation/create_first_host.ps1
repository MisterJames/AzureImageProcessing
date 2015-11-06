# Switch-AzureMode -Name AzureResourceManager
# Add-AzureAccount
# Select-AzureSubscription -SubscriptionName "Free Trial"

# get the zone, two required params
$rootzone = Get-AzureDnsZone -Name imagenomnom.com -ResourceGroupName AzureImageProcessing

# create the subdomain
#$rs = New-AzureDnsRecordSet -Name www -Zone $rootzone -RecordType A -Ttl 300

# get a recordset from scratch
$rootzone = Get-AzureDnsZone -Name imagenomnom.com -ResourceGroupName AzureImageProcessing
$rs = New-AzureDnsRecordSet –Name james –RecordType A -Zone $rootzone -Ttl 60 | 
    Add-AzureDnsRecordConfig -Ipv4Address "40.112.142.148" |
    Set-AzureDnsRecordSet

# add the CNAME
$zone = Get-AzureDnsZone -Name "imagenomnom.com" -ResourceGroupName AzureImageProcessing
$rs = New-AzureDnsRecordSet -Name "awverify.james" -RecordType CNAME -Zone $zone -Ttl 60 | 
    Add-AzureDnsRecordConfig -Cname "awverify.imagenomnom.azurewebsites.net" | 
    Set-AzureDnsRecordSet

    
# add the CNAME
$zone = Get-AzureDnsZone -Name "imagenomnom.com" -ResourceGroupName AzureImageProcessing
$rs = New-AzureDnsRecordSet -Name "*" -RecordType CNAME -Zone $zone -Ttl 60 | 
    Add-AzureDnsRecordConfig -Cname "imagenomnom.azurewebsites.net" | 
    Set-AzureDnsRecordSet

# create a CNAME resource record with your DNS provider that points from either 
#     www.yourdomain.com to imagenomnom.azurewebsites.net
#     awverify.www.yourdomain.com to awverify.imagenomnom.azurewebsites.net
# IP Address - 40.112.142.148


# add the CNAME
$zone = Get-AzureDnsZone -Name "imagenomnom.com" -ResourceGroupName AzureImageProcessing
$rs = New-AzureDnsRecordSet -Name "awverify.www" -RecordType CNAME -Zone $zone -Ttl 60 | 
    Add-AzureDnsRecordConfig -Cname "awverify.imagenomnom.azurewebsites.net" | 
    Set-AzureDnsRecordSet


$rootzone = Get-AzureDnsZone -Name "imagenomnom.com" -ResourceGroupName AzureImageProcessing
Get-AzureDnsRecordSet -Zone $rootzone 



$zone = Get-AzureDnsZone -Name "imagenomnom.com" -ResourceGroupName AzureImageProcessing

New-AzureDnsRecordSet -Name "@" -RecordType "A" -Zone $zone -Ttl 60 | 
    Add-AzureDnsRecordConfig -Ipv4Address 40.112.142.148 |
    Set-AzureDnsRecordSet 

Get-AzureDnsRecordSet -Zone $zone 


Get-Help Set-AzureDnsRecordSet -Examples
