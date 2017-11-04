﻿"use strict";

(function () {
    var app = angular.module("timetableApp");
    app.component("restrictionsPanel", {
        templateUrl: "app/timetables page/restrictions/restrictions-panel.component.html",
        bindings: {
            initialRestrictions: "<",       // One way binding
            onUpdate: "&",                    // Function binding
            onCancel: "&"                      // Function binding
        },
        controllerAs: "vm",     // This is used so that the restrictions-panel HTML can refer to the variables in this controller
        controller: function () {

            // Get the initial restrictions
            this.earliestClass = this.initialRestrictions.earliestClass;
            this.latestClass = this.initialRestrictions.latestClass;
            this.walkDurationInBackToBackClasses = this.initialRestrictions.walkDurationInBackToBackClasses;

            /**
             * Called when the value of any one-way binding(s) has been changed
             * @param {changeObj} changeObj - The changed one-way binding values (refer to Angular JS 1.5 doc)
             */
            this.$onChanges = function (changeObj) {
                if (changeObj == undefined)
                    return;

                if (changeObj.earliestClass != undefined)
                    this.earliestClass = changeObj.earliestClass.currentValue;

                if (changeObj.latestClass != undefined)
                    this.latestClass = changedObj.latestClass.currentValue;

                if (changeObj.walkDurationInBackToBackClasses != undefined)
                    this.walkDurationInBackToBackClasses = changedObj.walkDurationInBackToBackClasses.currentValue;
            };

            /**
            * Called when the Apply button is clicked
            */
            this.apply = function () {
                var newRestrictions = {
                    earliestClass: this.earliestClass,
                    latestClass: this.latestClass,
                    walkDurationInBackToBackClasses: this.walkDurationInBackToBackClasses
                };

                this.onUpdate({ restrictions: newRestrictions });
            };

            /**
            * Called when the Cancel button is clicked
            */
            this.cancel = function () {
                this.onCancel();
            };
        }
    });
}());