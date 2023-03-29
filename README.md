# HomesEngland-FindnRegister

## Development

Before starting you will need to add user secrets through dotnet user-secrets:

run the following commands from the Find\&Register project folder:
(replacing the square brackets with appropiate values)

    cd Find\&Register

    dotnet user-secrets set "Analytics:GaTag" [ask the tech team for the values]
    dotnet user-secrets set "ApplicationInsights:ConnectionString" [ask the tech team for the values]
    dotnet user-secrets set "DataSources:Locations:ApiClientName" [ask the tech team for the values]
    dotnet user-secrets set "DataSources:Locations:TokenHeaderKey" [ask the tech team for the values]
    dotnet user-secrets set "DataSources:Locations:ApiUrl" [ask the tech team for the values]
    dotnet user-secrets set "DataSources:Locations:ApiToken" [ask the tech team for the values]
    dotnet user-secrets set "DataSources:Locations:ClientHeaderKey" [ask the tech team for the values]
    dotnet user-secrets set "DataSources:Locations:UseApi" [ask the tech team for the values]
    dotnet user-secrets set "DataSources:Providers" [ask the tech team for the values]

To use sharepoint datasource for the providers, please add the following user secrets:

    dotnet user-secrets set "SharePointGraph:ClientId" [ask the tech team for the values]
    dotnet user-secrets set "SharePointGraph:ClientSecret" [ask the tech team for the values]
    dotnet user-secrets set "SharePointGraph:ProviderSite" [ask the tech team for the values]
    dotnet user-secrets set "SharePointGraph:ProvidersList" [ask the tech team for the values]
    dotnet user-secrets set "SharePointGraph:SharepointHost" [ask the tech team for the values]
    dotnet user-secrets set "SharePointGraph:MicrosoftInstance" [ask the tech team for the values]
    dotnet user-secrets set "SharePointGraph:GraphUrl" [ask the tech team for the values]
    dotnet user-secrets set "SharePointGraph:TenantId" [ask the tech team for the values]

If you wish to run locally with dummy data set:

    dotnet user-secrets set "DataSources:Locations:UseApi" false
    dotnet user-secrets set "DataSources:Locations:LocalFile" "resources/TestLocations.json"

### Coding standard

Find and Register follows Homes England coding standards and must follow Govuk frontend design standards.
    Please see: https://frontend.design-system.service.gov.uk

### Building assembly files

In Find&Register project root directory terminal run:
    dotnet build

### Running Unit Tests

Selenium tests require Chrome or Chrome web driver to be installed.
Please download and follow installation instructions from:
    https://chromedriver.chromium.org/downloads

To run selenium tests, the application must be built and must be running. The application runs on port 7059 by default and this port should be left free.
In Find&Register project root directory terminal run:
    dotnet build
    dotnet run

Then once this is running, in the repository root directory we can run the tests through:
    dotnet test

### Building CI/CD pipeline
Go to to ADO, then pipeline create new pipeline.
Choose github, then connect to github account on the next step. 
then it will ask which code repo to connect too.  

### Selenium Integration Test
Integration test are built off of Xunit. The application is using a chrome web driver so insure that your chrome version is at least Version 111.0.5563.64 or higher.

there are global dependencies in the usings.cs file and this project also has config variables set in the appsettings.json file. currently it just has the one variable BaseUrl. this is set to local host but it can be changed to point to DEV/QA or the UAT url.

To Perform integration test, ensure that an instance of the site is running, this could be local host or dev/qa/uat. have the BeseURL pointing to that site leaving out everything after the / 

once the instace is up andd running, open another instance of visual studio and look for any integration test function. Right click within that function and then click either menu option that appears. run test or debug test.
