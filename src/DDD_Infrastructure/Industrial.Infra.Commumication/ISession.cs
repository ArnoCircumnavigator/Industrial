using System;
using System.Collections.Generic;
using System.Text;

namespace Industrial.Infra.Commumication
{
    /// <summary>
    /// 会话
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        /// <returns></returns>
        Guid GetGuid();
        /// <summary>
        /// 开始时间
        /// </summary>
        /// <returns></returns>
        DateTime GetStartTime();
        /// <summary>
        /// 上次活跃时间
        /// </summary>
        /// <returns></returns>
        DateTime GetLastActive();
    }
}
