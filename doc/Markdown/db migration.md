edit connection string 
![image](https://github.com/user-attachments/assets/2c20e829-3f55-48af-935c-797a12de0ff0)

```
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=kokshengbi;User Id=SA;Password=Password123;TrustServerCertificate=True;"
  },
```


Add DesignTimeDbContextFactory
![image](https://github.com/user-attachments/assets/20d494d8-cb8a-41c1-a3a8-fce9891954cb)


Add into DataContext
![image](https://github.com/user-attachments/assets/3b704284-b1fc-44ed-acbf-c722cdc88bae)

```
  // Design-time constructor for migrations
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        // You can set _publishDomainEventsInterceptor to null or ignore it here.
    }
```


```
dotnet ef migrations add InitialCreate
```
```
dotnet ef database update
```

run on kokshengbi.Infrastructure
![image](https://github.com/user-attachments/assets/9ec66b48-42d8-4d0f-a5e0-9d351016d9f8)

Migration Folder

![image](https://github.com/user-attachments/assets/e502b9dc-db5f-4360-a86b-db6d3abc81f4)


Azure Data Studio
![image](https://github.com/user-attachments/assets/33350d02-93e6-49b9-8eab-281a23c298cd)
