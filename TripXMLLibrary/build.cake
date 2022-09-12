var target = Argument("target", "Push");
var solutionFolder = "./";
var outFolder = "./out/";
var propsFile = "./TripXML.Library.csproj";
var title = XmlPeek(propsFile, "//AssemblyTitle");
var readedVersion = XmlPeek(propsFile, "//Version");
var currentVersion = new Version(readedVersion);
var env = Argument("env", "Release");

Task("Clean")
    .Does(() =>
{
    Information("Cleaning Directories");
    CleanDirectory(outFolder);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => {
        Information("Restoring nuget packages");
        DotNetRestore(solutionFolder);
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        DotNetCoreBuild(solutionFolder, new DotNetCoreBuildSettings
        {
            Configuration = env,
        });
    });

Task("Pack")
    .IsDependentOn("Build")
    .Does(() => {
        Information("Packing project into nuget");
       DotNetPack(solutionFolder, new DotNetPackSettings
       {
           Configuration = env,
           OutputDirectory = outFolder
       });
    });

Task("Push")
    .IsDependentOn("Pack")
    .Does(() => {
        Information("Pushing nuget to server");
            DotNetNuGetPush(outFolder + $"{title}.{currentVersion}.nupkg", new DotNetNuGetPushSettings {
                Source = "https://techtransnuget.azurewebsites.net/v3/index.json",
                WorkingDirectory = solutionFolder,
                ApiKey = "c479d5b4-5f41-463e-a57f-5bab381e73a3"
            });            
        });

RunTarget(target);