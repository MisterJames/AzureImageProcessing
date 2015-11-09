Add-AzureAccount

Login-AzureAccount

$azureAdApplication = New-AzureADApplication -DisplayName "imagenomnomApp" -HomePage "https://imagenomnom.com" -IdentifierUris "https://imagenomnom.com" -Password "Bananas0F_fun"

New-AzureADServicePrincipal -ApplicationId $azureAdApplication.ApplicationId

Write-Host $azureAdApplication.ApplicationId
# 2bd3f9fe-2861-437b-8689-2804b525b3c5/Bananas0F_fun

New-AzureRoleAssignment -RoleDefinitionName Reader -ServicePrincipalName $azureAdApplication.ApplicationId

$subscription = Get-AzureSubscription

$creds = Get-Credential

Add-AzureAccount -Credential $creds -ServicePrincipal -Tenant $subscription.TenantId


