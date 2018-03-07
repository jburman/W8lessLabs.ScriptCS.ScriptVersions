using System.Security.Cryptography;

public string HashFile(SHA1 sha1, FileInfo file, StringBuilder sb) 
{
    byte[] hashBytes = null;

    using(var fileStream = file.OpenRead()) 
        hashBytes = sha1.ComputeHash(fileStream);

    for(int i = 0; i < hashBytes.Length; i++) 
            sb.AppendFormat("{0:X2}", hashBytes[i]);
    
    return sb.ToString();
}

public FileInfo[] MatchFiles(string rootFolder, string pattern) 
{
    var dir = new DirectoryInfo(rootFolder);
    var matcher = new Minimatcher(pattern);
    var files = dir.GetFiles("*", SearchOption.AllDirectories);
    return files.Where(f => matcher.IsMatch(f.Name)).ToArray();
}