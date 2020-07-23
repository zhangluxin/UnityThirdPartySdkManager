using System.IO;
using UnityEngine;
using UnityThirdPartySdkManager.Editor.Configs;

namespace UnityThirdPartySdkManager.Editor.Generator
{
    /// <summary>
    /// ios生成器
    /// </summary>
    public class IosGenerator
    {
        /// <summary>
        /// 配置
        /// </summary>
        private Config _config;

        /// <summary>
        /// 项目路径
        /// </summary>
        private string _pathToBuiltProject;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="pathToBuiltProject">项目路径</param>
        /// <param name="config">配置</param>
        public IosGenerator(string pathToBuiltProject, Config config)
        {
            _config = config;
            _pathToBuiltProject = pathToBuiltProject;
        }

        /// <summary>
        /// 生成
        /// </summary>
        public void Run()
        {
            if (!Directory.Exists(_pathToBuiltProject))
            {
                Debug.LogError("项目路径不存在");
                return;
            }
            
        }
    }
}