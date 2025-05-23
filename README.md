# iidentifiiblog

Sql Migrations command:

//AuthDbContext
dotnet ef migrations add AuthModelUpdate --context AuthDbContext --project IIdentifii.Blog.Repository --startup-project IIdentifii.Blog
dotnet ef database update --context AuthDbContext --project IIdentifii.Blog.Repository --startup-project IIdentifii.Blog

//BlogDbContext
dotnet ef migrations add BlogModelsUpdate --context BlogDbContext --project IIdentifii.Blog.Repository --startup-project IIdentifii.Blog
dotnet ef database update --context BlogDbContext --project IIdentifii.Blog.Repository --startup-project IIdentifii.Blog