# CSCE 361 - Software Engineering Project #

The developers on this project are Abby Seibel, Alexis Aguilera Ortiz, Bao Bui, Emmanuel Lopez, and Kevin Pham 

This project was developed in an Agile workflow and focuses on the SOLID Principles, good UI/UX design techniques, and overall Software engineering principles.

# Voting System #

The voting system is a web app that runs the elections of a small town. 

During an election year the portal allows voters to vote once in each election for a selected canidate and on a handful of issues. Third parties can see who has voted, but not how they have voted. There is a secure login port, and an overall dashboard that counts results

The town administration is in charge of putting issues, elections, canidates and registered voter's information into the database system.

## Architecture ##
The architecture used in this project is based on IDESIGN and MVC. This is to decouple the code and produce them in specific modules. Specifically all the buisness Logic will be handled by engine classes (called controller classes in the solution file), data, and accessor classes as well. 

## Unit Testing ## 

MS Test is used to test the functionality of the Engines and Accessors

The Three A methodology, Arrange, Act, Assert is used to make sure that all functions have the proper output. 


## Angular ##
The front end is built on a Node.js angular framework. 

In this framework, Typescript files are very important to the functionality of the app.

Therefore, the solid principles are followed even in typescript files, with most of our interfaces being found here.



# Additional notes and instructions for the app # 

It is important to note that voters can only vote for one canidate but many issues. 

Once a voter has submited their vote to be recorded, they can look at their vote but not change it.

