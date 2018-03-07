#load "scriptversions_util.csx"

using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Minimatch;
using Newtonsoft.Json;
using W8lessLabs.ScriptVersions;

void PrintUsage() {
    Console.WriteLine(@"Expected usage: scriptcs scriptversions.csx -- ""<output-file.json>"" ""<file-group:file-glob>"" [...additional file groups]
        Example:
        scriptcs scriptversions.csx -- ""..\webroot\scriptversions.json"" ""js:..\webroot\scripts\*.min.js"" ""css:..\webroot\css\*.min.css""");
}

if(Env.ScriptArgs.Count > 1) 
{
    bool scriptVersionsUpdated = false;
    string scriptVersionsJsonFile = Path.GetFullPath(Env.ScriptArgs[0]);

    SHA1 sha1 = SHA1.Create();
    
    ScriptVersionsFilePersist persist = new ScriptVersionsFilePersist(scriptVersionsJsonFile);
    ScriptVersionsFile fileVersions = persist.Load();
    if(fileVersions == null)
        fileVersions = new ScriptVersionsFile();

    StringBuilder hashString = new StringBuilder();

    for(int i = 1; i < Env.ScriptArgs.Count; i++) {
        string arg = Env.ScriptArgs[i];
        string[] argParts = arg.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
        if(argParts.Length == 3) {
            string fileGroup = argParts[0];
            string rootFolder = argParts[1];
            string pattern = argParts[2];

            var matchedFiles = MatchFiles(rootFolder, pattern);
            FileVersion[] newFileHashes = new FileVersion[matchedFiles.Length];

            for(int j = 0; j < matchedFiles.Length; j++)
            {
                var matchedFile = matchedFiles[j];
                hashString.Clear();
                newFileHashes[j] = new FileVersion(matchedFile.Name, HashFile(sha1, matchedFile, hashString), matchedFile.Directory.Name, 1); 
            }
            
            if(fileVersions.SetVersions(fileGroup, newFileHashes))
                scriptVersionsUpdated = true;

        } else {
            PrintUsage();
            break;
        }
    }

    if(scriptVersionsUpdated) {
        persist.Save(fileVersions);
        //fileVersions.LastUpdated = DateTimeOffset.Now;
        //File.WriteAllText(scriptVersionsJsonFile, JsonConvert.SerializeObject(fileVersions, Newtonsoft.Json.Formatting.Indented));
    }

} else {
    PrintUsage();
}