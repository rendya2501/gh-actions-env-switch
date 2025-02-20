using System;
using System.IO;

var exportedCount = new Random().Next(0, 2); // 0 または 1 をランダムに出力

// NULLチェック
var githubEnvPath = Environment.GetEnvironmentVariable("GITHUB_ENV");
if (githubEnvPath == null)
{
    throw new Exception("Environment.GetEnvironmentVariable(GITHUB_ENV) is null !!");
}

// GITHUB_ENV に値を書き込む
using (var writer = new StreamWriter(githubEnvPath, true))
{
    writer.WriteLine($"EXPORTED_COUNT={exportedCount}");
}

writer.WriteLine($"HOGE_FUGA={exportedCount} >> $GITHUB_ENV");
