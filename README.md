# FileUpload

.Net Core Web Application, that also works as a Desktop Application.

It is an application that allows a user to upload the files and it stores the files on Windows

It also provides the list of all the uploaded documents and provide the functionality to download the documents.

I have used Electron Framework that allows the development of Desktop application using Web Technologies


**Requirements for this App:**

**.NET Core 3.0, 
Visual Studio 2019, 
Node.js,
npm,
Electron CLI**



This application uses Local DB (SQL), you can create the local db using visual Studio. There are two options to create the database in your LocalDB.

## Create Database using .mdf File using Visual Studio Tools

There is Database file **FileUploadDB.mdf** which is stored under \_Database folder to which you can connect using Visual Studio.

In the Visual Studio 2019, 
- Go to "Tools" menu and then click on "Connect to Database". 
- The "Add Connection" window opens, Change the Data Source to "Microsoft SQL Server Database File", 
- For Database File Name, then browse it in the project folder, and under \_Database folder, 
- select "FileUploadDB.mdf"
- Use Windows Authentication.
- You can click on Test Connection to check if Connection is proper or not, once done click OK.
- The database connection will now show in the "Server Explorer" tab. If you can’t see it, go to "View" menu and click "Server Explorer".
- It will also show up in SQL Server Explorer (View -> SQL Server Explorer)


## Or Create new Database using Package Manager Console:

Using Code-First Approach of Entity Framework, you can create the database in LocalDB.

To create the database, Open the **Package Manager Console** (View -> Other Windows -> Package Manager Console) in Visual Studio and run these commands:

> Add-Migration InitialDBCreation 

Here InitialDBCreation is the name for the migration, which you can change and give your suitable name.

After the build succeeded, then, Run the following command to create the database and tables.

> Update-Database

As soon as the build is succeeded, it will create the database in the LocalDB. To check, Open SQL Server Object Explorer( View -> SQL Server Object Explorer), under (localdb)\MSSQLLocalDB, expand Databases and you can see the Database named **FileUploadDB_Dev** and if you expand it, you can see two tables : dbo.Users and dbo.FileReferences.

Your Database is created, build the solution and run it and make sure you use **Google Chrome** to run this Web application.



