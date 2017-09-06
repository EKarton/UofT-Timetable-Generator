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
This project consists of several components, each responsible for performing a certain task to get the best timetables. The image below illustrates the system architecture of the project.
<div width="100%" style="text-align: center;">
<img src="https://raw.githubusercontent.com/EKarton/UofT-Timetable-Generator/master/docs/images/System%20Architecture.PNG" width="800px"/>
</div>

Users will be using the front-end web application to generate their timetables. In the homepage, they will first have to specify the courses they will be taking. 
<div width="100%" style="text-align: center;">
<img src="https://raw.githubusercontent.com/EKarton/UofT-Timetable-Generator/master/docs/images/Hompage.PNG" width="800px"/>
</div>

Typing the first three letters of the course code in the search bar will reveal the relevant, available courses.
<div width="100%" style="text-align: center;">
<img src="https://raw.githubusercontent.com/EKarton/UofT-Timetable-Generator/master/docs/images/Selecting%20a%20course.png" width="800px"/>
</div>

After selecting the courses, click on the "Generate Timetables" button. It will then call the server to generate the timetables, and navigate to the Timetables page. The generated timetables will be displayed in icons like the image below.
<div width="100%" style="text-align: center;">
<img src="https://raw.githubusercontent.com/EKarton/UofT-Timetable-Generator/master/docs/images/Timetables%20page.PNG" width="800px"/>
</div>

In addition, users can specify restrictions and preferences to tailor their timetables to their needs. It is done by clicking on the Preferences or Restrictions button on the top of the page. The preferences/restrictions panel will appear, revealing the options the users have. Applying their new restrictions/preferences will regenerate their timetables.
<div width="100%" style="text-align: center;">
<img src="https://raw.githubusercontent.com/EKarton/UofT-Timetable-Generator/master/docs/images/Restriction%20panel.PNG" width="800px"/>
</div>

Users are able to view, print, and bookmark their favorite timetables by clicking on one of the generated timetable.
<div width="100%" style="text-align: center;">
<img src="https://raw.githubusercontent.com/EKarton/UofT-Timetable-Generator/master/docs/images/Timetable%20viewer.PNG" width="800px"/>
</div>

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
