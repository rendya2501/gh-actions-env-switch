using System;
using System.IO;

var exportedCount = new Random().Next(0, 2); // 0 または 1 をランダムに出力

// NULLチェック
var githubEnvPath = Environment.GetEnvironmentVariable("GITHUB_ENV");
if (githubEnvPath == null)
{
    Console.WriteLine("Environment.GetEnvironmentVariable(GITHUB_ENV) is null !!");
}
Console.WriteLine(githubEnvPath);

// GITHUB_ENV に値を書き込む
var writeLineExportedCount = $"EXPORTED_COUNT={exportedCount}";
using (var writer = new StreamWriter(githubEnvPath, true))
{
    writer.WriteLine(writeLineExportedCount);
}
Console.WriteLine(writeLineExportedCount);

// Console.WriteLine($"echo \"HOGE_FUGA={exportedCount}\" >> $GITHUB_ENV");
Console.WriteLine($"\"HOGE_FUGA={exportedCount}\" >> $GITHUB_ENV");
 
