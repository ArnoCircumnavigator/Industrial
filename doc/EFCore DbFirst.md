- 首先把对应的驱动装好，这里以mysql为例
  - ![](./EFCore%20DbFirst.md.assets/p1.jpg)
- 可以存在挡住流程的项目，可以先“卸载”掉，后面再加回来
- 在PM控制台（PMC）中输入如下指令
  - Scaffold-DbContext "Server=mysql-test1.mysql.database.chinacloudapi.cn;User Id=DBA;Password=Noproblem001;Database=TestEFCore" -Provider "Pomelo.EntityFrameworkCore.MySql" -Context EfCoreContext -OutputDir Models -Namespace Industrial.Infra.Database.Models -Force
  - 不同数据库指令有所不同，具体的可以参照工具库的API文档
  - 后面的可配置参数参考官方文档https://learn.microsoft.com/zh-cn/ef/core/cli/powershell?source=recommendations

# 参考文档
- https://www.cnblogs.com/clis/p/16172367.html
- https://learn.microsoft.com/zh-cn/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli