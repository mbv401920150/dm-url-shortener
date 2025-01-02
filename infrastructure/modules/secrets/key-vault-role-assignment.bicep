param keyVaultName string
param principalIds array
param principalType string = 'ServicePrincipal'
param roleDefinitionId string = 'e181df14-00eb-4035-819b-3ca585545e2b' // Generated randomly

// At that point it's expected that this keyVault is already existed
resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

// For each entry of the array, it will be added 
resource keyVaultRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for principalId in principalIds: {
  name: guid(keyVault.id, principalId, roleDefinitionId)
  scope: keyVault // Where this role assignment will apply?
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', roleDefinitionId)
    principalId: principalId
    principalType: principalType
  }
}]
