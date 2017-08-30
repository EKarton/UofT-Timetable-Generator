"use strict";

(function () {

    // Create a new directive <Timetable data=timetables[0] displayInfo=true></Timetable>
    var app = angular.module("timetableApp");
    app.directive("timetablePreview", function () {

        var TimetableBlock = function (x, y, width, height, color) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = color;
        };

        var Gridline = function (x1, y1, x2, y2) {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        var Title = function (title, x, y) {
            this.title = title;
            this.x = x;
            this.y = y;
        }

        var controller = function ($scope) {
            $scope.days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            $scope.earliestTime = 9.00;
            $scope.latestTime = 23.00;

            $scope.headerHeight = 10;
            $scope.svgWidth = null;
            $scope.svgHeight = null;
        };

        var link = function (scope, element, attributes) {
            var columnWidth = null;
            var dataRowHeight = 10;

            function createBlocks(blocks, colorScheme) {
                scope.timetableBlocks = [];

                for (var i = 0; i < blocks.length; i++) {
                    var x = (columnWidth * (blocks[i].startDay));
                    var y = scope.headerHeight + (dataRowHeight * (blocks[i].startTime - scope.earliestTime));
                    var width = columnWidth;
                    var height = (blocks[i].endTime - blocks[i].startTime) * dataRowHeight;
                    var color = colorScheme[blocks[i].courseCode + "|" + blocks[i].activityType];
                    scope.timetableBlocks.push(new TimetableBlock(x, y, width, height, color));
                }
            }

            function createGridlines() {
                scope.gridLines = [];

                // Making horizontal lines
                var numHorizontalLines = scope.latestTime - scope.earliestTime;
                for (var i = 0; i <= numHorizontalLines; i++) {
                    var x1 = 0;
                    var y1 = scope.headerHeight + (i * dataRowHeight);
                    var x2 = scope.svgWidth;
                    var y2 = y1;
                    scope.gridLines.push(new Gridline(x1, y1, x2, y2));
                }

                // Making the vertical lines
                for (var i = 0; i <= scope.days.length; i++) {
                    var x1 = i * columnWidth;
                    var y1 = scope.headerHeight;
                    var x2 = x1;
                    var y2 = scope.svgHeight;
                    scope.gridLines.push(new Gridline(x1, y1, x2, y2));
                }
            }

            function setSize() {
                var svgElement = element.find("svg")[0];

                // Get the width of parent
                scope.svgWidth = svgElement.getBoundingClientRect().width;
                columnWidth = scope.svgWidth / scope.days.length;

                // Compute the height
                scope.svgHeight = dataRowHeight * (scope.latestTime - scope.earliestTime) + scope.headerHeight;

                // Set the width and height
                svgElement.setAttribute("viewBox", "0, 0, " + scope.svgWidth + ", " + scope.svgHeight);
                svgElement.setAttribute("preserveAspectRatio", "xMaxYMin slice");
            }

            // Each time the data's value is changed, it updates the graph
            scope.$watchCollection("[blocks,  colorscheme, charttitle]", function (newValues, oldValues) {
                var blocks = newValues[0];
                var colorScheme = newValues[1];
                scope.chartTitle = newValues[2];

                //console.log("Timetable previre: ", blocks);

                setSize();
                createGridlines();
                createBlocks(blocks, colorScheme);
            });
        };

        return {
            restrict: "E",
            scope: {  blocks: "=", colorscheme: "=", charttitle: "@charttitle" },
            templateUrl: "app/timetables page/timetable preview/timetable-preview.directive.html",
            controller: controller,
            link: link
        };
    });
}());