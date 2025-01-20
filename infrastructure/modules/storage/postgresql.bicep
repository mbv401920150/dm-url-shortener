param name string
param location string

// This parameter will be secure, means the password will not be visible in the logs or will not be storage in any other place.
@secure()
param administratorLoginPassword string
param administratorLogin string

param keyVaultName string

var databaseName = 'ranges'

resource postgresqlServer 'Microsoft.DBforPostgreSQL/flexibleServers@2024-08-01' = {
  name: name
  location: location
  sku: {
    name: 'Standard_B1ms'
    tier: 'Burstable'
  }
  properties: {
    version: '16'
    storage: {
      storageSizeGB: 32
    }
    backup: {
      backupRetentionDays: 7
      geoRedundantBackup: 'Disabled'
    }
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
  }
  resource database 'databases' = {
    name: databaseName
  }
  resource firewallAzure 'firewallRules' = {
    name: 'allow-all-azure-internal-IPs'
    properties: {
      startIpAddress: '0.0.0.0'
      endIpAddress: '0.0.0.0'
    }
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

resource cosmosDbConnectionString 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: 'Postgre--ConnectionString'
  properties: {
    value: 'Server=${postgresqlServer.name}.postgre.database.azure.com;Database=${databaseName};Port=5432;User Id=${administratorLogin};Password=${administratorLoginPassword}'
  }
}

output serverId string = postgresqlServer.id
