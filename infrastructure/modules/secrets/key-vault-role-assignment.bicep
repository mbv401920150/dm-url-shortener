param keyVaultName string
param principalIds array
param principalType string = 'ServicePrincipal'
// Run this command: 
//   az role definition list --query "[].{RoleName:roleName, Id:id}" -o table
// You will find all roles IDs that can be used.
// This is the one that we need to use: 'Key Vault Secrets User' | 4633458b-17de-408a-b874-0445c86b69e6
param roleDefinitionId string = '4633458b-17de-408a-b874-0445c86b69e6'

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
