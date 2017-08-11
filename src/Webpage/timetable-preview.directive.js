"use strict";

(function () {

    // Create a new directive <Timetable data=timetables[0] displayInfo=true></Timetable>
    var app = angular.module("timetableApp");
    app.directive("timetablePreview", function () {

        var days = ["", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        var timeToStartDay = 9.00;
        var timeToEndDay = 23.00;

        var namespace = "http://www.w3.org/2000/svg";
        var svgElement = null;

        function addTitle(title) {
            console.log(title);
            var gElement = document.createElementNS(namespace, "g");
            var textElement = document.createElementNS(namespace, "text");

            // Set the text to the title and horizontally align it to the middle
            textElement.textContent = title;
            textElement.setAttribute("text-anchor", "middle");

            // Set the middle x coordinate and the bottom y coordinate
            textElement.setAttribute("x", svgElement.clientWidth / 2);
            textElement.setAttribute("y", 20);

            // Add the new text element
            gElement.append(textElement);
            svgElement.append(gElement);
        }

        function addGridlines(dx, dy) {
            var widthInterval = (svgElement.clientWidth - dx) / 7;     
            var heightInterval = (svgElement.clientHeight - dy) / (timeToEndDay - timeToStartDay + 1);

            // Creating a container that will hold all the grid lines
            var linesContainer = document.createElementNS(namespace, "g");

            // Making the horizontal lines
            var numHorizontalLines = timeToEndDay - timeToStartDay + 1;
            for (var i = 0; i <= numHorizontalLines; i++)
            {
                var line = document.createElementNS(namespace, "line");
                line.setAttribute("x1", dx);
                line.setAttribute("y1", dy + (i * heightInterval));
                line.setAttribute("x2", dx + svgElement.clientWidth);
                line.setAttribute("y2", dy + (i * heightInterval));

                line.setAttribute("style", "stroke:#c5c8cc; stroke-width:1");
                linesContainer.append(line);
            }

            // Making the vertical lines
            for (var curDay = 0; curDay < days.length; curDay++)
            {
                var line = document.createElementNS(namespace, "line");
                line.setAttribute("x1", dx + (curDay) * widthInterval);
                line.setAttribute("y1", dy);
                line.setAttribute("x2", dx + (curDay) * widthInterval);
                line.setAttribute("y2", svgElement.clientHeight);

                line.setAttribute("style", "stroke:#c5c8cc; stroke-width:1");
                linesContainer.append(line);
            }

            // Adding the container to the svg element
            svgElement.append(linesContainer);
        }

        function addBlock(block, dx, dy, color) {
            var widthInterval = (svgElement.clientWidth - dx) / 7;
            var heightInterval = (svgElement.clientHeight - dy) / (timeToEndDay - timeToStartDay + 1);
            var startTime = block.startTime % 100;
            var endTime = block.endTime % 100;
            var startDay = parseInt(block.startTime / 100);
            var endDay = parseInt(block.endTime / 100);
            console.log(block);

            for (var i = startDay; i <= endDay; i++)
            {
                var rectElement = document.createElementNS(namespace, "rect");
                rectElement.setAttribute("fill", color);
                if (startDay === endDay)
                {
                    var rectX = dx + (widthInterval * i);
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
                    var rectX = dx + (widthInterval * i);
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
                    var rectX = dx + (widthInterval * i);
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
                    rectElement.setAttribute("x", dx + (widthInterval * i));
                    rectElement.setAttribute("width", widthInterval);
                    rectElement.setAttribute("height", height);
                }
                svgElement.append(rectElement);
            }
        }

        function createTimetable(scope, element, attributes) {

            // Each time the data's value is changed, it updates the graph
            scope.$watchCollection("[blocks,  colorscheme, charttitle]", function (newValues, oldValues) {
                var blocks = newValues[0];
                var colorScheme = newValues[1];
                var title = newValues[2];
                svgElement = element.find("svg")[0];

                // Add a title
                addTitle(title);

                // Add the grid lines
                addGridlines(0, 22);

                // Add the blocks
                for (var i = 0; i < blocks.length; i++)
                    addBlock(blocks[i], 0, 22, colorScheme[blocks[i].courseCode + "|" + blocks[i].activityType]);
            });
        }

        return {
            restrict: "E",
            scope: {  blocks: "=", colorscheme: "=", charttitle: "@charttitle" },
            templateUrl: "timetable-preview.template.html",
            link: createTimetable
        };
    });
}());