# thinkbridge_surjitsinghtuteja
Web API Assignment source code

Crud operation using .Net 5.0 core Web API with swagger to Test it easily.

Kindly follow the steps to make it up and running:
1) Create one Database in SQL Server
2) Replace current connection string with newly created database connection string in appsettings.json file
3) Run Script awailable in DBScript folder to create tables and Stored procedures.


/api/Items - GET
without parameters it will return all items considering default values
-----------------
Parameters detail
-----------------
Keyword (optional)
PageNo (optional) default = 1
PageSize (optional) default = 10
SortField (optional) default = 'Name' (Name/Price)
SortExp (optional)  default = 'asc' (asc/desc)
-----------------------------------------------------------

/api/Items - POST
Image can be uploaded (optional) now, right now I am storing image in web api local resource folder.
-----------------------------------------------------------

/api/Items - PUT
Image can be uploaded (optional) now, if any upload is there, old one will be deleted from web api local resource folder.
-----------------------------------------------------------

/api/Items/{id} - GET
get item details by id
-----------------------------------------------------------
/api/Items/{id} - DELETE
delete item with uploaded picture if any

