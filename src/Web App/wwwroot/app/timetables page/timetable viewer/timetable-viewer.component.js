﻿"use strict";

(function(){
    var app = angular.module("timetableApp");
    app.component("timetableViewer", {
        templateUrl: "app/timetables page/timetable viewer/timetable-viewer.component.html",
        bindings: {
            yearlyTimetable: "<",
            term: "<",
            sectionColors: "<",
            isBookmarked: "<",
            onAddBookmark: "&",
            onRemoveBookmark: "&"
        },
        controllerAs: "vm",
        controller: function () {

            /**
             * This function is called when any of the one-way binding objects are changed.
             * @param {changeObj} changeObj - The changeObj (refer to the Angular JS 1.5 documentation)
             */
            this.$onChanges = function (changeObj) {

                // If the yearly timetable changed
                if (changeObj.yearlyTimetable != undefined)
                    this.yearlyTimetable = changeObj.yearlyTimetable.currentValue;

                // If the section colors changed
                if (changeObj.sectionColors != undefined)
                    this.sectionColors = changeObj.sectionColors.currentValue;

                // If it is the term that changed
                if (changeObj.term != undefined)
                    this.term = changeObj.term.currentValue;

                // If it is the isBookmarked that changed
                if (changeObj.isBookmarked != undefined)
                    this.isBookmarked = changeObj.isBookmarked.currentValue;

                // Get the current termed timetable
                this.changeTerm(this.term);
            };

            /**
             * Prints the page
             */
            this.printTimetable = function () {
                window.print();
            };

            /**
             * Changes the term of the timetable to view
             */
            this.changeTerm = function (newTerm) {
                this.term = newTerm;

                if (newTerm === "Fall")
                    this.termTimetable = this.yearlyTimetable.fallTimetableBlocks;
                else if (newTerm === "Winter")
                    this.termTimetable = this.yearlyTimetable.winterTimetableBlocks;
            };

            /**
             * Bookmarks the current yearly timetable
             */
            this.addBookmark = function () {
                this.isBookmarked = true;
                this.onAddBookmark({ yearlyTimetable: this.yearlyTimetable });
            };

            /**
             * Removes the current yearly timetable from bookmarks
             */
            this.removeBookmark = function () {
                this.isBookmarked = false;
                this.onRemoveBookmark({ yearlyTimetable: this.yearlyTimetable });
            };

            // Get the current termed timetable
            this.changeTerm(this.term);
        }
    });
}());