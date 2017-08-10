"use strict";

(function () {

    // Create a new directive <Timetable data=timetables[0] displayInfo=true></Timetable>
    var app = angular.module("timetableApp");
    app.directive("uofttimetable", function () {

        var namespace = "http://www.w3.org/2000/svg";
        var days = ["", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        var svgElement = null;

        function addTitle(title) {
            console.log(title);
            var gElement = document.createElementNS(namespace, "g");
            var textElement = document.createElementNS(namespace, "text");
            textElement.textContent = title;
            textElement.setAttribute("x", 10);
            textElement.setAttribute("y", 10);
            gElement.append(textElement);
            svgElement.append(gElement);
        }

        function addBlock(block, dx, dy, width, height) {
            var timeToStartDay = 9.00;
            var timeToEndDay = 23.00;
            var widthInterval = width / 7;                                                              // The width of a rect per day
            var heightInterval = height / (timeToEndDay - timeToStartDay);  // The height of a rect per hr
            var startTime = block.startTime % 100;
            var endTime = block.endTime % 100;
            var startDay = parseInt(block.startTime / 100);
            var endDay = parseInt(block.endTime / 100);
            console.log(block);

            for (var i = startDay; i <= endDay; i++)
            {
                var rectElement = document.createElementNS(namespace, "rect");
                rectElement.setAttribute("fill", "black");
                if (startDay === endDay)
                {
                    var rectX = dx + (width / 7 * i);
                    var rectY = dy + (heightInterval * (startTime - timeToStartDay));
                    var rectWidth = widthInterval;
                    var rectHeight = (endTime - startTime) * heightInterval;
                    rectElement.setAttribute("x", rectX);
                    rectElement.setAttribute("y", rectY);
                    rectElement.setAttribute("width", rectWidth);
                    rectElement.setAttribute("height", rectHeight);
                }

                else if (i === startDay)
                {
                    var rectX = dx + (width / 7 * i);
                    var rectY = dy + (heightInterval * (startTime - timeToStartDay));
                    var rectWidth = widthInterval;
                    var rectHeight = (timeToEndDay - startTime) / heightInterval;
                    rectElement.setAttribute("x", rectX);
                    rectElement.setAttribute("y", rectY);
                    rectElement.setAttribute("width", rectWidth);
                    rectElement.setAttribute("height", rectHeight);
                }
                else if (i === endDay)
                {
                    var rectX = dx + (width / 7 * i);
                    var rectY = dy + (heightInterval * (startTime - timeToStartDay));
                    var rectWidth = widthInterval;
                    var rectHeight = (timeToEndDay - startTime) / heightInterval;

                    if (startDay == endDay)
                        rectHeight = (endTime - startTime) * heightInterval;

                    rectElement.setAttribute("x", rectX);
                    rectElement.setAttribute("y", rectY);
                    rectElement.setAttribute("width", rectWidth);
                    rectElement.setAttribute("height", rectHeight);
                }
                else
                {                    
                    rectElement.setAttribute("y", dy);
                    rectElement.setAttribute("x", dx + (width / 7 * i));
                    rectElement.setAttribute("width", widthInterval);
                    rectElement.setAttribute("height", height);
                }
                svgElement.append(rectElement);
            }
        }

        function createTimetable(scope, element, attributes) {
            // Wait for the data to be reached
            scope.$watchCollection("[blocks, displayinfo, charttitle]", function (newValues, oldValues) {
                svgElement = element.find("svg")[0];

                // Add a title
                addTitle(newValues[2]);

                // Add the blocks
                var blocks = newValues[0];
                for (var i = 0; i < blocks.length; i++)
                    addBlock(blocks[i], 10, 10, svgElement.clientWidth, svgElement.clientHeight);
            });
        }

        return {
            restrict: "E",
            scope: {  blocks: "=", displayinfo: "=", charttitle: "@charttitle" },
            templateUrl: "uoftDirective.template.html",
            link: createTimetable
        };
    });
}());