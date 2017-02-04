# ASP_Core_Training
Training course building a Web App with ASP.NET Core, MVC 6, EF Core, and Angular

---------------------------------------------------------
Initialise Local Database:

```javascript
dotnet ef migrations add InitialDatabase
```

Updating the Database Migration:

```javascript
dotet ef database update
```
Minify js files:
```javascript
gulp minify
```

Change Environment Variable:
```javascript
set ASPNETCORE-ENVIRONMENT=Testing
```

Publish project:
```javascript
dotnet publish -o <YOURDIRECTORY> 
```
Publish project w/ runtime included:
```javascript
dotnet publish -o <YOURDIRECTORY> -r <runtime>

eg,

dotnet publish -o <YOURDIRECTORY> -r win81-64
```

Build project DLL:
```javascript
dotnet BigTree
```


-----------------------------
API Endpoints (localhost dependent):

[Trips](http://localhost:57916/api/trips)

--------------------------------



Bing API Key:

Key must be obtained from https://www.bingmapsportal.com


---------------------------------
