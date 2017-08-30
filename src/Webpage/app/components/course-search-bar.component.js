"use strict";

(function () {
    var app = angular.module("timetableApp");
    app.component("courseSearchBar", {
        templateUrl: "./app/components/course-search-bar.component.html",
        bindings: {
            onSelectCourse: "&"
        },
        controllerAs: "vm",
        controller: function (CourseSearch) {

            this.oldQuery = "";
            this.courseResults = [];

            this.updateCourseResults = function (newQuery) {
                // Avoid making an http request if the user enters the same department code
                if (this.oldQuery === newQuery)
                    return;

                // Check if the new department code is >= 3 chars long
                if (newQuery.length != 3)
                    return;

                var obj = this;
                CourseSearch.getUoftCourses(newQuery,
                    function (courseResults) {
                        obj.courseResults = courseResults;
                        obj.oldQuery = newQuery;
                    },
                    function (promise) {
                        alert("Unable to get course data from the server");
                    }
                );
            };

            this.selectCourse = function (selectedCourse) {
                this.s = selectedCourse;
                this.onSelectCourse({ course: this.s });
            };
        }
    });
}());