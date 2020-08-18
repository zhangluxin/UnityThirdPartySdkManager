using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityThirdPartySdkManager.Editor.Configs;

namespace UnityThirdPartySdkManager.Editor.Generators
{
    /// <summary>
    ///     android生成器
    /// </summary>
    public class AndroidGenerator : Generator
    {
        public AndroidGenerator(string pathToBuiltProject, Config config) : base(pathToBuiltProject, config)
        {
        }


        public override void Run()
        {
            // ModifyGradle();
            // ModifyFiles();
        }

        private void ModifyGradle()
        {
            var gradleFile = Path.Combine(pathToBuiltProject, "unityLibrary", "build.gradle");
            if (!File.Exists(gradleFile)) Debug.LogError("无法找到gradle文件，请检查生成设置!");
            var lines = File.ReadLines(gradleFile);
            var contents = lines as string[] ?? lines.ToArray();
            try
            {
                for (var i = 0; i < contents.Length; i++)
                {
                    if (!contents[i].Contains("dependencies") || i + 2 >= contents.Length - 1) continue;
                    var newContent = new StringBuilder();
                    newContent.Append(string.Join("\n", GenerateDependList()));
                    newContent.Append("\n");
                    newContent.Append(contents[i + 2]);
                    contents[i + 2] = newContent.ToString();
                    break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            File.WriteAllLines(gradleFile, contents);
        }

        /// <summary>
        /// 生成依赖列表
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GenerateDependList()
        {
            var list = new List<string>();
            if (config.weChat.enable)
            {
                list.Add($"implementation '{config.weChat.depend}'");
            }

            return list;
        }

        /// <summary>
        /// 改变文件
        /// </summary>
        private void ModifyFiles()
        {
            
        }
    }
}