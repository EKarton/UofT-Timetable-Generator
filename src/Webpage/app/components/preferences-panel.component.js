"use strict";

(function () {
    var app = angular.module("timetableApp");
    app.component("preferencesPanel", {
        templateUrl: "./app/templates/preferences-panel.template.html",
        bindings: {
            initialPreference: "<",
            onUpdate: "&",
            onCancel: "&"
        },
        controllerAs: "vm",         // This is needed to prevent confusion in the parent's scope vs this scope when accessing variables in html
        controller: function () {

            // Set the UI initial preference values
            this.classType = this.initialPreference.classType;
            this.walkDistance = this.initialPreference.walkDistance;
            this.numDaysInClass = this.initialPreference.numDaysInClass;
            this.timeBetweenClasses = this.initialPreference.timeBetweenClasses;
            this.lunchPeriod = this.initialPreference.lunchPeriod;

            this.changeClassType = function (newClassType) {
                if (this.classType === newClassType)
                    this.classType = null;
                else
                    this.classType = newClassType;
            };

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