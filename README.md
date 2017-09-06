# UofT Timetable Generator

### Description
The Uoft Timetable Generator aims to create the best university timetable for any UofT students. Equipped with the most accurate genetic algorithm, it can create the best timetables based on user's preferences such as setting a maximum time for the latest class, or having the smallest walking distance between classes.

The Uoft Timetable Generator project is a full stack web development project. It is comprised from several components: the Web Scrapper, SQL Database infrustructure, Data Models, Timetable Generator Library, Genetic Algorithm Analyzer, C# Web Api, and a Angular JS Front End Web Application. 

### Table of Contents
- Walkthrough
- Installation
- Usage
- Credits
- License

### Walkthrough of this project



### Installation
##### Required Programs and Tools:
- MS SQL Server on a machine (optional: SQL Server Management Studio)
- Visual Studio

##### Set up the database
- Install MS SQL Server on a machine
- Set up a new database to store the UofT data
- Open up the **UofT Database** project properties in Visual Studio, edit the connection string such that it links to your newly created database
- Select the *Create new database* setting in the Build settings of the **UofT Database** project properties (so that it creates the relational database structure in your new database).
- Run the **UofT Database** project by setting it as a *startup project* and pressing F5.

##### Set up the web scrapper and get the latest UofT data:
- Ensure that the project **Web Scrapper** have the latest nuget packages:
  - HTML Agility Pack
  - Selenium Webdriver
  - Selenium Support
- Ensure that the chrome driver (chromedriver.exe) is copied to the output directory, under the folder:
_Selenium/Web drivers_
- Edit the connection string of the **UofT.dbml** file so that any updates/deletions to the database will be made to your database.
- Run it by selecting the project **Web Scrapper** as the start-up project, and run it.
**NOTE:** It takes a while to scrape the data from the web.

##### Set up the Data Models project
- Change the connection string in the **UofT.dbml** file to your database.
- Build the project by setting it as a start-up project and pressing F5.

##### Running the Web Api and the Angular JS app on the local machine
- Open up the Solutions Explorer in Visual Studio.
- Right-click on the solution and select **Properties**.
- Configure the start-up project so that it will run both the **Web Api** and the **Webpage** project. 
- Save the changes.
- Finally, run the projects by pressing F5. Two webpages should appear: the webpage from the Web Api and the webpage to the front-end application.

### Usage
Please note that this project is used for educational purposes and is not intended to be used commercially. We are not liable for any damages/changes done by this project.

### Credits
Emilio Kartono, who made the entire project.

### License
This project is protected under the GNU licence. Please refer to the Licence.txt for more information.
