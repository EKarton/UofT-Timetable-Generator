"use strict";

(function () {
    var app = angular.module("timetableApp");
    app.component("courseSearchBar", {
        templateUrl: "app/courses page/course search bar/course-search-bar.component.html",
        bindings: {
            selectedCourses: "<",
            onSelectCourse: "&",
            onDeselectCourse: "&"
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
                if (changeObj.selectedCourses !== undefined)
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

                        console.log(obj.selectedCourses);

                        obj.courseResults = [];

                        // Add the courses from the server that were not selected yet
                        if (obj.selectedCourses.length > 0) {
                            for (var i = 0; i < courseResults.length; i++) {

                                var isAlreadySelected = false;

                                for (var j = 0; j < obj.selectedCourses.length; j++) {
                                    if (obj.selectedCourses[j].code === courseResults[i].code) {
                                        if (obj.selectedCourses[j].campus === courseResults[i].campus) {
                                            obj.courseResults.push(obj.selectedCourses[j]);
                                            isAlreadySelected = true;
                                            break;
                                        }
                                    }
                                }

                                if (!isAlreadySelected)
                                    obj.courseResults.push(courseResults[i]);
                            }
                        }
                        else
                            obj.courseResults = courseResults;
                        console.log(obj.courseResults);
                    },
                    function (promise) {
                        alert("Unable to get course data from the server");
                    }
                );
            };

            /**
             * Handles whether a course was clicked.
             * It will deselect the course when the course was selected.
             * It will select the courses to add to the timetable when the course was not selected yet.
             * @param {Course} course - A course clicked from the search results
             */
            this.onCourseClicked = function (course) {
                if (!course.isSelected) {
                    this.s = course;
                    course.isSelected = true;
                    this.onSelectCourse({ course: this.s });
                }
                else {
                    this.s = course;
                    course.isSelected = false;
                    this.onDeselectCourse({ course: this.s });
                }
            }
        }
    });
}());