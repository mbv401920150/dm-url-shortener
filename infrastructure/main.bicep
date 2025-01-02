﻿param location string = resourceGroup().location
var uniqueId = uniqueString(resourceGroup().id)

module keyVault 'modules/secrets/keyvalut.bicep' = {
    name: 'keyVaultDevelopment'
    params: {
        vaultName: 'kv-${uniqueId}'
        location: location
    }
}

module apiService 'modules/compute/appservice.bicep' = {
    name: 'apiDeployment'
    params: {
        appName: 'api-${uniqueId}'
        appServicePlanName: 'plan-api-${uniqueId}'
        location: location
    }
}
