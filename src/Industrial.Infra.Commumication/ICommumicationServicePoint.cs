using System;
using System.Collections.Generic;
using System.Text;

namespace Industrial.Infra.Commumication
{
    /// <summary>
    /// 通讯模块中的服务端
    /// </summary>
    public interface ICommumicationServicePoint : IDisposable
    {
        /// <summary>
        /// 开机
        /// </summary>
        void Start();

        /// <summary>
        /// 关机
        /// </summary>
        void Stop();

        /// <summary>
        /// 是否可用
        /// </summary>
        /// <returns></returns>
        bool IsValid();
        /// <summary>
        /// 获取服务状态
        /// </summary>
        /// <returns></returns>
        ServiceStatus GetServiceStatus();
    }
    /// <summary>
    /// 连接状态
    /// </summary>
    public enum ServiceStatus
    {
        /// <summary>
        /// 新建
        /// </summary>
        New,
        /// <summary>
        /// 启动中
        /// </summary>
        Starting,
        /// <summary>
        /// 运行...
        /// </summary>
        Runing,
        /// <summary>
        /// 关闭中
        /// </summary>
        Stoping,
        /// <summary>
        /// 已关闭
        /// </summary>
        Stoped,
        /// <summary>
        /// 释放中
        /// </summary>
        Disposing,
        /// <summary>
        /// 已释放
        /// </summary>
        Disposed
    }
}
