# NotificationSchedulingSystem

This solution consists from 3 projects: API, Business Layer and Data Layer. 
There is one project for tests, I did not make separate testprojects for every project because the projects are small, I just put all in folders.

The application has possibility to add a company object to a database. The system checks during creation of an added company if is in conditions to add notifications with provided periodicity to it. 
Periodicity is specified in appsettings.

Also, the application can fetch data from the database and show a response object with only allowed properties. 