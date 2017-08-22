"use strict";

(function(){
	var app = angular.module("timetableApp");
	app.controller("TimetablesController", function($scope, $location, Generator){
        $scope.timetables = "";
        $scope.sectionColors = {};
        $scope.selectedTimetable = null;

        $scope.isSideMenuOpened = false;

        var generateRandomColor = function () {
            var red = Math.floor(Math.random() * (255 - 100) + 100);
            var green = Math.floor(Math.random() * (255 - 100) + 100);
            var blue = Math.floor(Math.random() * (255 - 100) + 100);
            return "rgb(" + red + ", " + green + ", " + blue + ")";
        };

        var generateColorScheme = function () {
            $scope.sectionColors = {};

            // For the fall timetable
            var fallBlocks = $scope.timetables[0].fallTimetableBlocks;
            for (var i = 0; i < fallBlocks.length; i++)
            {
                var key = fallBlocks[i].courseCode + "|" + fallBlocks[i].activityType;
                if ($scope.sectionColors[key] == undefined)
                    $scope.sectionColors[key] = generateRandomColor();
            }

            // For the winter timetable
            var winterBlocks = $scope.timetables[0].winterTimetableBlocks;
            for (var i = 0; i < winterBlocks.length; i++)
            {
                var key = winterBlocks[i].courseCode + "|" + winterBlocks[i].activityType;
                if ($scope.sectionColors[key] == undefined)
                    $scope.sectionColors[key] = generateRandomColor();
            }
        };

        // This will initiate and return a http promise. This promise does not contain any data, 
        // but it will (eventually) after it completes its http request. It does this because if
        // we wait for the http request to be complete, the browser will also wait (which is bad).
        Generator.generateUoftTimetables()
            .then(function (response) {

                // When the $http finally gets the data
                console.log("Got data");
                $scope.timetables = response.data;
                generateColorScheme();
                console.log($scope.sectionColors);
            }, function (response) {

                // When the $http gets an error
                $scope.timetables = "ERROR";
            }
        );

        $scope.openTimetableViewer = function (timetable, term) {

            // Remove the vertical scroll bar for the mini timetables panel
            var miniTablesPanel = document.getElementsByTagName("body")[0];
            miniTablesPanel.style.overflow = "hidden";

            // Expand the overlay panel
            var element = document.getElementById("overlayPanel");
            element.style.height = "100%";

            // Select which timetable
            if (term == "Fall")
                $scope.selectedTimetable = timetable.fallTimetableBlocks;
            else if (term == "Winter")
                $scope.selectedTimetable = timetable.winterTimetableBlocks;
        };

        $scope.closeTimetableViewer = function () {

            // Show the vertical scroll bar for the mini timetables panel if needed
            var miniTablesPanel = document.getElementsByTagName("body")[0];
            miniTablesPanel.style.overflow = "auto";

            // Hide the overlay panel
            var element = document.getElementById("overlayPanel");
            element.style.height = "0";

            $scope.selectedTimetable = null;
        };

        $scope.toggleSideMenu = function () {
            if ($scope.isSideMenuOpened)
                $scope.closeSideMenu();

            else if ($scope.isSideMenuOpened === false)
                $scope.openSideMenu();
        };

        $scope.closeSideMenu = function () {
            var element = document.getElementById("sideMenuPanel");
            element.style.width = "0";
            document.getElementById("miniTimetablesViewer").style.marginLeft = "0";
            $scope.isSideMenuOpened = false;
        };

        $scope.openSideMenu = function () {
            var sideMenuPanel = document.getElementById("sideMenuPanel");
            sideMenuPanel.style.width = "auto";

            var mainPanel = document.getElementById("miniTimetablesViewer");
            mainPanel.style.marginLeft = sideMenuPanel.getBoundingClientRect().width.toString() + "px";
            $scope.isSideMenuOpened = true;
        };
	});
}());