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
            * Called when one-way binding value (s) is changed
            * @param {changeObj} changeObj - A change object (refer to the Angular JS 1.5 documentation)
            */
            this.$onChanges = function (changeObj) {
                if (changeObj.classType != undefined)
                    this.classType = changeObj.classType.currentValue;

                if (changeObj.walkDistance != undefined)
                    this.walkDistance = changeObj.walkDistance.currentValue;

                if (changeObj.numDaysInClass != undefined)
                    this.numDaysInClass = changeObj.numDaysInClass.currentValue;

                if (changeObj.timeBetweenClasses != undefined)
                    this.timeBetweenClasses = changeObj.timeBetweenClasses.currentValue;

                if (changeObj.lunchPeriod != undefined)
                    this.lunchPeriod = changeObj.lunchPeriod.currentValue;
            };

            /**
            * Changes the preferences' class type property
            * If the newClassType is the same as the current preferences' class type property,
             * it will set the preferences' class type property to null
            * @param {String} newClassType - The new class type setting
            */
            this.changeClassType = function (newClassType) {
                if (this.classType === newClassType)
                    this.classType = null;
                else
                    this.classType = newClassType;
            };

            /**
            * Change the preferences' total walk distance property
            * If the newWalkDistance is the same as the current preferences' walk distance property,
             * it will set the preferences' walk distance property to null
            * @param {String} newWalkDistance - The new walk distance setting
            */
            this.changeWalkDistance = function (newWalkDistance) {
                if (this.walkDistance === newWalkDistance)
                    this.walkDistance = null;
                else
                    this.walkDistance = newWalkDistance;
            };

            /**
            * Change the preferences' number of days in class property
            * If the newNumDaysInClass is the same as the current preferences' number of days in class
             * property, it will set the preferences' number of days in class property to null
            * @param {String} newNumDaysInClass - The new number of days in class setting
            */
            this.changeNumDaysInClass = function (newNumDaysInClass) {
                if (this.numDaysInClass === newNumDaysInClass)
                    this.numDaysInClass = null;
                else
                    this.numDaysInClass = newNumDaysInClass;
            };

            /**
             * Change the preferences' time between classes property
             * If the newTimeBetweenClasses is the same as the current preferneces' time between classes
             * property, it will set the preferences' time between classes property to null
             * @param {int} newTimeBetweenClasses - The time (minutes) between classes
             */
            this.changeTimeBetweenClasses = function (newTimeBetweenClasses) {
                if (this.timeBetweenClasses === newTimeBetweenClasses)
                    this.timeBetweenClasses = null;
                else
                    this.timeBetweenClasses = newTimeBetweenClasses;
            };

            /**
             * Called when the apply button is clicked
             */
            this.apply = function () {

                var newPreferences = {
                    classType: this.classType,
                    walkDistance: this.walkDistance,
                    timeBetweenClasses: this.timeBetweenClasses,
                    lunchPeriod: this.lunchPeriod
                };

                // The {} is needed to specify the parameter "preferences" to the object "newPreferences"
                this.onUpdate({ preferences: newPreferences })
            };

            /**
             * Called when the cancel button is clicked
             */
            this.cancel = function () {
                this.onCancel();
            };
        }
    });
}());