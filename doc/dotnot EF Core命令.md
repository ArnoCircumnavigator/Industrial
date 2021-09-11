- 安装EF Core CLI工具

  ```shell
  dotnet tool install --global dotnet-ef
  ```
  
- 创建一个迁移

  - 项目安装对应的包，可以参考Industrial.Database
  - 在当前工程路径下执行

  ```shell
  dotnet ef migrations add [本次迁移的名称]
  ```

- 更新到数据库

  ```shel
  dotnet ef database update
  ```

# 更换数据库

- 在工程中导入对应的Nuget包
- 删除现有Migrations文件（删除之前先用git备份，以防万一）
- 在服务器上创建对应的数据库
- 修改Dbcontext配置
- 创建迁移文件，迁移
