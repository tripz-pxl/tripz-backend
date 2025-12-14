param location string = resourceGroup().location
param appName string = 'tripz-backend'
param environment string = 'dev'
param skuName string = 'B1'
param skuCapacity int = 1

var appServicePlanName = '${appName}-plan-${environment}'
var appServiceName = '${appName}-${environment}'

resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: appServicePlanName
  location: location
  kind: 'linux'
  sku: {
    name: skuName
    capacity: skuCapacity
  }
  properties: {
    reserved: true
  }
}

resource appService 'Microsoft.Web/sites@2023-01-01' = {
  name: appServiceName
  location: location
  kind: 'app,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNET|10.0'
      alwaysOn: true
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: environment
        }
        {
          name: 'ASPNETCORE_URLS'
          value: 'http://+:80'
        }
      ]
    }
  }
}

output appServiceUrl string = appService.properties.defaultHostName
output appServiceId string = appService.id
output appServicePlanId string = appServicePlan.id
