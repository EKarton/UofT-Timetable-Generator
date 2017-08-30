"use strict";

(function () {
    var app = angular.module("timetableApp");
    app.component("preferencesPanel", {
        templateUrl: "app/timetables page/preferences/preferences-panel.component.html",
        bindings: {
            initialPreferences: "<",   // One way binding
            onUpdate: "&",              // Function binding; this component can use this function
            onCancel: "&"                 // Function binding; this component can use this function
        },
        controllerAs: "vm",         // This is needed to prevent confusion in the parent's scope vs this scope when accessing variables in html
        controller: function () {

            // Set the UI initial preference values
            this.classType = this.initialPreferences.classType;
            this.walkDistance = this.initialPreferences.walkDistance;
            this.numDaysInClass = this.initialPreferences.numDaysInClass;
            this.timeBetweenClasses = this.initialPreferences.timeBetweenClasses;
            this.lunchPeriod = this.initialPreferences.lunchPeriod;

            /**
            * Called when one of the buttons for the ClassType property is clicked
            * If the newClassType is the same as the previous class type, it will set the current class type to null
            * @param {String} newClassType The new class type
            */
            this.changeClassType = function (newClassType) {
                if (this.classType === newClassType)
                    this.classType = null;
                else
                    this.classType = newClassType;
            };

            /**
            * Called when one of the buttons for the WalkDistance property is clicked
            * If the newWalkDistance is the same as the previous class type, it will set the current class type to null
            * @param {String} newWalkDistance The new walk distance
            */
            this.changeWalkDistance = function (newWalkDistance) {
                if (this.walkDistance === newWalkDistance)
                    this.walkDistance = null;
                else
                    this.walkDistance = newWalkDistance;
            };

            this.changeNumDaysInClass = function (newNumDaysInClass) {
                if (this.numDaysInClass === newNumDaysInClass)
                    this.numDaysInClass = null;
                else
                    this.numDaysInClass = newNumDaysInClass;
            };

            this.changeTimeBetweenClasses = function (newTimeBetweenClasses) {
                if (this.timeBetweenClasses === newTimeBetweenClasses)
                    this.timeBetweenClasses = null;
                else
                    this.timeBetweenClasses = newTimeBetweenClasses;
            };

            this.apply = function () {
                console.log("Apply clicked");
                console.log("New settings", this.classType, this.walkDistance, this.daysWithClass, this.timeBetweenClasses, this.lunchPeriod);

                var newPreferences = {
                    classType: this.classType,
                    walkDistance: this.walkDistance,
                    timeBetweenClasses: this.timeBetweenClasses,
                    lunchPeriod: this.lunchPeriod
                };

                // The {} is needed to specify the parameter "preferences" to the object "newPreferences"
                this.onUpdate({ preferences: newPreferences })
            };

            this.cancel = function () {
                this.onCancel();
            };
        }
    });
}());