"use strict";

(function(){
	var app = angular.module("timetableApp");
    app.controller("TimetablesController", function ($scope, $location, TimetableGenerator) {

        // The generated timetables, restrictions, etc
        $scope.data = TimetableGenerator.generatedTimetables;

        // The selected yearly timetable, term, and blocks that was clicked
        $scope.selectedYearlyTimetable = null;
        $scope.selectedTerm = "";

        // Triggers for each component in the Timetables page
        $scope.isSideMenuOpened = false;
        $scope.isPreferencesPanelOpened = false;
        $scope.isRestrictionsPanelOpened = false;

        /**
         * Open / close the preferences panl
         */
        $scope.togglePreferencesPanel = function () {
            $scope.isPreferencesPanelOpened = !$scope.isPreferencesPanelOpened;
            $scope.isRestrictionsPanelOpened = false;
        };

        /**
         * Open / close the restrictions panel
         */
        $scope.toggleRestrictionsPanel = function () {
            $scope.isRestrictionsPanelOpened = !$scope.isRestrictionsPanelOpened;
            $scope.isPreferencesPanelOpened = false;
        };

        /**
         * Open the timetable viewer
         * @param {Timetable} yearlyTimetable - The yearly timetable to view
         * @param {string} term - The term in the timetable to view
         */
        $scope.openTimetableViewer = function (yearlyTimetable, term) {
            $scope.showOverlayPanel();
            $scope.selectedYearlyTimetable = yearlyTimetable;
            $scope.selectedTerm = term;
        };

        /**
         * Opens the overlay panel in the page
         */
        $scope.showOverlayPanel = function () {
            // Remove the vertical scroll bar for the mini timetables panel
            var miniTablesPanel = document.getElementsByTagName("body")[0];
            miniTablesPanel.style.overflow = "hidden";

            // Expand the overlay panel
            var element = document.getElementById("overlayPanel");
            element.style.height = "100%";
        };

        /**
         * Closes the overlay panel in the page
         */
        $scope.hideOverlayPanel = function () {
            // Show the vertical scroll bar for the mini timetables panel if needed
            var miniTablesPanel = document.getElementsByTagName("body")[0];
            miniTablesPanel.style.overflow = "auto";

            // Hide the overlay panel
            var element = document.getElementById("overlayPanel");
            element.style.height = "0";
        };

        /**
         * Open / close the side meny
         */
        $scope.toggleSideMenu = function () {
            if ($scope.isSideMenuOpened)
                $scope.closeSideMenu();

            else if ($scope.isSideMenuOpened === false)
                $scope.openSideMenu();
        };

        /**
         * Close the side menu
         */
        $scope.closeSideMenu = function () {
            var element = document.getElementById("sideMenuPanel");
            element.style.width = "0";
            document.getElementById("miniTimetablesViewer").style.marginLeft = "0";
            $scope.isSideMenuOpened = false;
        };

        /**
         * Open the side menu
         */
        $scope.openSideMenu = function () {
            var sideMenuPanel = document.getElementById("sideMenuPanel");
            sideMenuPanel.style.width = "auto";

            var mainPanel = document.getElementById("miniTimetablesViewer");
            mainPanel.style.marginLeft = sideMenuPanel.getBoundingClientRect().width.toString() + "px";
            $scope.isSideMenuOpened = true;
        };

        /**
         * Updates the preferences and regenerates the timetables.
         * It will not generate new timetables if the new preferences are the same as the old
         * @param {Preferences} preferencs - The new preferences
         */
        $scope.updatePreferences = function (preferences) {
            // If it is different, update
            if ($scope.data.preferences != preferences)
                TimetableGenerator.generateTimetables($scope.data.courseCodes, preferences, $scope.data.restrictions);

            $scope.togglePreferencesPanel();
        };

        /**
         * Updates the restrictions and regenerates the timetables.
         * It will not generate new timetables if the new restrictions are the same as the old
         * @param {Restrictions} restrictions - The new restrictions
         */
        $scope.updateRestrictions = function (restrictions) {
            // If it is different, update
            if ($scope.data.restrictions != restrictions)
                TimetableGenerator.generateTimetables($scope.data.courseCodes, $scope.data.preferences, restrictions);

            $scope.toggleRestrictionsPanel();
        };

        /**
         * Regenerates the timetables
         */
        $scope.regenerateTimetables = function () {
            TimetableGenerator.generateTimetables(
                $scope.data.courseCodes,
                $scope.data.preferences,
                $scope.data.restrictions
            );
        };
	});
}());