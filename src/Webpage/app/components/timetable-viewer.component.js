"use strict";

(function(){
    var app = angular.module("timetableApp");
    app.component("timetableViewer", {
        templateUrl: "./app/components/timetable-viewer.component.html",
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

            // This function is called when any of the one-way binding objects are changed
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

                console.log("One way binding changed");
                console.log("Change obj:", changeObj);
                console.log("New timetable:", this.yearlyTimetable);
                console.log("Section colors", this.sectionColors);
                console.log("new term:", this.term);
                console.log("new bookmark", this.isBookmarked);


                // Get the current termed timetable
                this.changeTerm(this.term);
            };

            console.log("Constructed", this.yearlyTimetable, this.term, this.sectionColors, this.isBookmarked, this.onAddBookmark, this.onRemoveBookmark);

            this.printTimetable = function () {
                window.print();
            };

            this.changeTerm = function (newTerm) {
                this.term = newTerm;

                if (newTerm === "Fall")
                    this.termTimetable = this.yearlyTimetable.fallTimetableBlocks;
                else if (newTerm === "Winter")
                    this.termTimetable = this.yearlyTimetable.winterTimetableBlocks;
            };

            this.addBookmark = function () {
                this.isBookmarked = true;
                this.onAddBookmark({ yearlyTimetable: this.yearlyTimetable });
            };

            this.removeBookmark = function () {
                this.isBookmarked = false;
                this.onRemoveBookmark({ yearlyTimetable: this.yearlyTimetable });
            };


            // Get the current termed timetable
            this.changeTerm(this.term);
        }
    });
}());