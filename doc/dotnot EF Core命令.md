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

- 