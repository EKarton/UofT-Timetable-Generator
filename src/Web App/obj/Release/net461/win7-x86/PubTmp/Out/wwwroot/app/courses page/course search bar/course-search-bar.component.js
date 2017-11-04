"use strict";

(function () {
    var app = angular.module("timetableApp");
    app.component("courseSearchBar", {
        templateUrl: "app/courses page/course search bar/course-search-bar.component.html",
        bindings: {
            selectedCourses: "<",
            onSelectCourse: "&"
        },
        controllerAs: "vm",
        controller: function (CourseSearch) {

            // The course results in the dropdown menu
            this.courseResults = [];

            /**
             * Called when one-way binding value (s) is changed
             * @param {changeObj} changeObj - A change object (refer to the Angular JS 1.5 documentation)
             */
            this.$onChanges = function (changeObj) {
                if (changeObj.selectedCourses != undefined)
                    this.selectedCourses = changeObj.selectedCourses.currentValue;
            };

            /**
             * Updates the course results after specifying a course code
             * It will not add courses that has been selected already
             * @param {string} newQuery - A complete / incomplete UofT course code
             */
            this.updateCourseResults = function (newQuery) {
                var obj = this;
                CourseSearch.getUoftCourses(newQuery,
                    function (courseResults) {                        

                        // Make sure that the new course results are not selected
                        obj.courseResults = [];
                        for (var i = 0; i < courseResults.length; i++) {

                            // Check if it is a selected course
                            var isSelectedCourse = false;
                            for (var j = 0; j < obj.selectedCourses.length; j++) {
                                if (courseResults[i].code === obj.selectedCourses[j].code) {
                                    isSelectedCourse = true;
                                    break;
                                }
                            }

                            // If it is not a selected course, add it
                            if (!isSelectedCourse)
                                obj.courseResults.push(courseResults[i]);
                        }
                    },
                    function (promise) {
                        alert("Unable to get course data from the server");
                    }
                );
            };

            /**
             * Selects a course
             * @param {Course} selectedCourse - A course selected from the dropdown menu
             */
            this.selectCourse = function (selectedCourse) {
                this.s = selectedCourse;
                this.onSelectCourse({ course: this.s });

                // Remove any course results that has been selected already
                for (var i = 0; i < this.courseResults.length; i++) {
                    for (var j = 0; j < this.selectedCourses.length; j++) {
                        if (this.courseResults[i].code === this.selectedCourses[j].code) {
                            this.courseResults.splice(i, 1);
                            i -= 1;
                        }
                    }
                }
            };
        }
    });
}());