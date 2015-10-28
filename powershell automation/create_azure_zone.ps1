
Switch-AzureMode -Name AzureResourceManager
Add-AzureAccount
Select-AzureSubscription -SubscriptionName "Free Trial"

New-AzureResourceGroup -Name AzureImageProcessing -location "West US"
Register-AzureProvider -ProviderNamespace Microsoft.Network


New-AzureDnsZone -Name imagenomnom.com -ResourceGroupName AzureImageProcessing
Get-AzureDnsRecordSet -ZoneName imagenomnom.com -ResourceGroupName AzureImageProcessing
