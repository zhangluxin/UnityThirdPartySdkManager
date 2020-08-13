using UnityThirdPartySdkManager.Editor.Configs;

namespace UnityThirdPartySdkManager.Editor.Generators
{
    /// <summary>
    /// 生成器基类
    /// </summary>
    public abstract class Generator
    {
        /// <summary>
        ///     配置
        /// </summary>
        protected readonly Config Config;

        /// <summary>
        ///     项目路径
        /// </summary>
        protected readonly string PathToBuiltProject;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="pathToBuiltProject"></param>
        /// <param name="config"></param>
        protected Generator(string pathToBuiltProject, Config config)
        {
            Config = config;
            PathToBuiltProject = pathToBuiltProject;
        }

        /// <summary>
        ///     生成
        /// </summary>
        public virtual void Run()
        {
        }
    }
}