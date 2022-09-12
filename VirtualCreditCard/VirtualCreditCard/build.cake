using Cake.Common.Tools.OctopusDeploy;
var target = Argument("target", "OctoPush");
var solutionFolder = "./";
var outFolder = "./out/";

var assemblyInfo = ParseAssemblyInfo("./VirtualCreditCard.csproj");

var propsFile = "./Directory.Build.props";
var title = XmlPeek(propsFile, "//AssemblyTitle");
var readedVersion = XmlPeek(propsFile, "//Version");
var currentVersion = new Version(readedVersion);

Information("Version: {0}", readedVersion);
Information("File version: {0}", currentVersion);
Information("Title: {0}", title);

/********************************************************
Information("Version: {0}", assemblyInfo.AssemblyVersion);
Information("File version: {0}", assemblyInfo.AssemblyFileVersion);
Information("Informational version: {0}", assemblyInfo.AssemblyInformationalVersion);
Information("Title: {0}", assemblyInfo.Title);
var title = assemblyInfo.Title;
var readedVersion = assemblyInfo.AssemblyFileVersion;
var currentVersion = new Version(readedVersion);
*********************************************************/

var env = Argument("configuration", "Debug");

Task("Clean")
    .Does(() =>
{
    CleanDirectory(outFolder);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => {
        DotNetRestore(solutionFolder);
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        DotNetCoreBuild(solutionFolder, new DotNetCoreBuildSettings
        {
            Configuration = env,
            NoRestore = true
        });
    });

Task("Publish")
    .IsDependentOn("Build")
    .Does(() => {
        var publishSettings = new DotNetPublishSettings(){
            Configuration = env,
            ArgumentCustomization = args => args.Append("--no-restore")
        };
        DotNetPublish(solutionFolder, publishSettings);
    });

Task("OctoPack")
    .IsDependentOn("Publish")
    .Does(() => {
       OctoPack(title, new OctopusPackSettings
       {
                Title = title,  
                BasePath = $"./bin/{env}/net6.0/publish/",
                OutFolder = outFolder,
                Overwrite = true,      
                Version = readedVersion
             });
            
        });

Task("OctoPush")
    .IsDependentOn("OctoPack")
    .Does(() => {
            OctoPush("http://georgia/octopus/", 
            "API-XMPHEHGJH9HBXKAWDNS7RQRAXRJBVLDL", 
            new FilePath($"{outFolder}{title}.{readedVersion}.nupkg"), 
            new OctopusPushSettings {  
                  ReplaceExisting = true,                  
            });
        });

RunTarget(target);