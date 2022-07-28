using Cake.Common.Tools.OctopusDeploy;
var target = Argument("target", "OctoPush");
var solutionFolder = ".././";
var outFolder = "./out/";
var assemblyFile = ParseAssemblyInfo("./My Project/AssemblyInfo.vb");
var title = assemblyFile.Title;
var readedVersion = assemblyFile.AssemblyFileVersion;
var currentVersion = new Version(readedVersion);
var env = Argument("configuration", "Release");

Task("Clean")
    .Does(() =>
{
    CleanDirectory(outFolder);
    Console.WriteLine(title);
    Console.WriteLine(readedVersion);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => {
        NuGetRestore(solutionFolder);
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        MSBuild(solutionFolder, new MSBuildSettings
        {
            ToolVersion = MSBuildToolVersion.VS2019,
            Configuration = env,                                                 
        });            
    });

Task("OctoPack")
    .IsDependentOn("Build")
    .Does(() => {
        var files = new List<string>(){"*.dll", "Web.config", "*.asmx", "*.xml", "*.aspx", 
            "bin***.dll", "bin***.config", "bin***.pdb", "App_Data***.*", "Code/bin***.dll", "Tables/BL***.*", 
            "Tables/Decoding***.xml", "Tables/Users***.*"};
       OctoPack(title, new OctopusPackSettings
       {
                Title = title,  
                BasePath = $"./",
                OutFolder = outFolder,
                Overwrite = true,      
                Version = readedVersion,
                Description = assemblyFile.Description,                
                Author = "Technology Transfer Corporation",                
                Include = files,
             });
        });

Task("OctoPush")
    //.IsDependentOn("OctoPack")
    .Does(() => {
            OctoPush("http://georgia/octopus/", 
            "API-XMPHEHGJH9HBXKAWDNS7RQRAXRJBVLDL", 
            new FilePath($"{outFolder}{title}.{readedVersion}.nupkg"), 
            new OctopusPushSettings {  
                  ReplaceExisting = true,                  
            });
        });

RunTarget(target);

