"use strict";

(function(){
	var app = angular.module("timetableApp");
	app.controller("TimetablesController", function($scope, $location, Generator){
        $scope.timetables = "";
        $scope.sectionColors = [];
        $scope.selectedTimetable = null;
        $scope.selectedTerm = null;

        var generateRandomColor = function () {
            var red = Math.floor(Math.random() * 255);
            var green = Math.floor(Math.random() * 255);
            var blue = Math.floor(Math.random() * 255);
            return "rgb(" + red + ", " + green + ", " + blue + ")";
        };

        var generateColorScheme = function () {
            var dict = [];

            // If there are no timetables, return
            if ($scope.timetables.length == 0)
                return dict;

            // For the fall timetable
            var fallBlocks = $scope.timetables[0].fallTimetableBlocks;
            for (var i = 0; i < fallBlocks.length; i++)
            {
                var key = fallBlocks[i].courseCode + "|" + fallBlocks[i].activityType;
                if (dict[key] == undefined)
                    dict[key] = generateRandomColor();
            }

            // For the winter timetable
            var winterBlocks = $scope.timetables[0].winterTimetableBlocks;
            for (var i = 0; i < winterBlocks.length; i++)
            {
                var key = winterBlocks[i].courseCode + "|" + winterBlocks[i].activityType;
                if (dict[key] == undefined)
                    dict[key] = generateRandomColor();
            }

            return dict;
        };

        // This will initiate and return a http promise. This promise does not contain any data, 
        // but it will (eventually) after it completes its http request. It does this because if
        // we wait for the http request to be complete, the browser will also wait (which is bad).
        Generator.generateUoftTimetables()
            .then(function (response) {

                // When the $http finally gets the data
                console.log("Got data");
                $scope.timetables = response.data;
                $scope.sectionColors = generateColorScheme();
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

            $scope.selectedTimetable = timetable;
            $scope.selectedTerm = term;
        };

        $scope.closeTimetableViewer = function () {

            // Show the vertical scroll bar for the mini timetables panel if needed
            var miniTablesPanel = document.getElementsByTagName("body")[0];
            miniTablesPanel.style.overflow = "auto";

            // Hide the overlay panel
            var element = document.getElementById("overlayPanel");
            element.style.height = "0";

            $scope.selectedTimetable = null;
            $scope.selectedTerm = null;
        }
	});
}());