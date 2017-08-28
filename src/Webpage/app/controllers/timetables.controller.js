"use strict";

(function(){
	var app = angular.module("timetableApp");
    app.controller("TimetablesController", function ($scope, $location, Generator) {

        var Timetable = function (fallSessions, winterSessions, name) {
            this.fallTimetableBlocks = fallSessions;
            this.winterTimetableBlocks = winterSessions;
            this.name = name;
            this.isBookmarked = false;
        };

        $scope.OverlayContent = {
            TimetableViewer: 1,
            RestrictionsPanel: 2,
            PreferencesPanel: 3
        };

        $scope.timetables = "";
        $scope.sectionColors = {};
        $scope.selectedYearlyTimetable = null;
        $scope.selectedTerm = "";
        $scope.selectedTimetableBlocks = null;

        $scope.isSideMenuOpened = false;
        $scope.isPreferencesPanelOpened = false;
        $scope.isRestrictionsPanelOpened = false;
        $scope.overlayContent = $scope.OverlayContent.TimetableViewer;

        $scope.initPref = {
            classType: "min",
            walkDistance: "min",
            numDaysInClass: "min",
            timeBetweenClasses: "min",
            lunchPeriod: 60
        };

        $scope.initRes = {
            earliestClass: 7,
            latestClass: 23,
            walkDurationInBackToBackClasses: 10
        };

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

        $scope.generateTimetables = function () {
            // This will initiate and return a http promise. This promise does not contain any data, 
            // but it will (eventually) after it completes its http request. It does this because if
            // we wait for the http request to be complete, the browser will also wait (which is bad).
            Generator.generateUoftTimetables()
                .then(function (response) {

                    // When the $http finally gets the data
                    console.log("Got data");

                    // Parse the raw data
                    $scope.timetables = [];
                    var rawTimetables = response.data;
                    for (var i = 0; i < rawTimetables.length; i++){
                        var rawT = rawTimetables[i];
                        var parsedT = new Timetable(rawT.fallTimetableBlocks, rawT.winterTimetableBlocks, rawT.name);
                        $scope.timetables.push(parsedT);
                    }

                    generateColorScheme();
                    console.log($scope.sectionColors);
                }, function (response) {

                    // When the $http gets an error
                    $scope.timetables = "ERROR";
                }
            );
        };

        $scope.togglePreferencesPanel = function () {
            $scope.isPreferencesPanelOpened = !$scope.isPreferencesPanelOpened;
            $scope.isRestrictionsPanelOpened = false;
        };

        $scope.toggleRestrictionsPanel = function () {
            $scope.isRestrictionsPanelOpened = !$scope.isRestrictionsPanelOpened;
            $scope.isPreferencesPanelOpened = false;
        };

        $scope.openPreferencesPanel = function () {
            $scope.showOverlayPanel();
            $scope.overlayContent = $scope.OverlayContent.PreferencesPanel;
        };

        $scope.openRestrictionsPanel = function () {
            $scope.showOverlayPanel();
            $scope.overlayContent = $scope.OverlayContent.RestrictionsPanel;
        };

        $scope.openTimetableViewer = function (yearlyTimetable, term) {
            $scope.showOverlayPanel();
            $scope.overlayContent = $scope.OverlayContent.TimetableViewer;
            console.log($scope.overlayContent);
            $scope.selectedYearlyTimetable = yearlyTimetable;
            $scope.selectedTerm = term;

            // Select which timetable
            if (term == "Fall")
                $scope.selectedTimetableBlocks = yearlyTimetable.fallTimetableBlocks;
            else if (term == "Winter")
                $scope.selectedTimetableBlocks = yearlyTimetable.winterTimetableBlocks;
        };

        $scope.closeTimetableViewer = function () {
            $scope.hideOverlayPanel();
            $scope.selectedTimetable = [];
        };

        $scope.changeTermInTimetableViewer = function (term) {
            // Select which timetable
            if (term == "Fall")
                $scope.selectedTimetableBlocks = $scope.selectedYearlyTimetable.fallTimetableBlocks;
            else if (term == "Winter")
                $scope.selectedTimetableBlocks = $scope.selectedYearlyTimetable.winterTimetableBlocks;
            $scope.selectedTerm = term;
        };

        $scope.printTimetable = function (timetable) {
            window.print();
        };

        $scope.showOverlayPanel = function () {
            // Remove the vertical scroll bar for the mini timetables panel
            var miniTablesPanel = document.getElementsByTagName("body")[0];
            miniTablesPanel.style.overflow = "hidden";

            // Expand the overlay panel
            var element = document.getElementById("overlayPanel");
            element.style.height = "100%";
        };

        $scope.hideOverlayPanel = function () {
            // Show the vertical scroll bar for the mini timetables panel if needed
            var miniTablesPanel = document.getElementsByTagName("body")[0];
            miniTablesPanel.style.overflow = "auto";

            // Hide the overlay panel
            var element = document.getElementById("overlayPanel");
            element.style.height = "0";
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

        $scope.updatePreferences = function (preferences) {
            alert("Preferences updated");
        };

        $scope.updateRestrictions = function (restrictions) {
            alert("Restrictions updated");
        };

        $scope.generateTimetables();
	});
}());