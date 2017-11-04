"use strict";

(function () {
    var app = angular.module("timetableApp");
    app.component("selectedCourseList", {
        templateUrl: "app/courses page/selected course list/selected-course-list.component.html",
        bindings: {
            selectedCourses: "<",
            onRemoveCourse: "&"
        },
        controllerAs: "vm",
        controller: function () {

            /**
             * Called when the value of any one-way binding(s) has been changed
             * @param {changeObj} changeObj - The changed one-way binding values (refer to Angular JS 1.5 doc)
             */
            this.$onChanges = function (changeObj) {
                if (changeObj.selectedCourses != undefined)
                    this.selectedCourses = changeObj.selectedCourses.currentValue;
            };

            /**
             * Removes a course
             * @param {Course} course - The course to remove
             */
            this.removeCourse = function (course) {
                this.onRemoveCourse({course: course});
            };
        }
    });
}());