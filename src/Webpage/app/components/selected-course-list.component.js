"use strict";

(function () {
    var app = angular.module("timetableApp");
    app.component("selectedCourseList", {
        templateUrl: "./app/components/selected-course-list.component.html",
        bindings: {
            selectedCourses: "<",
            onRemoveCourse: "&"
        },
        controllerAs: "vm",
        controller: function () {

            this.$onChanges = function (changeObj) {
                if (changeObj.selectedCourses != undefined)
                    this.selectedCourses = changeObj.selectedCourses.currentValue;
            };

            this.removeCourse = function (course) {
                this.onRemoveCourse({course: course});
            };
        }
    });
}());