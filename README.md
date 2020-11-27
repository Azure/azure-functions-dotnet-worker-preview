# Azure Functions .NET 5 support

## How to run the sample

### Install .NET 5.0
Download .NET 5.0 [from here](https://dotnet.microsoft.com/download/dotnet/5.0)

### Install the Azure Functions Core Tools
Please make sure you have Azure Functions Core Tools >= `3.0.2996`.

To download using `npm`, you can run  `npm i -g azure-functions-core-tools@3 --unsafe-perm true`.

For other ways to download, please check out our docs at [Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools)

### Add local settings file

In the `FunctionApp` folder, create a file named `local.settings.json` and add the following content.

#### FunctionApp/local.settings.json

```json
{
  "IsEncrypted": false,
  "Values": {
    "FUNCTIONS_WORKER_RUNTIME": "dotnet5",
    "languageWorkers:dotnet5:workerDirectory": ".",
    "AzureWebJobsStorage": ""
  }
}
```

* `FUNCTIONS_WORKER_RUNTIME` - Set this to a value of `dotnet5`. This is likely to change in the future as this worker is intended for future .NET versions as well.
* `languageWorkers:dotnet5:workerDirectory` - Where the functions host will locate the .NET 5 worker. Set this to `.` (this will be the function app root)
* `AzureWebJobsStorage` - Some of the functions in the sample require a Storage account. Set the value of `AzureWebJobsStorage` to the connection string to a valid Storage account or running Storage Emulator.

### FunctionApp folder structure

Here are the important artifacts in a .NET 5 Azure Functions app.

#### NuGet.Config

During the private preview, the .NET worker packages are published in a MyGet feed, and the following sources need to be present in *NuGet.config*.

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://www.nuget.org/api/v2/" />
    <add key="azure_app_service" value="https://www.myget.org/F/azure-appservice/api/v2" />
    ...
  </packageSources>
</configuration>
```

### FunctionApp.csproj

**Important** - Currently, the worker needs to be compiled into a DLL named `FunctionApp.dll`. The simplest way to ensure this happens is to name your project file `FunctionApp.csproj`. This limitation will be removed in a future release.

There are some main differences between a .NET 5 Azure Functions project compared to .NET Core 3.1.

* `TargetFramework` and `OutputType` - A .NET 5 Azure Functions app is a .NET 5 executable (console app) that runs in a process that is separate from the Azure Functions host.
* `AzureFunctionsVersion` - .NET 5 Azure Functions still uses the `v3` Azure Functions host.
* `_FunctionsSkipCleanOutput` - Ensure this is set to prevent the build process from removing important files in the output.

```xml
<PropertyGroup>
  <TargetFramework>net5.0</TargetFramework>
  <LangVersion>preview</LangVersion>
  <AzureFunctionsVersion>v3</AzureFunctionsVersion>
  <OutputType>Exe</OutputType>
  <_FunctionsSkipCleanOutput>true</_FunctionsSkipCleanOutput>
</PropertyGroup>
```

Also note the package references needed for the .NET 5 worker. You can use other .NET 5 compatible packages in your project.

For functions attributes to work, you also need to reference the appropriate WebJobs SDK packages that contain the required types.

### Functions

Like .NET Core 3.1 function apps, functions are in C# files. They are currently separated into folders but, like .NET Core 3.1 functions, they can be organized differently if you wish.

One important difference with .NET 5 functions is that "rich bindings", such as binding to a Cosmos DB client, are not supported. Use strings and C# objects (POCOs). For HTTP, use `HttpRequestData` and `HttpResponseData` objects.

* `Function1` - An HTTP trigger with a Blob input and a Queue output.
* `Function2` - A Queue trigger with a Blob input.
* `Function3` - An HTTP trigger with a Queue output.
* `Function4` - A simple HTTP trigger.
* `Function5` - An HTTP triggered function that demonstrates dependency instruction.

### Run the sample locally

- Go to the function app directory - `cd FunctionApp`
- Run `func host start --csharp` [Optional `--verbose`]. This will preform a build and then run the host.

### Attaching the debugger

We are working with the Visual Studio and VS Code teams to add support for debugging and deployment. For now, follow these instructions to debug an app.

#### VS Code

In the "Run" icon in the Activity Bar. The `.NET Core Attach` launch task should be selected. **With the function app running**, start the `.NET Core Attach` launch configuration. It will prompt you for a process to attach to. Select the `dotnet` process running `FunctionApp.dll`.

#### Visual Studio

To debug in Visual Studio, uncomment the `Debugger.Launch()` statements in *Program.cs*. The process will attempt to launch a debugger before continuing.

## Deploying to Azure

*Coming soon. Please chech back for details.*

<!-- not working right now


### Create the Azure resources

1. To deploy the app, first ensure that you've installed the Azure CLI. 

1. Login to the CLI.

    ```bash
    az login
    ```

1. If necessary, use `az account set` to select the subscription you want to use.
  
1. Create a resource group, Storage account, and Azure Functions app.

    ```bash
    az group create --name AzureFunctionsQuickstart-rg --location westeurope
    az storage account create --name <STORAGE_NAME> --location westeurope --resource-group AzureFunctionsQuickstart-rg --sku Standard_LRS
    az functionapp create --resource-group AzureFunctionsQuickstart-rg --consumption-plan-location westeurope --runtime dotnet --functions-version 3 --name <APP_NAME> --storage-account <STORAGE_NAME>
    ```

1. Currently, the function app needs to first be created as .NET Core 3.1. Change the .NET version to `v5.0`.

    ```bash
  az functionapp config set --net-framework-version "v5.0" --resource-group AzureFunctionsQuickstart-rg --name <APP_NAME>
    ```

### Deploying the app

1. Ensure you're in the `FunctionApp` folder.

1. Deploy the app.

    ```bash
    func azure functionapp publish <APP_NAME> --csharp --force
    ```

1. Set the function app settings.

    ```bash
    az functionapp config appsettings set --settings "FUNCTIONS_WORKER_RUNTIME=dotnet5" "languageWorkers:dotnet5:workerDirectory=." --resource-group AzureFunctionsQuickstart-rg --name <APP_NAME>
    ```


-->

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
