# s3868494-s3782490-a2
Oliver Rowarth - s3868494
James Ogilvie - s3782490

# GitHub
https://github.com/rmit-wdt-fs-2023/s3868494-s3782490-a2

# Trello
https://trello.com/invite/b/pjcS7Zgs/ATTIe6a5a5245e46ca891b5f5421f24ac83c9647D1E8/wdt-assignment-2


## Project Structure
This project consists of three web projects, one class library, and one xUnit project.

### A note on database migrations / updates
An important note to make before getting started is how migrations are performed considering that models and DbContext is shared though a common class library.

In this project all migration and database update operations occur through the customer app, namely, the **MCBAWebApp**, as indicated by this piece of code
`builder => builder.MigrationsAssembly("MCBAWebApp"));` in its **Program.cs file**. Any attempt to perform these operations within other projects will likely fail and thow various errors.

### MCBADataLibrary
This project contains EF Core models, and a DbContext for accessing said models. These models should meet all requirements listed in the specification. This project also contains a few DTO's which the Admin API and the Admin Site use to communicate with one another.

### MCBAWebApp ASP.NET Core MVC application
This project encompasses the entire customer facing website, including data access, controllers, and views.

For this part of the assignment we attempted all relevant sections in the rubric (apart from full unit testing). This includes


- Seeding the database
- ATM / Transfer features
- Statements
- Profile editing (including passwords)
- BillPay / Scheduled Payments
- Image handling for customer profile pictures, including storage

For images, we decided to utilise the composition design patern, and have images stored in a seperate model. This allows for faster loading of customers when the image is not needed. 

### MCBAAdminAPI ASP.NET Core WebAPI application
This project encompasses the admin API logic including data access through the repository pattern, and API controller endpoints.

For this part of the assignment we attempted all relevant sections in the rubric (except any unit testing). This includes backend logic for:

- Customer profile modification (excluding images)
- Customer blocking / unblocking
- BillPay blocking / unblocking

Finally, all documentation for this API is recorded though method comments within the code instead of here in the README.

### MCBAAdminSite ASP.NET Core MVC application
This project encompasses the admin UI through the use of MVC.

For this part of the assignment we attempted all relevant sections in the rubric (apart from any unit testing). This includes frontend logic for:

- Customer profile modification (excluding images)
- Customer blocking / unblocking
- BillPay blocking / unblocking

It also includes a HttpClientFactory for consuming the Admin API enpoints which performs the necessary backend operations.

### MCBAWebApp.Tests xUnit Tests
This project contains tests covering the customer MVC website.
It covers ATM / Transfer features, profile features, statement features, and login features. 

We would have liked to test more extensively and to have also tested the other projects but by the end we had run out of steam and found many of the issues that blocked our way took too much time to solve, such as complex object mocking and mocking objects which use extension methods, also how model validation may not explicitly be triggered sometimes (see ProfileControllerTests line 156 and try with and without manual model validation)