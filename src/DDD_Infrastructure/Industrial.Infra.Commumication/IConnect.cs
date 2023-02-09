using System;

namespace Industrial.Infra.Commumication
{
    /// <summary>
    /// 连接
    /// </summary>
    public interface IConnect : IDisposable
    {
        /// <summary>
        /// 开启连接
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭连接
        /// </summary>
        void Close();

        /// <summary>
        /// 是否可用
        /// </summary>
        /// <returns></returns>
        bool IsValid();

        /// <summary>
        /// 连接状态
        /// </summary>
        /// <returns></returns>
        ConnectStatus GetStatus();

        /// <summary>
        /// 获得当前连接的会话信息
        /// 是可用的，由于用的NET Standerd 2.0 , C# 7 不支持可空值类型，所以后面注意
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        bool TryGetSession(out ISession session);
    }

    /// <summary>
    /// 连接状态
    /// </summary>
    public enum ConnectStatus
    {
        /// <summary>
        /// 新建
        /// </summary>
        New,
        /// <summary>
        /// 连接中
        /// </summary>
        Connecting,
        /// <summary>
        /// 已连接
        /// </summary>
        Connected,
        /// <summary>
        /// 关闭中
        /// </summary>
        Closing,
        /// <summary>
        /// 已断开
        /// </summary>
        Closed,
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
