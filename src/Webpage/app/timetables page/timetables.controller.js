"use strict";

(function(){
	var app = angular.module("timetableApp");
    app.controller("TimetablesController", function ($scope, $location, TimetableGenerator) {

        $scope.data = TimetableGenerator.generatedTimetables;

        $scope.selectedYearlyTimetable = null;
        $scope.selectedTerm = "";
        $scope.selectedTimetableBlocks = null;

        $scope.isSideMenuOpened = false;
        $scope.isPreferencesPanelOpened = false;
        $scope.isRestrictionsPanelOpened = false;

        $scope.togglePreferencesPanel = function () {
            $scope.isPreferencesPanelOpened = !$scope.isPreferencesPanelOpened;
            $scope.isRestrictionsPanelOpened = false;
        };

        $scope.toggleRestrictionsPanel = function () {
            $scope.isRestrictionsPanelOpened = !$scope.isRestrictionsPanelOpened;
            $scope.isPreferencesPanelOpened = false;
        };

        $scope.openTimetableViewer = function (yearlyTimetable, term) {
            $scope.showOverlayPanel();
            $scope.selectedYearlyTimetable = yearlyTimetable;
            $scope.selectedTerm = term;
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
            // If it is different, update
            if ($scope.data.preferences != preferences)
                TimetableGenerator.generateTimetables($scope.data.courseCodes, preferences, $scope.data.restrictions);

            $scope.togglePreferencesPanel();
        };

        $scope.updateRestrictions = function (restrictions) {
            // If it is different, update
            if ($scope.data.restrictions != restrictions)
                TimetableGenerator.generateTimetables($scope.data.courseCodes, $scope.data.preferences, restrictions);

            $scope.toggleRestrictionsPanel();
        };

        $scope.regenerateTimetables = function () {
            TimetableGenerator.generateTimetables(
                $scope.data.courseCodes,
                $scope.data.preferences,
                $scope.data.restrictions
            );
        };
	});
}());