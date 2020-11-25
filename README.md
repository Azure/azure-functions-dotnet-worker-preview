# Azure Functions .NET 5 support

## How to run the sample

### Download .NET 5.0
Download .NET 5.0 rc2 [from here](https://dotnet.microsoft.com/download/dotnet/5.0)

### Download the Azure Functions Core Tools
Please make sure you have Azure Functions Core Tools >= `3.0.2996`.

To download using `npm`, you can run  `npm i -g azure-functions-core-tools@3 --unsafe-perm true`.

For other ways to download, please checkout our docs at [Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools)

### Run the sample locally
- Go to the function app directory - `cd FunctionApp`
- Run `func host start --csharp` [Optional `--verbose`]. This will preform a build and then run the host.

### Important files in this sample that you shouldn't remove
- `local.settings.json` - The following two environment variables are required to run it locally. In Azure, these two needs to be set as Application Settings.
```
{
  ...
  "Values": {
    "FUNCTIONS_WORKER_RUNTIME": "dotnet5",
    "languageWorkers:dotnet5:workerDirectory": "./"
  }
  ...
}

```
- `NuGet.Config` - The .NET worker packages are published in a MyGet feed, and the following sources need to be present. --
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://www.nuget.org/api/v2/" />
    <add key="azure_app_service" value="https://www.myget.org/F/azure-appservice/api/v2" />
    ...
  </packageSources>
</configuration>
```

- `FunctionApp.csproj` - This has to have at least the following content --
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
    <LangVersion>preview</LangVersion>
    <AzureFunctionsVersion>v3</AzureFunctionsVersion>
    <OutputType>Exe</OutputType>
    <_FunctionsSkipCleanOutput>true</_FunctionsSkipCleanOutput>
  </PropertyGroup>
  <ItemGroup>
    ...
    <PackageReference Include="Microsoft.Azure.Functions.Worker" Version="1.1.0-preview2" />
    <PackageReference Include="Microsoft.Azure.Functions.Worker.SourceGenerator" Version="1.1.0-preview2" OutputItemType="Analyzer" />
    <PackageReference Include="Microsoft.NET.Sdk.FunctionsWorker" Version="5.0.0-preview1-0002" />>
  </ItemGroup>
  <ItemGroup>
    <None Update="host.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="local.settings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      <CopyToPublishDirectory>Never</CopyToPublishDirectory>
    </None>
  </ItemGroup>
</Project>
```

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
